using Importer.Controller;
using Importer.Controllers;
using System;
using System.IO;

namespace Importer
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return ParseFile();
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during processing STDIN {ex}");
                return 1;
            }
        }

        private static int ParseFile()
        {
            var output = Console.OpenStandardOutput();
            var input = Console.OpenStandardInput();

            InputFileParser parser = new InputFileParser(input);
            parser.ParseFile();
            if (parser.HasErrors)
            {
                using (StreamWriter sw = new StreamWriter(output))
                {
                    parser.ParseErrors.ForEach((err) => sw.WriteLine(err));
                }
                return 1;
            }

            ReportGenerator reporter = new ReportGenerator(parser);
            reporter.SampleReport(output);
            return 0;
        }
    }
}
