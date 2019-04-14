using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
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
            ExplicitWeightRange.Or(
                SingleWeight.Select(weight => new Range<int>(weight)));

        public static readonly Parser<decimal> SinglePrice =
            from dollars in Parse.Digit.AtLeastOnce().Text()
            from period in Parse.Char('.')
            from cents in Parse.Digit.Repeat(2).Text()
            select decimal.Parse(dollars + "." + cents);

        public static readonly Parser<Range<decimal>> ExplicitPriceRange =
            from min in SinglePrice
            from dash in Parse.Char('-')
            from max in SinglePrice
            select new Range<decimal>(min, max);

        public static readonly Parser<Range<decimal>> PriceRange =
            ExplicitPriceRange.Or(
                SinglePrice.Select(price => new Range<decimal>(price)));

        private static readonly Parser<IEnumerable<char>> Separator = 
            Parse.Char(' ').AtLeastOnce();

        public static readonly Parser<string> GradeEntryDescription =
            from leading in Separator
            from text in Parse.Letter.Or(Parse.Char(' ')).AtLeastOnce().Text()
            select text;

        public static readonly Parser<GradeEntry> GradeEntryLine =
            from indent in Parse.Char(' ').Many()
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
            select new GradeEntry(
                head, 
                weightRange, 
                avgWeight, 
                priceRange, 
                avgPrice, 
                description.GetOrElse(""));

        public static readonly Parser<string> GradeInfoDescriptionLine =
            from indent in Parse.Char(' ').Many()
            from description in Parse.CharExcept(new[]{'\n','\r'}).AtLeastOnce().Text()
            from eol in Parse.LineEnd
            select description.Trim();

        public static readonly Parser<GradeInfo> GradeInfo = 
            from description in GradeInfoDescriptionLine
            from tableHeader in Parse.String(" Head   Wt Range   Avg Wt    Price Range   Avg Price")
            from nl in Parse.LineEnd
            from entries in GradeEntryLine.AtLeastOnce()
            from blank in Parse.LineEnd
            select new GradeInfo(description, entries);

        private static readonly Parser<string> Disclaimer =
            from open in Parse.String("***")
            from text in Parse.CharExcept('*').Many()
            from close in Parse.String("***")
            select "";

        private static readonly Parser<string> Header =
            from boringText in Parse.CharExcept('*').Many()
            from disclaimer in Disclaimer
            from ws in Parse.WhiteSpace.Many()
            select "";

        private static readonly Parser<string> Footer =
            from source in Parse.String("Source")
            from rest in Parse.AnyChar.Many()
            select string.Empty;

        public static readonly Parser<IEnumerable<GradeInfo>> Report =
            from header in Header
            from info in GradeInfo.Many()
            from footer in Footer
            select info;
    }
}
