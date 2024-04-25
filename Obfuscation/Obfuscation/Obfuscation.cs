namespace Obfuscation
{
    using System;
    using System.Diagnostics;
    using System.IO;

    class Obfuscation
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the directory path containing .lua files:");
            string directoryPath = Console.ReadLine().Trim();

            while (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
            {
                if (string.IsNullOrEmpty(directoryPath))
                    Console.WriteLine("Input cannot be empty. Please enter a valid directory path:");
                else
                    Console.WriteLine("Directory does not exist. Please enter a valid directory path:");

                directoryPath = Console.ReadLine().Trim();
            }

            Console.WriteLine("Enter the output directory path for the obfuscated files:");
            string outputDirectory = Console.ReadLine().Trim();

            while (string.IsNullOrEmpty(outputDirectory))
            {
                Console.WriteLine("Output directory cannot be empty. Please enter a valid output directory path:");
                outputDirectory = Console.ReadLine().Trim();
            }

            string luaInterpreterPath = @"lua";
            string prometheusCliPath = @"C:\OBUFSCATION\Prometheus-master\cli.lua";

            string[] luaFiles = Directory.GetFiles(directoryPath, "*.lua");

            foreach (string file in luaFiles)
            {
                string arguments = $"{prometheusCliPath} --preset Minify \"{file}\" --out {outputDirectory}\\{Path.GetFileName(file)}";
                Console.WriteLine("Running command: " + luaInterpreterPath + " " + arguments);
                ProcessStartInfo startInfo = new ProcessStartInfo(luaInterpreterPath, arguments)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try
                {
                    using (Process process = Process.Start(startInfo))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        Console.WriteLine(output);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            CleanUpObfuscatedFiles(outputDirectory);

            Console.WriteLine("Obfuscation process completed.");
        }

        static void CleanUpObfuscatedFiles(string directory)
        {
            string[] filesToDelete = Directory.GetFiles(directory, "*.obfuscated.lua");
            foreach (var file in filesToDelete)
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete {file}: {ex.Message}");
                }
            }
        }
    }
}