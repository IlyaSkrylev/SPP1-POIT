using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests_Generator
{
    public class program
    {
        static async Task Main(string[] args)
        {
            var inputDirectory = "D:\\Works\\6 semestr\\SPP\\SPP1-POIT\\Tests Generator\\Test Classes\\Classes";
            var outputDirectory = "D:\\Works\\6 semestr\\SPP\\SPP1-POIT\\Tests Generator\\Test Classes Tests";

            var inputFiles = Directory.GetFiles(inputDirectory, "*.cs");

            var generator = new TestClassGenerator.TestGenerator();
            await generator.GenerateTestsAsync(inputFiles, outputDirectory);

            Console.WriteLine("Генерация завершена.");
        }
    }
}