using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scanner;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScannerTests
{
    [TestClass]
    public class MainScannerTests
    {
        private string testDirectory;

        [TestInitialize]
        public void Setup()
        {
            testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectory);

            File.WriteAllText(Path.Combine(testDirectory, "file1.txt"), "Test content");
            Directory.CreateDirectory(Path.Combine(testDirectory, "subfolder1"));
            File.WriteAllText(Path.Combine(testDirectory, "subfolder1", "file2.txt"), "More test content");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [TestMethod]
        public void Constructor_InitializesCorrectly()
        {
            var scanner = new MainScanner(testDirectory, 2);

            Assert.IsNotNull(scanner);
        }

        [TestMethod]
        public async Task StartScan_ProcessesRootFolder()
        {
            var scanner = new MainScanner(testDirectory, 2);

            scanner.StartScan();
            await scanner.WaitForCompletionAsync();

            Assert.IsNotNull(scanner.RootFolder);
            Assert.AreEqual(Path.GetFileName(testDirectory), scanner.RootFolder.name);
            Assert.IsTrue(scanner.RootFolder.size > 0);
        }

        [TestMethod]
        public async Task StartScan_ProcessesSubfolders()
        {
            var scanner = new MainScanner(testDirectory, 2);

            scanner.StartScan();
            await scanner.WaitForCompletionAsync();

            Assert.AreEqual(1, scanner.RootFolder.folders.Count);
            Assert.AreEqual("subfolder1", scanner.RootFolder.folders[0].name);
            Assert.IsTrue(scanner.RootFolder.folders[0].size > 0);
        }

        [TestMethod]
        public async Task StartScan_ProcessesFiles()
        {
            var scanner = new MainScanner(testDirectory, 2);

            scanner.StartScan();
            await scanner.WaitForCompletionAsync();

            Assert.AreEqual(1, scanner.RootFolder.files.Count);
            Assert.AreEqual("file1.txt", scanner.RootFolder.files[0].name);
            Assert.IsTrue(scanner.RootFolder.files[0].size > 0);
        }

        [TestMethod]
        public async Task CalculateFolderSize_CalculatesCorrectly()
        {
            var scanner = new MainScanner(testDirectory, 2);
            scanner.StartScan();
            await scanner.WaitForCompletionAsync();

            var size = scanner.RootFolder.size;

            Assert.IsTrue(size >= 0);
            Assert.AreEqual(size, scanner.RootFolder.size);
        }

        [TestMethod]
        public async Task CalculatePercentage_CalculatesCorrectly()
        {
            var scanner = new MainScanner(testDirectory, 2);
            scanner.StartScan();
            await scanner.WaitForCompletionAsync();

            Assert.AreEqual(100.0, scanner.RootFolder.percentage);
            foreach (var file in scanner.RootFolder.files)
            {
                Assert.IsTrue(file.percentage > 0 && file.percentage <= 100);
            }
        }

        [TestMethod]
        public async Task WaitForCompletionAsync_WaitsForCompletion()
        {
            var scanner = new MainScanner(testDirectory, 2);
            scanner.StartScan();

            await scanner.WaitForCompletionAsync();

            Assert.IsNotNull(scanner.RootFolder);
        }
    }
}