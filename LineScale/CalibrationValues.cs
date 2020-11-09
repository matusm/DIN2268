//=============================================================
//
// DIN 2268 Längenmaße mit Teilung
//
// class CalibrationValues
//       realisiert alle nullpunktbezogenen Teilungsfehler
//       eines Längenmaßes
//
// Name:          Bezeichnung des Längenmaßes
//
// ScaleMarks:    Feld aller nullpunktbezogenen Teilungsfehler
//
// ColumnHeaderX: Überschrift der ersten csv-Spalte
//
// ColumnHeaderX: Überschrift der zweiten csv-Spalte
//
// AddScaleMark(string): Fügt eine neue Marke hinzu. Die Daten
//                       werden aus der Zeile aus (typisch)
//                       einer csv-Datei erhalten.
// 
//=============================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DIN2268.LineScale
{
    public class CalibrationValues
    {
        public string Name { get; }
        public ScaleMark[] ScaleMarks => markList.ToArray();
        public string ColumnHeaderX { get; private set; }
        public string ColumnHeaderY { get; private set; }
        public string UnitX => ExtractUnit(ColumnHeaderX);
        public string UnitY => ExtractUnit(ColumnHeaderY);

        public CalibrationValues(string name)
        {
            Name = name.Trim();
            markList = new List<ScaleMark>();
        }

        public void AddScaleMark(ScaleMark mark)
        {
            markList.Add(mark);
            markList.Sort();
        }

        public void AddScaleMark(string csvLine)
        {
            string[] token = splitCsvLine(csvLine);
            if (token.Length != 2)
                return;
            if (int.TryParse(token[0], out int length))
            {
                if (double.TryParse(token[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double deviation))
                {
                    AddScaleMark(new ScaleMark(length, deviation));
                    return;
                }
            }
            ColumnHeaderX = token[0].Trim();
            ColumnHeaderY = token[1].Trim();
        }

        private string[] splitCsvLine(string csvLine)
        {
            string[] token = csvLine.Split(
                new[] { ";" },
                StringSplitOptions.RemoveEmptyEntries);
            return token;
        }

        // the final token after "/" is considered as the unit symbol
        private string ExtractUnit(string columnHeader)
        {
            string[] token = columnHeader.Split(
                new[] { "/" },
                StringSplitOptions.RemoveEmptyEntries);
            if (token.Length < 2)
                return "a.u.";
            return token.Last().Trim();
        }

        private readonly List<ScaleMark> markList;

    }
}
