using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Develoopers.Edata.Test
{
    public class QueryBuilderTests
    {
        [Fact]
        public void Should_return_equals_match_for_int()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 2, Name = "afacan"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };

            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { Filter = "equals(\"Id\",\"2\")" }).ToList();

            result.Count.Should().Be(1);
            result.First().Name.Should().Be("afacan");
        }

        [Fact]
        public void Should_return_equals_match_for_string()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 2, Name = "afacan"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };

            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { Filter = "equals(\"Name\",\"erman\")" }).ToList();

            result.Count.Should().Be(1);
            result.First().Name.Should().Be("erman");
        }

        [Fact]
        public void Should_return_equals_and_startswith_for_version2()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 2, Name = "afacan"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };

            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { Filter = "equals(\"Id\",\"1\") and StartsWith(\"name\",\"erm\")" }).ToList();

            result.Count.Should().Be(1);
            result.First().Name.Should().Be("erman");
        }

        [Fact]
        public void Should_return_equals_match_for_string_as_version2()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 2, Name = "afacan"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };

            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { Filter = "equals(\"Id\",\"1\") and startsWith(\"name\",\"erm\")" }).ToList();

            result.Count.Should().Be(1);
            result.First().Name.Should().Be("erman");
        }

        [Fact]
        public void Should_return_equals_match_for_single_filter_as_version2()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 2, Name = "afacan"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };

            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { Filter = "equals(\"Id\",\"1\")" }).ToList();

            result.Count.Should().Be(1);
            result.First().Name.Should().Be("erman");
        }

        [Fact]
        public void Should_sort_by_for_multiple_properties()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman2"},
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 1, Name = "erman3"},
                new TestModel {Id = 2, Name = "afacan5"},
                new TestModel {Id = 2, Name = "afacan1"},
                new TestModel {Id = 2, Name = "afacan3"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };


            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { SortBy = "Name,Id", Desc = true }).ToList();
        }

        [Fact]
        public void Should_sort_by_for_single_properties()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman2"},
                new TestModel {Id = 1, Name = "erman"},
                new TestModel {Id = 1, Name = "erman3"},
                new TestModel {Id = 2, Name = "afacan5"},
                new TestModel {Id = 2, Name = "afacan1"},
                new TestModel {Id = 2, Name = "afacan3"},
                new TestModel {Id = 3, Name = "bob"},
                new TestModel {Id = 4, Name = "john"},
            };


            var result = QueryFilterBuilder<TestModel>.Build(dummyList.AsQueryable(),
                new DataQueryModel { SortBy = "Name", Desc = true }).ToList();
        }

        [Fact]
        public void Should_create_query_for_nested_properties()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "data2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan5",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "data3"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 4,
                    Name = "john",
                    NestedObj = new NestedObj {Id = 6, AnotherObj = new AnotherObj {Data = "data4"}}
                },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "equals(\"NestedObj.AnotherObj.Data\",\"data\")" });
            query.Count().Should().Be(3);
        }

        [Fact]
        public void Should_create_query_for_nested_properties_with_some_are_null()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "data2"}}
                },
                new TestModel {Id = 2, Name = "afacan5", NestedObj = new NestedObj {Id = 3}},
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel {Id = 4, Name = "john", NestedObj = new NestedObj {Id = 6}},
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "equals(\"NestedObj.AnotherObj.Data\",\"data\")" });
            query.Count().Should().Be(3);
        }

        [Fact]
        public void Should_create_query_for_nested_nullable_properties()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, OrgType = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "data2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan5",
                    NestedObj = new NestedObj {Id = 3, OrgType = 1, AnotherObj = new AnotherObj {Data = "data3"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "data4"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "data"}}
                },
                new TestModel
                {
                    Id = 4,
                    Name = "john",
                    NestedObj = new NestedObj {Id = 6, AnotherObj = new AnotherObj {Data = "data4"}}
                },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "equals(\"NestedObj.OrgType\",\"1\")" });
            query.Count().Should().Be(2);
        }

        [Fact]
        public void Should_return_matched_item_with_single_filter()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, Name = "erman2", OrganisationType = 1},
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "Equals(\"OrganisationType\",\"1\")" });
            query.Count().Should().Be(1);
        }

        [Fact]
        public void Should_create_query_with_nested_sortBy()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, OrgType = 1, AnotherObj = new AnotherObj {Data = "a1"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "a3"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan5",
                    NestedObj = new NestedObj {Id = 3, OrgType = 1, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "a4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "a6"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "a7"}}
                },
                new TestModel
                {
                    Id = 4,
                    Name = "john",
                    NestedObj = new NestedObj {Id = 6, AnotherObj = new AnotherObj {Data = "a8"}}
                },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { SortBy = "NestedObj.AnotherObj.Data" });
            var data = query.ToList();

            data.Count.Should().Be(8);
            data[0].NestedObj.AnotherObj.Data.Should().Be("a1");
            data[1].NestedObj.AnotherObj.Data.Should().Be("a2");
            data[2].NestedObj.AnotherObj.Data.Should().Be("a2");
            data[3].NestedObj.AnotherObj.Data.Should().Be("a3");
            data[4].NestedObj.AnotherObj.Data.Should().Be("a4");
            data[5].NestedObj.AnotherObj.Data.Should().Be("a6");
            data[6].NestedObj.AnotherObj.Data.Should().Be("a7");
            data[7].NestedObj.AnotherObj.Data.Should().Be("a8");
        }


        [Fact]
        public void Should_create_query_with_take()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, OrgType = 1, AnotherObj = new AnotherObj {Data = "a1"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "a3"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan5",
                    NestedObj = new NestedObj {Id = 3, OrgType = 1, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "a4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "a6"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "a7"}}
                },
                new TestModel
                {
                    Id = 4,
                    Name = "john",
                    NestedObj = new NestedObj {Id = 6, AnotherObj = new AnotherObj {Data = "a8"}}
                },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Take = 2 });
            var data = query.ToList();

            data.Count.Should().Be(2);
            data[0].Name.Should().Be("erman2");
            data[1].Name.Should().Be("erman");
        }

        [Fact]
        public void Should_create_query_with_skip_and_take()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    Name = "erman2",
                    NestedObj = new NestedObj {Id = 1, OrgType = 1, AnotherObj = new AnotherObj {Data = "a1"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman",
                    NestedObj = new NestedObj {Id = 1, AnotherObj = new AnotherObj {Data = "a3"}}
                },
                new TestModel
                {
                    Id = 1,
                    Name = "erman3",
                    NestedObj = new NestedObj {Id = 2, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan5",
                    NestedObj = new NestedObj {Id = 3, OrgType = 1, AnotherObj = new AnotherObj {Data = "a2"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan1",
                    NestedObj = new NestedObj {Id = 3, AnotherObj = new AnotherObj {Data = "a4"}}
                },
                new TestModel
                {
                    Id = 2,
                    Name = "afacan3",
                    NestedObj = new NestedObj {Id = 4, AnotherObj = new AnotherObj {Data = "a6"}}
                },
                new TestModel
                {
                    Id = 3,
                    Name = "bob",
                    NestedObj = new NestedObj {Id = 5, AnotherObj = new AnotherObj {Data = "a7"}}
                },
                new TestModel
                {
                    Id = 4,
                    Name = "john",
                    NestedObj = new NestedObj {Id = 6, AnotherObj = new AnotherObj {Data = "a8"}}
                },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Take = 9, Skip = 1 });
            var data = query.ToList();

            data.Count.Should().Be(7);
            data[0].Name.Should().Be("erman");
            data[6].Name.Should().Be("john");
        }

        [Fact]
        public void Should_create_greater_than_query()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 3},
                new TestModel {Id = 4}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = "Gt(\"Id\",\"1\")" });
            var data = query.ToList();

            data.Count.Should().Be(5);
            data[0].Id.Should().Be(2);
            data[4].Id.Should().Be(4);
        }

        [Fact]
        public void Should_create_greater_than_query_for_datetimeOffset()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, StartDate = DateTimeOffset.UtcNow},
                new TestModel {Id = 2, StartDate = DateTimeOffset.UtcNow.AddMonths(-2)},
                new TestModel {Id = 3, StartDate = DateTimeOffset.UtcNow.AddMonths(-3)},
                new TestModel {Id = 4, StartDate = DateTimeOffset.UtcNow.AddMonths(-4)},
                new TestModel {Id = 5, StartDate = DateTimeOffset.UtcNow.AddMonths(-5)},
                new TestModel {Id = 6, StartDate = DateTimeOffset.UtcNow.AddMonths(-6)},
                new TestModel {Id = 7, StartDate = DateTimeOffset.UtcNow.AddMonths(-7)},
                new TestModel {Id = 8, StartDate = DateTimeOffset.UtcNow.AddMonths(-8)}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = $"Gt(\"StartDate\",\"{DateTimeOffset.UtcNow.Date.ToShortDateString()}\")" });
            var data = query.ToList();

            data.Count.Should().Be(1);
        }

        [Fact]
        public void Should_create_greater_than_or_equal_query()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 3},
                new TestModel {Id = 4}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = "Ge(\"Id\",\"2\")" });
            var data = query.ToList();

            data.Count.Should().Be(5);
            data[0].Id.Should().Be(2);
            data[4].Id.Should().Be(4);
        }

        [Fact]
        public void Should_create_less_than_query()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 3},
                new TestModel {Id = 4}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = "Lt(\"Id\",\"4\")" });
            var data = query.ToList();

            data.Count.Should().Be(7);
            data.First().Id.Should().Be(1);
            data.Last().Id.Should().Be(3);
        }

        [Fact]
        public void Should_create_less_than_or_equal_query()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 1},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 2},
                new TestModel {Id = 3},
                new TestModel {Id = 4}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = "Le(\"Id\",\"2\")" });
            var data = query.ToList();

            data.Count.Should().Be(6);
            data.First().Id.Should().Be(1);
            data.Last().Id.Should().Be(2);
        }

        [Fact]
        public void Should_create_equals_query_for_boolean()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel {IsItTrue = true, Id = 1},
                new TestModel {IsItTrue = true, Id = 2},
                new TestModel {IsItTrue = false, Id = 3},
                new TestModel {IsItTrue = true, Id = 4},
                new TestModel {IsItTrue = false, Id = 5},
                new TestModel {IsItTrue = true, Id = 6},
                new TestModel {IsItTrue = false, Id = 7},
                new TestModel {IsItTrue = true, Id = 8}
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "Equals(\"IsItTrue\",\"true\")" });
            var data = query.ToList();
            data.Count.Should().Be(5);
        }

        [Fact]
        public void Should_create_equals_query_for_nestedobj_boolean()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel
                {
                    Id = 1,
                    NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {BooleanProp = true}}
                },
                new TestModel
                {
                    Id = 2,
                    NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {BooleanProp = true}}
                },
                new TestModel {Id = 3, NestedObj = new NestedObj {BooleanProp = true}},
                new TestModel {Id = 4, NestedObj = new NestedObj {BooleanProp = false}},
                new TestModel {Id = 5, NestedObj = new NestedObj {BooleanProp = false}},
                new TestModel {Id = 6, NestedObj = new NestedObj {BooleanProp = false}},
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList,
                new DataQueryModel { Filter = "Equals(\"NestedObj.AnotherObj.BooleanProp\",\"true\")" });
            var data = query.ToList();
            data.Count.Should().Be(2);
        }

        [Fact]
        public void Should_get_nasted_property_value()
        {
            var data = new TestModel
            {
                Id = 1,
                NestedObj = new NestedObj { BooleanProp = true, AnotherObj = new AnotherObj { BooleanProp = true } }
            };
            var value = QueryFilterBuilder<object>.GetPropertyValue(data, "NestedObj.AnotherObj.BooleanProp");

            value.Should().Be(true);
        }

        [Fact]
        public void Should_sort_nasted_properties()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel { Id = 1, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "a"} } },
                new TestModel { Id = 2, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "b"} } },
                new TestModel { Id = 3, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "d"} } },
                new TestModel { Id = 4, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "z"} } },
                new TestModel { Id = 5, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "y"} } },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { SortBy = "NestedObj.AnotherObj.Data" });
            var data = query.ToList();
            data.Count.Should().Be(5);
            data.First().NestedObj.AnotherObj.Data.Should().Be("a");
            data.Last().NestedObj.AnotherObj.Data.Should().Be("z");
        }

        [Fact]
        public void Should_sort_nasted_properties_desc()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel { Id = 1, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "a"} } },
                new TestModel { Id = 2, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "b"} } },
                new TestModel { Id = 3, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "d"} } },
                new TestModel { Id = 4, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "z"} } },
                new TestModel { Id = 5, NestedObj = new NestedObj {BooleanProp = true, AnotherObj = new AnotherObj {Data= "y"} } },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { SortBy = "NestedObj.AnotherObj.Data", Desc = true });

            var data = query.ToList();
            data.Count.Should().Be(5);
            data.First().NestedObj.AnotherObj.Data.Should().Be("z");
            data.Last().NestedObj.AnotherObj.Data.Should().Be("a");
        }

        [Fact]
        public void Should_filter_nullable_boolean_property()
        {
            var dummyList = new List<TestModel>
            {
                new TestModel { Id = 1, IsTrue = true },
                new TestModel { Id = 2, IsTrue = false },
                new TestModel { Id = 3, IsTrue = true },
                new TestModel { Id = 4, IsTrue = false },
                new TestModel { Id = 5, IsTrue = true },
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Filter = "Equals(\"IsTrue\",\"true\")" });

            var data = query.ToList();
            data.Count.Should().Be(3);
        }

        [Fact]
        public void Should_apply_sort_and_take()
        {
            var now = DateTime.UtcNow;
            var dummyList = new List<TestModel>
            {
                new TestModel {Id = 1, IsTrue = true, StartDate = now.AddDays(-5)},
                new TestModel {Id = 2, IsTrue = true, StartDate = now.AddDays(-2)},
                new TestModel {Id = 3, IsTrue = true, StartDate = now.AddDays(-1)},
                new TestModel {Id = 4, IsTrue = true, StartDate = now.AddDays(-4)},
                new TestModel {Id = 5, IsTrue = true, StartDate = now.AddDays(-3)},
            }.AsQueryable();

            var query = QueryFilterBuilder<TestModel>.Build(dummyList, new DataQueryModel { Take = 2, Filter = "Equals(\"IsTrue\",\"true\")", SortBy = "StartDate", Desc = true });

            var data = query.ToList();
            data.Count.Should().Be(2);

            data.First().StartDate.Should().Be(now.AddDays(-1));
            data.Last().StartDate.Should().Be(now.AddDays(-2));
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int OrganisationType { get; set; }
            public NestedObj NestedObj { get; set; }
            public bool IsItTrue { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public bool? IsTrue { get; set; }
        }

        public class NestedObj
        {
            public int Id { get; set; }
            public int? OrgType { get; set; }
            public AnotherObj AnotherObj { get; set; }
            public bool BooleanProp { get; set; }
        }

        public class AnotherObj
        {
            public string Data { get; set; }
            public bool BooleanProp { get; set; }
        }
    }


}
