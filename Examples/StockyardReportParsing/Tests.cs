using NUnit.Framework;
using Sprache;

namespace StockyardReportParsing
{
    [TestFixture]
    public class Tests
    {
        [TestCase("123", 123)]
        [TestCase("123   ", 123)]
        public void ShouldParseSingleWeight(string input, int expected)
        {
            Assert.That(ReportParser.SingleWeight.Parse(input), Is.EqualTo(expected));
        }

        [TestCase("1-2", 1, 2)]
        [TestCase("100-200", 100, 200)]
        public void ShouldParseExplicitWeightRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.ExplicitWeightRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(expectedMin));
            Assert.That(range.Max, Is.EqualTo(expectedMax));
        }

        [TestCase("1-2", 1, 2)]
        [TestCase("100-200", 100, 200)]
        [TestCase("123", 123, 123)]
        [TestCase("100-200   ", 100, 200)]
        [TestCase("123   ", 123, 123)]
        public void ShouldParseWeightRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.WeightRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(expectedMin));
            Assert.That(range.Max, Is.EqualTo(expectedMax));
        }

        [TestCase("123.45", 123_45)]
        [TestCase("123.00   ", 123_00)]
        public void ShouldParseSinglePrice(string input, int expected)
        {
            Assert.That(ReportParser.SinglePrice.Parse(input), Is.EqualTo(ToDecimal(expected)));
        }

        [TestCase("1.24-2.78", 1_24, 2_78)]
        [TestCase("100.00-200.00", 100_00, 200_00)]
        public void ShouldParseExplicitPriceRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.ExplicitPriceRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(ToDecimal(expectedMin)));
            Assert.That(range.Max, Is.EqualTo(ToDecimal(expectedMax)));
        }

        [TestCase("100.00-200.00", 100_00, 200_00)]
        [TestCase("123.45", 123_45, 123_45)]
        [TestCase("100.00-200.00   ", 100_00, 200_00)]
        [TestCase("123.00   ", 123_00, 123_00)]
        public void ShouldParsePriceRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.PriceRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(ToDecimal(expectedMin)));
            Assert.That(range.Max, Is.EqualTo(ToDecimal(expectedMax)));
        }

        static decimal ToDecimal(int i) => ((decimal) i) / 100;
    }
}