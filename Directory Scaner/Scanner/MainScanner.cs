using Scanner.Structures;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanner
{
    public class MainScanner
    {
        private int threadsCount;
        private List<Thread> threads;
        private Queue<Action> tasks;
        private Dictionary<int, bool> isThreadWorking;
        public CancellationTokenSource cancellationTokenSource;
        private string rootPath;
        private bool isCompleted = false;
        private int activeTasksCount = 0;
        private object folderLock;
        private ManualResetEvent scanCompleted = new ManualResetEvent(false);

        public FolderItem RootFolder;

        public MainScanner(string pathToFolder, int threadsCount)
        {
            this.threadsCount = threadsCount;
            rootPath = pathToFolder;
            threads = new List<Thread>();
            tasks = new Queue<Action>();
            isThreadWorking = new Dictionary<int, bool>();
            cancellationTokenSource = new CancellationTokenSource();
            folderLock = new object();
        }

        public void StartScan()
        {
            for (int i = 0; i < threadsCount; i++)
            {
                var t = new Thread(DoThreadWork);
                threads.Add(t);
                t.IsBackground = true;
                t.Start();
                isThreadWorking.Add(t.ManagedThreadId, false);
            }

            EnqueueTask(() => ProcessFolder(rootPath, null));
        }

        public async Task WaitForCompletionAsync()
        {
            await Task.Run(() => scanCompleted.WaitOne());

            await Task.Run(() =>
            {
                foreach (var t in threads)
                    t.Join();

                RootFolder.size = CalculateFolderSize(RootFolder);
                CalculatePercentage(RootFolder, RootFolder.size);
            });
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        private void DoThreadWork()
        {
            while (true)
            {
                Action task = DequeueTask();
                if (task == null)
                    break;

                try
                {
                    task();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private Action DequeueTask()
        {
            lock (tasks)
            {
                while (tasks.Count == 0)
                {
                    if (isCompleted)
                        return null; 

                    Monitor.Wait(tasks);
                }

                return tasks.Dequeue();
            }
        }

        private void EnqueueTask(Action action)
        {
            lock (tasks)
            {
                activeTasksCount++;
                tasks.Enqueue(action);
                Monitor.PulseAll(tasks);
            }
        }

        private void ProcessFolder(string path, FolderItem parent)
        {
            if (cancellationTokenSource.Token.IsCancellationRequested)
            {
                DecrementActiveTasksCount();
                return;
            }

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                if (dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    DecrementActiveTasksCount();
                    return;
                }

                FolderItem currentFolder = new FolderItem
                {
                    name = dirInfo.Name,
                    size = 0,
                    folders = new List<FolderItem>(),
                    files = new List<FileItem>()
                };

                if (parent == null)
                {
                    lock (this)
                    {
                        RootFolder = currentFolder;
                    }
                }
                else
                {
                    lock (folderLock)
                    {
                        parent.folders.Add(currentFolder);
                    }
                }

                foreach (var file in dirInfo.GetFiles())
                {
                    if ((file.Attributes & FileAttributes.ReparsePoint) != 0)
                        continue;

                    long size = file.Length;
                    FileItem fileItem = new FileItem
                    {
                        name = file.Name,
                        size = size,
                        percentage = 0
                    };

                    currentFolder.files.Add(fileItem);
                    currentFolder.size += size;
                }

                foreach (var subDir in dirInfo.GetDirectories())
                {
                    EnqueueTask(() => ProcessFolder(subDir.FullName, currentFolder));
                }
            }
            catch
            {
            }
            finally
            {
                DecrementActiveTasksCount();
            }
        }

        private void DecrementActiveTasksCount()
        {
            bool isNeedComplete = false;
            lock (tasks)
            {
                activeTasksCount--;

                if (activeTasksCount == 0 && tasks.Count == 0)
                {
                    isCompleted = true;
                    isNeedComplete = true;
                    Monitor.PulseAll(tasks);
                }
            }
            if (isNeedComplete)
            {
                scanCompleted.Set();
            }
        }

        private long CalculateFolderSize(FolderItem folder)
        {
            folder.size = 0;
            foreach (var f in folder.files)
            {
                folder.size += f.size;
            }

            foreach (var f in folder.folders)
            {
                folder.size += CalculateFolderSize(f);
            }
            return folder.size;
        }

        private void CalculatePercentage(FolderItem folder, long parentFolderSize)
        {
            folder.percentage = Math.Round((double)folder.size / parentFolderSize * 100, 1);
            foreach (var f in folder.files)
            {
                f.percentage = Math.Round((double)f.size / folder.size * 100, 1);
            }

            foreach (var f in folder.folders)
            {
                CalculatePercentage(f, folder.size);
            }
        }
    }
}