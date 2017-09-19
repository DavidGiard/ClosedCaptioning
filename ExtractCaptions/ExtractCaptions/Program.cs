using ExtractCaptions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractCaptions
{
    class Program
    {

        static void Main(string[] args)
        {
            var inputFilePath = "";
            var outputFilePath = "";

            try
            {
                if (!SetParameters(args, ref inputFilePath, ref outputFilePath))
                {
                    ShowUsage();
                    return;
                }

                using (var r = new StreamReader(inputFilePath))
                {
                    string json = r.ReadToEnd();
                    InsightsTop insightJson = JsonConvert.DeserializeObject<InsightsTop>(json);

                    StreamWriter sw = new StreamWriter(outputFilePath);
                    sw.WriteLine("WEBVTT");
                    sw.WriteLine();

                    var breakdowns = insightJson.Breakdowns;
                    foreach (Breakdown breakdown in breakdowns)
                    {
                        Insights insights = breakdown.Insights;
                        TranscriptBlock[] transcriptBlocks = insights.TranscriptBlocks;

                        foreach (TranscriptBlock transcriptBlock in transcriptBlocks)
                        {
                            Line[] lines = transcriptBlock.Lines;
                            foreach (Line line in lines)
                            {
                                string startTime = FormatTime(line.TimeRange.Start); //line.TimeRange.Start.Substring(0, 12); // PadRight(12, zeroCharacter);
                                string endTime = FormatTime(line.TimeRange.End); //.Substring(0, 12); // .PadRight(12, zeroCharacter);
                                decimal confidence = line.Confidence;
                                string text = line.Text;
                                Console.WriteLine("{0} --> {1}", startTime, endTime);
                                Console.WriteLine(text);
                                Console.WriteLine();
                                sw.WriteLine("{0} --> {1}", startTime, endTime);
                                sw.WriteLine(text);
                                sw.WriteLine();

                            }
                        }
                    }
                    sw.Close();
                }

                Console.WriteLine("New file created: {0}", outputFilePath);
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred");
            }
        }

        private static bool SetParameters(string[] args, ref string inputFilePath, ref string outputFilePath)
        {
            if (args.Length > 0)
            {
                inputFilePath = args[0];
            }
            else
            {
                Console.WriteLine("Please provide input file path");
                return false;
            }
            if (args.Length > 1)
            {
                outputFilePath = args[1];
            }
            else
            {
                outputFilePath = GetOutputPath(inputFilePath);
            }

            if (!ParametersAreOK(inputFilePath, outputFilePath))
            {
                return false;
            }

            return true;
        }

        private static bool ParametersAreOK(string inputFilePath, string outputFilePath)
        {
            var returnValue = true;
            var input = inputFilePath.Trim();
            var output  = outputFilePath.Trim();
            if (input == "")
            {
                Console.WriteLine("No input file specified");
                returnValue = false;
            }
            if (input == "")
            {
                Console.WriteLine("No input file specified");
                returnValue = false;
            }
            if (input == output )
            {
                Console.WriteLine("Input file ({0}) cannot be the same", input);
                Console.WriteLine("as output file ({0})", output );
                returnValue = false;
            }
            return returnValue;

        }

        private static string FormatTime(string timeString)
        {
            if(timeString == "00:00:00")
            {
                return "00:00:00.000";
            }
            char zeroCharacter = "0".ToCharArray()[0];
            var newTime = timeString.PadRight(12, zeroCharacter);
            newTime =newTime.Substring(0, 12);
            return newTime; 
        }

        private static string GetOutputPath(string startFilePath)
        {
            var startFileName = Path.GetFileName(startFilePath);
            var startFileRootName = Path.GetFileNameWithoutExtension(startFileName);
            var folder = Path.GetDirectoryName(startFilePath);
            var outputFilePath = Path.Combine(folder, startFileRootName);
            outputFilePath += ".vtt";
            var outputFile = Path.Combine(folder, outputFilePath);
            return outputFilePath;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("ExtractCaptions [sourceFile] [outputFile]");
            Console.WriteLine("sourceFile: Full or relative path of JSON file to start with");
            Console.WriteLine("            This must be a JSON file in the format returned by https://videoindexer.ai");
            Console.WriteLine("outputFile (Optional): Full or relative path of the file to create");
            Console.WriteLine("            If omitted, a file will be created in the folder of the sourceFile");
            Console.WriteLine("            with the same root file name and the extension '.vtt'");

        }
    }
}
