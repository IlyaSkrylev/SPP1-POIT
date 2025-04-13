using Scanner;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Directory_Scaner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
            //scanner = null;
            /*int[] time = new int[5];
            for (int i = 1; i <= 5; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                var scanner = new MainScanner("D:\\Works\\6 semestr\\SPP\\Directory Scaner\\Temp", i);
                scanner.StartScan();

                // Асинхронное ожидание завершения
                await scanner.WaitForCompletionAsync();
                var info = scanner.RootFolder;

                // Или если вы не можете использовать await:
                scanner.WaitForCompletionAsync().ContinueWith(task =>
                {
                    var info = scanner.RootFolder;
                    // Здесь код, который выполнится после завершения сканирования
                });
                sw.Stop();
                time[i - 1] = (int)sw.ElapsedMilliseconds;
            }
            int j = 43;*/
        }

        /*public void btnCancel_Click(object o, RoutedEventArgs e)
        {
            if (scanner != null)
            {
                scanner.Cancel();
            }
        }

        public void btnStart_Click(object o, RoutedEventArgs e)
        {
            ScanDirectory("D:\\Works\\6 semestr\\SPP\\Directory Scaner\\Temp");
        }

        public async void ScanDirectory(string path)
        {
            scanner = new MainScanner(path, 5);
            scanner.StartScan();

            // Асинхронное ожидание завершения
            await scanner.WaitForCompletionAsync();

            // Или если вы не можете использовать await:
            scanner.WaitForCompletionAsync().ContinueWith(task =>
            {
                var info = scanner.RootFolder;
                int j = 0;
            });
        }*/
    }
}
