using NUnit.Framework;
using Sprache;

namespace StockyardReportParsing
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldParseSingleWeight()
        {
            Assert.That(ReportParser.SingleWeight.Parse("123"), Is.EqualTo(123));
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
        public void ShouldParseWeightRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.WeightRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(expectedMin));
            Assert.That(range.Max, Is.EqualTo(expectedMax));
        }

        [Test]
        public void ShouldParseSinglePrice()
        {
            Assert.That(ReportParser.SinglePrice.Parse("123.45"), Is.EqualTo(ToDecimal(123_45)));
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
        public void ShouldParsePriceRange(string input, int expectedMin, int expectedMax)
        {
            var range = ReportParser.PriceRange.Parse(input);
            Assert.That(range.Min, Is.EqualTo(ToDecimal(expectedMin)));
            Assert.That(range.Max, Is.EqualTo(ToDecimal(expectedMax)));
        }

        [Test]
        public void ShouldParseGradeEntry()
        {
            var entry = ReportParser.GradeEntry.Parse("1     1095      1095       115.00         115.00\n");

            Assert.That(entry.Head, Is.EqualTo(1));

            Assert.That(entry.Weight.Min, Is.EqualTo(1095));
            Assert.That(entry.Weight.Max, Is.EqualTo(1095));
            Assert.That(entry.AverageWeight, Is.EqualTo(1095));

            Assert.That(entry.Price.Min, Is.EqualTo(115.00m));
            Assert.That(entry.Price.Max, Is.EqualTo(115.00m));
            Assert.That(entry.AveragePrice, Is.EqualTo(115.00m));
        }

        [Test]
        public void ShouldParseGradeEntryWithRanges()
        {
            var entry = ReportParser.GradeEntry.Parse("20   1300-1485   1399    117.00-126.50     125.32\n");

            Assert.That(entry.Head, Is.EqualTo(20));

            Assert.That(entry.Weight.Min, Is.EqualTo(1300));
            Assert.That(entry.Weight.Max, Is.EqualTo(1485));
            Assert.That(entry.AverageWeight, Is.EqualTo(1399));

            Assert.That(entry.Price.Min, Is.EqualTo(117.00m));
            Assert.That(entry.Price.Max, Is.EqualTo(126.50m));
            Assert.That(entry.AveragePrice, Is.EqualTo(125.32m));
        }

        [Test]
        public void ShouldParseGradeEntryWithDescription()
        {
            var entry = ReportParser.GradeEntry.Parse("1     1095      1095       115.00         115.00 Some text\n");

            Assert.That(entry.Description, Is.EqualTo("Some text"));
        }

        static decimal ToDecimal(int i) => ((decimal)i) / 100;
    }
}