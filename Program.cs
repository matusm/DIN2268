using System;
using System.IO;
using System.Linq;
using DIN2268.LineScale;

namespace DIN2268
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string userInput;
            if (args.Length == 0)
                userInput = "MM004158_2020_2mm.csv";
            else
                userInput = args[0];
            string baseFileName = Path.GetFileNameWithoutExtension(userInput);
            string inputFileName = Path.ChangeExtension(baseFileName, ".csv");
            string outputFileName = Path.ChangeExtension(baseFileName + "_processed", ".csv");

            CalibrationValues rawData = new CalibrationValues($"Data from {inputFileName}");
            try
            {
                StreamReader hFile = File.OpenText(inputFileName);
                string line;
                while ((line = hFile.ReadLine()) != null)
                {
                    rawData.AddScaleMark(line);
                }
                hFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            if (rawData.ScaleMarks.Length <= 1)
            {
                Console.WriteLine("no data loaded!");
                return;
            }

            // calculate

            LineScaleAnalysis result = new LineScaleAnalysis();

            for (int i = 0; i < rawData.ScaleMarks.Length; i++)
            {
                for (int j = i + 1; j < rawData.ScaleMarks.Length; j++)
                {
                    ScaleMark mark1 = rawData.ScaleMarks[i];
                    ScaleMark mark2 = rawData.ScaleMarks[j];
                    ScaleMark distance = mark1.DistanceTo(mark2);
                    result.AddNewData(distance);
                }
            }

            LineScaleCharacteristics characteristics = new LineScaleCharacteristics(result);

            Console.WriteLine("Kenngrößen nach DIN 2268");
            Console.WriteLine($"Anzahl der Teilungsmarken: {rawData.ScaleMarks.Length}");
            Console.WriteLine($"Gesamtteilungslänge: {rawData.ScaleMarks.Last().NominalLength} {rawData.UnitX}");
            Console.WriteLine($"Teilungsschritt: {characteristics.B.Length} {rawData.UnitX}");
            Console.WriteLine();
            Console.WriteLine("Maximaler Teilungsfehler und sein Teilungsabschnitt");
            Console.WriteLine($"   A({characteristics.A.Deviation:F3} {rawData.UnitY} ; {characteristics.A.Length} {rawData.UnitX})");
            Console.WriteLine("Maximaler Teilungsfehler der Teilungsschritte");
            Console.WriteLine($"   B({characteristics.B.Deviation:F3} {rawData.UnitY} ; {characteristics.B.Length} {rawData.UnitX})");
            Console.WriteLine("Maximale Änderung der Teilungsfehler und die zugehörige Länge");
            Console.WriteLine($"   C({characteristics.C.Deviation} {rawData.UnitY}/{rawData.UnitX} ; {characteristics.C.Length} {rawData.UnitX})");

            // finaly write the output file
            try
            {
                StreamWriter outFile = new StreamWriter(outputFileName);
                string csvCaptionLine = "length ; maxDeviation";
                outFile.WriteLine(csvCaptionLine);
                for (int i = 0; i < characteristics.MaxAbsDeviations.Length; i++)
                {
                    var data = characteristics.MaxAbsDeviations[i];
                    string csvLine = $"{data.Length} ; {data.Deviation:F3}";
                    outFile.WriteLine(csvLine);
                }
                outFile.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing output: {ex.Message}");
                return;
            }
        }
    }
}

