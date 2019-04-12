using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace StockyardReportParsing
{
    [DebuggerDisplay("{ToString()}")]
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

        public override string ToString() => $"{Min}-{Max}";
    }

    [DebuggerDisplay("{ToString()}")]
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

        public override string ToString() =>
            $"Head: {Head}, Weight: {Weight} ({AverageWeight} avg), Price: {Price} ({AveragePrice} avg), Description: {Description}";
    }

    [DebuggerDisplay("{ToString()}")]
    sealed class GradeInfo
    {
        public string Description { get; }
        public ImmutableList<GradeEntry> Entries { get; }

        public GradeInfo(string description, IEnumerable<GradeEntry> entries)
        {
            Description = description;
            Entries = entries.ToImmutableList();
        }

        public override string ToString() => $"{Description} ({Entries.Count} entries)";
    }
}
