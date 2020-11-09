//=============================================================
//
// DIN 2268 Längenmaße mit Teilung
//
// class DivisionStatistics
//       realisiert 2.4 und 2.6 Teilungsfehler
//
// NominalSectionLength:     Nenn-Abstand zwischen zwei Marken
//                           Nach Norm in der Einheit m
//
// MaximumAbsoluteDeviation: größter absoluter "Fehler" für
//                           diese Nennlänge
//                           Nach Norm in der Einheit µm
//
// MaximumDeviation:
//
// MinimumDeviation:
// 
// AverageDeviation:
// 
// NumberOfCombinations:
// 
// UpdateDataPoint(double): ein Fehler für diese Nennlänge
// 
//=============================================================

using System;
using At.Matus.DataSeriesPod;
namespace DIN2268.LineScale
{
    public class DivisionStatistics : IComparable<DivisionStatistics>
    {
        public int NominalSectionLength { get; }
        public double MaximumAbsoluteDeviation => Math.Max(MaximumDeviation, Math.Abs(MinimumDeviation));
        public double MaximumDeviation => deviationData.MaximumValue;
        public double MinimumDeviation => deviationData.MinimumValue;
        public double AverageDeviation => deviationData.AverageValue;
        public int NumberOfCombinations => (int)deviationData.SampleSize;

        public DivisionStatistics(int length)
        {
            NominalSectionLength = length;
            deviationData = new DataSeriesPod(NominalSectionLength.ToString());
        }

        public void UpdateDataPoint(double deviation)
        {
            deviationData.Update(deviation);
        }

        public int CompareTo(DivisionStatistics other)
        {
            return NominalSectionLength.CompareTo(other.NominalSectionLength);
        }

        public override string ToString()
        {
            return $"{NominalSectionLength}: {MinimumDeviation:F3}  {AverageDeviation:F3}  {MaximumDeviation:F3}";
        }

        private readonly DataSeriesPod deviationData;

    }
}
