using System.Collections.Generic;

namespace DIN2268.LineScale
{
    public class LineScaleAnalysis
    {

        public DivisionStatistics[] Summary => sections.ToArray();

        public LineScaleAnalysis()
        {
            sections = new List<DivisionStatistics>();
        }

        public void AddNewData(ScaleMark mark)
        {
            if (IsNewEntry(mark))
            {
                sections.Add(new DivisionStatistics(mark.NominalLength));
                sections.Sort();
            }
            UpdateData(mark);
        }

        private void UpdateData(ScaleMark mark)
        {
            foreach (var section in sections)
            {
                if (section.NominalSectionLength == mark.NominalLength)
                {
                    section.UpdateDataPoint(mark.Deviation);
                    return;
                }
            }
        }

        private bool IsNewEntry(ScaleMark mark)
        {
            if (sections.Count == 0) return true;
            foreach (var section in sections)
            {
                if (section.NominalSectionLength == mark.NominalLength)
                    return false;
            }
            return true;
        }

        private readonly List<DivisionStatistics> sections;

    }
}

