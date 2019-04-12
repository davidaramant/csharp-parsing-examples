using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace StockyardReportParsing
{
    static class StringHelperExtensions
    {
        public static string ConvertToString(this IEnumerable<char> chars) => new string(chars.ToArray());
    }

    static class ReportParser
    {
        public static Parser<int> SingleWeight =
            from digits in Parse.Digit.AtLeastOnce()
            from trailingWs in Parse.WhiteSpace.Optional()
            select int.Parse(digits.ConvertToString());

        public static Parser<Range<int>> ExplicitWeightRange =
            from min in SingleWeight
            from dash in Parse.Char('-')
            from max in SingleWeight
            select new Range<int>(min, max);

        public static Parser<Range<int>> WeightRange = 
            ExplicitWeightRange.Or(SingleWeight.Select(weight => new Range<int>(weight)));

        public static Parser<decimal> SinglePrice =
            from digits in Parse.Digit.AtLeastOnce()
            from period in Parse.Char('.')
            from cents in Parse.Digit.Repeat(2)
            from trailingWs in Parse.WhiteSpace.Optional()
            select decimal.Parse(digits.ConvertToString() + "." + cents.ConvertToString());

        public static Parser<Range<decimal>> ExplicitPriceRange =
            from min in SinglePrice
            from dash in Parse.Char('-')
            from max in SinglePrice
            select new Range<decimal>(min, max);

        public static Parser<Range<decimal>> PriceRange = 
            ExplicitPriceRange.Or(SinglePrice.Select(price => new Range<decimal>(price)));
    }
}
