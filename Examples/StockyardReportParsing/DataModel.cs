using System.Collections.Generic;

namespace StockyardReportParsing
{
    sealed class Range<T>
    {
        public T Min { get; }
        public T Max { get; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public Range(T value) : this(value, value)
        {
        }
    }

    sealed class GradeEntry
    {
        public int Head { get; }
        public Range<int> Weight { get; }
        public int AverageWeight { get; }
        public Range<decimal> Price { get; }
        public decimal AveragePrice { get; }
        public string Description { get; }

        public GradeEntry(
            int head,
            Range<int> weight,
            int averageWeight,
            Range<decimal> price,
            decimal averagePrice,
            string description)
        {
            Head = head;
            Weight = weight;
            AverageWeight = averageWeight;
            Price = price;
            AveragePrice = averagePrice;
            Description = description;
        }
    }

    sealed class GradeInfo
    {
        public string Description { get; }
        public IEnumerable<GradeEntry> Entries { get; }

        public GradeInfo(string description, IEnumerable<GradeEntry> entries)
        {
            Description = description;
            Entries = entries;
        }
    }
}
