using System.Collections.Generic;
using Sprache;

namespace StockyardReportParsing
{
    static class ReportParser
    {
        public static readonly Parser<int> Integer = 
            Parse.Digit.AtLeastOnce().Text().Select(int.Parse);

        public static readonly Parser<int> SingleWeight = Integer;

        public static readonly Parser<Range<int>> ExplicitWeightRange =
            from min in SingleWeight
            from dash in Parse.Char('-')
            from max in SingleWeight
            select new Range<int>(min, max);

        public static readonly Parser<Range<int>> WeightRange =
            ExplicitWeightRange.Or(SingleWeight.Select(weight => new Range<int>(weight)));

        public static readonly Parser<decimal> SinglePrice =
            from digits in Parse.Digit.AtLeastOnce().Text()
            from period in Parse.Char('.')
            from cents in Parse.Digit.Repeat(2).Text()
            select decimal.Parse(digits + "." + cents);

        public static readonly Parser<Range<decimal>> ExplicitPriceRange =
            from min in SinglePrice
            from dash in Parse.Char('-')
            from max in SinglePrice
            select new Range<decimal>(min, max);

        public static readonly Parser<Range<decimal>> PriceRange =
            ExplicitPriceRange.Or(SinglePrice.Select(price => new Range<decimal>(price)));

        private static readonly Parser<IEnumerable<char>> Separator = Parse.Char(' ').AtLeastOnce();

        public static readonly Parser<string> GradeEntryDescription =
            from leading in Parse.Char(' ').AtLeastOnce()
            from text in Parse.Letter.Or(Parse.Char(' ')).AtLeastOnce().Text()
            select text;

        public static readonly Parser<GradeEntry> GradeEntry =
            from head in Integer
            from _1 in Separator
            from weightRange in WeightRange
            from _2 in Separator
            from avgWeight in SingleWeight
            from _3 in Separator
            from priceRange in PriceRange
            from _4 in Separator
            from avgPrice in SinglePrice
            from description in GradeEntryDescription.Optional()
            from eol in Parse.LineEnd
            select new GradeEntry(head, weightRange, avgWeight, priceRange, avgPrice, description.GetOrElse(""));
    }
}
