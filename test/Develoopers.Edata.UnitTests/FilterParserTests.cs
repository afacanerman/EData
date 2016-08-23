using FluentAssertions;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Develoopers.Edata.UnitTest
{
    public class FilterParserTests
    {

        [Fact]
        public void Should_not_parse_filter_for_contains_if_has_whitespace_between_property_and_query()
        {
            var stFilter = new FilterParser("contains(\"property1\", \"query\")&");

            stFilter.Filters.Count().Should().Be(0);
        }

        [Fact]
        public void Should_not_parse_filter_for_contains()
        {
            var stFilter = new FilterParser("contains(\'property1\',\"query\")&");

            stFilter.Filters.Count().Should().Be(0);
        }

        [Fact]
        public void Should_parse_filter_for_startswith()
        {
            var stFilter = new FilterParser("startsWith(\"property1\",\"query\")");


            stFilter.Filters.Should().NotBeNull();
            stFilter.Filters.Any(p => p.FilterType == Filter.StartsWith).Should().BeTrue();

            stFilter.Filters.First().PropertyName.Should().Be("Property1");
            stFilter.Filters.First().Value.Should().Be("query");
        }

        [Fact]
        public void Should_parse_filter_for_startswith_if_has_whitespace_between_property_and_query()
        {
            var stFilter = new FilterParser("startswith(\"property1\", \"query\")&");

            stFilter.Filters.Count().Should().Be(0);
        }

        [Fact]
        public void Should_not_parse_filter_for_startswith()
        {
            var stFilter = new FilterParser("startswith(\'property1\',\"query\")&");

            stFilter.Filters.Count().Should().Be(0);

        }
        [Fact]
        public void Should_parse_filter_for_equals()
        {
            var stFilter = new FilterParser("equals(\"property1\",\"query\")");

            stFilter.Filters.Count().Should().Be(1);

        }

        [Fact]
        public void Should_parse_filter_for_greaterThan()
        {
            var stFilter = new FilterParser("gt(\"property1\",\"query\")");

            stFilter.Filters.Count().Should().Be(1);
        }

        [Fact]
        public void Should_parse_filter_for_lessThan()
        {
            var stFilter = new FilterParser("lt(\"property1\",\"query\")");

            stFilter.Filters.Count().Should().Be(1);
        }

        [Fact]
        public void Should_parse_filter_for_lessThanOrEqual()
        {
            var stFilter = new FilterParser("le(\"property1\",\"query\")");

            stFilter.Filters.Count().Should().Be(1);
        }

        [Fact]
        public void Should_parse_filter_for_greaterThanOrEqual()
        {
            var stFilter = new FilterParser("ge(\"property1\",\"query\")");

            stFilter.Filters.Count().Should().Be(1);
        }


        [Fact]
        public void should_match_filter_Odata_convention()
        {
            string strRegex = @"(?<Filter>" +
                              "\n" + @"     (?<Resource>.+?)\s+" +
                              "\n" + @"     (?<Operator>eq|ne|gt|lt|ge)\s+" +
                              "\n" + @"     '?(?<Value>.+?)'?)" +

                              "\n" + @"(?<Sep>:" +
                              "\n" + @"    \s*$" +
                              "\n" + @"   |\s+(?:or|and|not)\s+|\s" +
                              "\n" + @")" +
                              "\n";

            //            "\n" + @")|(?<Func>" +
            //"\n" + @"     (?<Name>.+?)(startswith|contains)" +
            //"\n" + @"     '?(?<Text>\(.*\)?)'?" +
            //"\n" + @"))" +

            Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            string strTargetString = @"name eq 'Facebook' or name eq 'Twitter' and subscribers gt '30' ";
            string strReplace = @"Filter >> ${Filter}" + "\n" + @"Resource:${Resource}" + "\n" + @"Operator:${Operator}" +
                                "\n" + @"Value:${Value}" + "\n" + @"Sep:${Sep}" + "\n";

            var result = myRegex.Replace(strTargetString, strReplace);
            var splitResult = result.Split('\n');

        }

        [Fact]
        public void Should_match_multiple_function_regexp_test()
        {
            var pattern = @"(?<Filter>
                            (?<Function>(startswith|contains|equals|gt|lt|ge))
                            (\('(?<PropertyName>.+?)','(?<Value>.+?)'\)))
                            (?<Sep>:
                                \s*$
                               |\s+(?:or|and|not)\s+|\s
                            )";

            string strTargetString = @"contains('erman','tat') and startswith('test','test') and equals('test','1') and";
            string strReplace = @"function:${Function}" + "\n" + @"PropertyName:${PropertyName}" +
                                "\n" + @"Value:${Value}" + "\n" + @"Sep:${Sep}" + "\n";

            Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            var result = myRegex.Replace(strTargetString, strReplace);
            var splitResult = result.Split('\n');
        }

        [Fact]
        public void Should_match_single_function_regexp_test()
        {
            var pattern = @"(?<Filter>
                            (?<Function>(startswith|contains|equals))
                            (\('(?<PropertyName>.+?)','(?<Value>.+?)'\)))";

            string strTargetString = @"contains('erman','tat') and";
            string strReplace = @"function:${Function}" + "\n" + @"PropertyName:${PropertyName}" +
                                "\n" + @"Value:${Value}" + "\n" + @"Sep:${Sep}" + "\n";

            Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            var result = myRegex.Replace(strTargetString, strReplace);
            var splitResult = result.Split('\n');
        }

        [Fact]
        public void Should_parse_more_than_one_filter()
        {
            var stFilter = new FilterParser("Contains(\"erman\",\"tat\") and StartsWith(\"test\",\"test\") and Equals(\"test\",\"1\")");
            stFilter.Filters.Count.Should().Be(3);

            var first = stFilter.Filters.First();
            first.FilterType.Should().Be(Filter.Contains);
            first.PropertyName.Should().Be("Erman");
            first.Value.Should().Be("tat");

            var last = stFilter.Filters.Last();
            last.FilterType.Should().Be(Filter.Equals);
            last.PropertyName.Should().Be("Test");
            last.Value.Should().Be("1");
        }
    }
}