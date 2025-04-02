using Scanner.Structures;
using Scanner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Directory_Scaner
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainScanner _scanner;
        private string _scanStatus;
        private bool _isScanning;
        private const int threadsCount = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand StartScanCommand { get; }
        public ICommand CancelScanCommand { get; }

        private ObservableCollection<FolderViewModel> _directoryStructure;
        public ObservableCollection<FolderViewModel> DirectoryStructure
        {
            get => _directoryStructure;
            set
            {
                _directoryStructure = value;
                OnPropertyChanged();
            }
        }

        public string ScanStatus
        {
            get => _scanStatus;
            set
            {
                _scanStatus = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            StartScanCommand = new RelayCommand(StartScan, () => !_isScanning);
            CancelScanCommand = new RelayCommand(CancelScan, () => _isScanning);
        }

        private async void StartScan()
        {
            try
            {

                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Выберите папку";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string selectedPath = folderDialog.SelectedPath;
                        _isScanning = true;
                        ScanStatus = "Scanning...";

                        _scanner = new MainScanner(selectedPath, threadsCount);
                        _scanner.StartScan();

                        await _scanner.WaitForCompletionAsync();

                        var rootFolder = _scanner.RootFolder;
                        if (rootFolder != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Root folder size: {rootFolder.size}");
                            System.Diagnostics.Debug.WriteLine($"Subfolders count: {rootFolder.folders.Count}");
                            System.Diagnostics.Debug.WriteLine($"Files count: {rootFolder.files.Count}");
                        }

                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            DirectoryStructure = new ObservableCollection<FolderViewModel>
                            {
                        ConvertToViewModel(_scanner.RootFolder)
                            };
                            _isScanning = false;
                            ScanStatus = "Scan completed";
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                ScanStatus = "Error occurred during scan";
            }
        }

        private void CancelScan()
        {
            _scanner?.Cancel();
            ScanStatus = "Scan cancelled";
            _isScanning = false;
        }

        private FolderViewModel ConvertToViewModel(FolderItem folder)
        {
            var folderVM = new FolderViewModel
            {
                Name = folder.name,
                Size = folder.size,
                Percentage = folder.percentage
            };

            foreach (var file in folder.files)
            {
                folderVM.Children.Add(new FileViewModel
                {
                    Name = file.name,
                    Size = file.size,
                    Percentage = file.percentage
                });
            }

            foreach (var subFolder in folder.folders)
            {
                folderVM.Children.Add(ConvertToViewModel(subFolder));
            }

            return folderVM;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
