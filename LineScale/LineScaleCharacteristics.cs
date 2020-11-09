namespace DIN2268.LineScale
{
    public class LineScaleCharacteristics
    {
        public CharacteristicParameter[] MaxAbsDeviations { get; private set; }
        public CharacteristicParameter A => MaxAbsDeviations[indexOfMaxDeviation];
        public CharacteristicParameter B => MaxAbsDeviations[0];
        public CharacteristicParameter C { get; private set; }

        public LineScaleCharacteristics(LineScaleAnalysis analysis)
        {
            MaxAbsDeviations = new CharacteristicParameter[analysis.Summary.Length];
            Generate_2_6_1(analysis);
            Generate_2_6_3();
        }

        private void Generate_2_6_1(LineScaleAnalysis analysis)
        {
            indexOfMaxDeviation = 0;
            double maxDeviation = 0.0;
            for (int i = 0; i < MaxAbsDeviations.Length; i++)
            {
                double mad = analysis.Summary[i].MaximumAbsoluteDeviation;
                int length = analysis.Summary[i].NominalSectionLength;
                MaxAbsDeviations[i] = new CharacteristicParameter(mad, length);
                if (mad > maxDeviation)
                {
                    maxDeviation = mad;
                    indexOfMaxDeviation = i;
                }
            }
        }

        private void Generate_2_6_3()
        {
            double cStarMax = 0.0;
            C = new CharacteristicParameter(0.0, 0);
            for (int i = 0; i < MaxAbsDeviations.Length; i++)
            {
                double cStar = (MaxAbsDeviations[i].Deviation - B.Deviation) / MaxAbsDeviations[i].Length;
                if (cStar > cStarMax)
                {
                    cStarMax = cStar;
                    C = new CharacteristicParameter(cStar, MaxAbsDeviations[i].Length);
                }
            }
        }

        private int indexOfMaxDeviation;
    }

    public struct CharacteristicParameter
    {
        public CharacteristicParameter(double deviation, int length)
        {
            Deviation = deviation;
            Length = length;
        }

        public double Deviation { get; set; }
        public int Length { get; set; }
    }
}

