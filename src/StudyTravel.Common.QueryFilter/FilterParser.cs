
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudyTravel.Common.QueryFilter
{
    public class FilterParser
    {
        // look at the tests to understand the pattern
        private const string FilterPattern = "(?<Filter>(?<Function>(startswith|contains|equals|gt|lt|ge|le))(\\(\"(?<PropertyName>.+?)\",\"(?<Value>.+?)\"\\)))(?<Sep>:\\s*$|\\s+(?:or|and|not)\\s+|\\s)";
        private const string StrReplace = @"function:${Function}" + "\n" + @"PropertyName:${PropertyName}" +
                                            "\n" + @"Value:${Value}" + "\n" + @"Sep:${Sep}" + "\n";
        private const char Separator = '\n';

        private const string SingleFilterPattern = "(?<Filter>(?<Function>(startswith|contains|equals|gt|lt|ge|le))(\\(\"(?<PropertyName>.+?)\",\"(?<Value>.+?)\"\\)))";
        private const string SingleStrReplace = @"function:${Function}" + "\n" + @"PropertyName:${PropertyName}" +
                                            "\n" + @"Value:${Value}" + "\n";


        private readonly IList<IFilter> _filterFunctionList = new List<IFilter>();

        public FilterParser(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return;
            filter += " ";

            var myRegex = new Regex(FilterPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            var result = myRegex.Replace(filter, StrReplace);
            var splitResult = result.Split(Separator);

            if (splitResult.Length == 1)
            {
                ParseSingleFilter(splitResult.First());
                return;
            }

            ParseMultipleFilters(splitResult);
        }

        private void ParseSingleFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return;

            var myRegex = new Regex(SingleFilterPattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            var result = myRegex.Replace(filter, SingleStrReplace);
            var splitResult = result.Split(Separator);

            if (splitResult.Length == 1) return;

            ParseMultipleFilters(splitResult);
        }

        private void ParseMultipleFilters(string[] splitResult)
        {
            foreach (var item in splitResult)
            {
                if (string.IsNullOrEmpty(item)) break;

                var splittedItem = item.Split(':');

                var key = FirstCharToUpper(splittedItem[0]);
                var value = splittedItem[1];

                if (key == "Function")
                {
                    try
                    {
                        _filterFunctionList.Add(new FilterFunction
                        {
                            FilterType = (Filter) Enum.Parse(typeof (Filter), FirstCharToUpper(value))
                        });
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"{value} could not parse filter type. Check spelling or case sensitivity!");
                    }
                }
                else if (key == "PropertyName")
                {
                    var latestFilter = _filterFunctionList.Last();
                    latestFilter.PropertyName = FirstCharToUpper(value);
                }
                else if (key == "Value")
                {
                    var latestFilter = _filterFunctionList.Last();
                    latestFilter.Value = value;
                }
                else
                {
                    continue;
                }
            }
        }

        public IList<IFilter> Filters => _filterFunctionList;

        private static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public interface IFilter
    {
        Filter FilterType { get; set; }
        string PropertyName { get; set; }
        object Value { get; set; }
    }

    public class FilterFunction : IFilter
    {
        public Filter FilterType { get; set; }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
