//=============================================================
//
// DIN 2268 Längenmaße mit Teilung
//
// class ScaleMark
//       realisiert 2.5 Nullpunktbezogener Teilungsfehler
//
// NominalLength: Nenn-Abstand der Marke von der Nullmarke
//                auch Nenn-Abstand zwischen zwei Marken.
//                Nach Norm in der Einheit m
//
// Deviation:     "Fehler" dieser Marke
//                Nach Norm in der Einheit µm
//
// DistanceTo(ScaleMark): Bestimmt den Teilungsabschnitt (mit
// Fehler) zwischen zwei nullpunktsbezogenen Teilstrichen.
// 
//=============================================================

using System;
namespace DIN2268.LineScale
{
    public class ScaleMark : IComparable<ScaleMark>
    {

        public int NominalLength { get; }   // nominal distance from zero-mark
        public double Deviation { get; }    // deviation from nominal distance

        public ScaleMark(int length, double deviation)
        {
            NominalLength = length;
            Deviation = deviation;
        }

        // get the deviation for a sub-interval
        // "other" must be longer than "this"
        public ScaleMark DistanceTo(ScaleMark other)
        {
            int newLength = other.NominalLength - NominalLength;
            double newDeviation = other.Deviation - Deviation;
            if (CompareTo(other) < 0) // this instance precedes other in the sort order.
            {
                return new ScaleMark(newLength, newDeviation);
            }
            return new ScaleMark(-newLength, -newDeviation); // TODO really?
        }

        public int CompareTo(ScaleMark other)
        {
            return NominalLength.CompareTo(other.NominalLength);
        }

        public override string ToString()
        {
            return $"{NominalLength} : {Deviation}";
        }

    }
}
