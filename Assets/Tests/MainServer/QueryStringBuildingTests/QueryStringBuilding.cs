using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using Bridge.Models.VideoServer;
using Bridge.VideoServer.Models;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class QueryStringBuilding
    {
        [Test]
        public void BuildDeepQueryString()
        {
            var expectedResult = $"?$Expand={nameof(SetLocation.SetLocationBundle)}($Expand={nameof(SetLocationBundle.CharacterSpawnPosition)}($Expand={nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)}))";
            var query = new Query<SetLocation>();
            query.ExpandField(nameof(SetLocation.SetLocationBundle))
                .ThenExpand(nameof(SetLocationBundle.CharacterSpawnPosition))
                .ThenExpand(nameof(CharacterSpawnPosition.SpawnPositionSpaceSize));
            
            var queryString = query.BuildQuery();

            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildDeepQueryWithNotDeepQueriesString()
        {
            var expectedResult = $"?$Expand={nameof(SetLocation.Group)},{nameof(SetLocation.SetLocationBundle)}($Expand={nameof(SetLocationBundle.CharacterSpawnPosition)}" +
                                 $"($Expand={nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)})),{nameof(SetLocation.Category)}";
            
            var query = new Query<SetLocation>();
            query.ExpandField(nameof(SetLocation.Group));

            query.ExpandField(nameof(SetLocation.SetLocationBundle))
                .ThenExpand(nameof(SetLocationBundle.CharacterSpawnPosition))
                .ThenExpand(nameof(CharacterSpawnPosition.SpawnPositionSpaceSize));

            query.ExpandField(nameof(SetLocation.Category));

            var queryString = query.BuildQuery();

            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildDeepQueryWithNotDeepQueriesStringWithInjectionSetupApi()
        {
            var expectedResult = $"?$Expand={nameof(SetLocation.Group)},{nameof(SetLocation.SetLocationBundle)}($Expand={nameof(SetLocationBundle.CharacterSpawnPosition)}" +
                                 $"($Expand={nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)})),{nameof(SetLocation.Category)}";

            var query = new Query<SetLocation>();
            query.ExpandField(new ExpandQuerySetup<SetLocation>(nameof(SetLocation.Group)));
            query.ExpandField(new ExpandQuerySetup<SetLocation>(nameof(SetLocation.SetLocationBundle))
                .ThenExpand(nameof(SetLocationBundle.CharacterSpawnPosition))
                .ThenExpand(nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)));

            query.ExpandField(nameof(SetLocation.Category));

            var queryString = query.BuildQuery();

            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildDeepQueryWithNotDeepQueriesStringWithInjectionStringArray()
        {
            var expectedResult = $"?$Expand={nameof(SetLocation.Group)},{nameof(SetLocation.SetLocationBundle)}($Expand={nameof(SetLocationBundle.CharacterSpawnPosition)}" +
                                 $"($Expand={nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)})),{nameof(SetLocation.Category)}";

            var query = new Query<SetLocation>();
            query.ExpandField(new ExpandQuerySetup<SetLocation>(nameof(SetLocation.Group)));
            query.ExpandField(new ExpandQuerySetup<SetLocation>(new[]
            {
                nameof(SetLocation.SetLocationBundle), nameof(SetLocationBundle.CharacterSpawnPosition),
                nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)
            }));

            query.ExpandField(nameof(SetLocation.Category));

            var queryString = query.BuildQuery();
       
            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildQueryWithCamelCaseFilter_ShouldReturnWithoutToLowerWords()
        {
            var expectedResult = $"?$Filter=startswith(Name,'SetName')";

            var query = new Query<SetLocation>();
            var filter = new FilterSetup()
            {
                FieldName = nameof(SetLocation.Name),
                FilterType = FilterType.StartWith,
                FilterValue = "SetName",
                CaseSensitive = true
            };
            query.SetFilters(filter);

            var queryString = query.BuildQuery();
            Debug.Log(queryString); 
            Assert.AreEqual(queryString,expectedResult );
        }

        [Test]
        public void BuildQueryWithCamelCaseFilter_ShouldReturnWithoutWithToLowerWords()
        {
            var expectedResult = $"?$Filter=startswith(tolower(Name),'setname')";

            var query = new Query<SetLocation>();
            var filter = new FilterSetup()
            {
                FieldName = nameof(SetLocation.Name),
                FilterType = FilterType.StartWith,
                FilterValue = "SetName",
                CaseSensitive = false
            };
            query.SetFilters(filter);

            var queryString = query.BuildQuery();
            Debug.Log(queryString);
            Assert.AreEqual(queryString, expectedResult);
        }

        [Test]
        public void BuildQueryWithOrderBy()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$OrderBy=Id desc");
            
            var query = new Query<SetLocation>();
            query.SetOrderBy(nameof(SetLocation.Id), OrderByType.Descend);

            var queryString = query.BuildQuery();
            
            Assert.AreEqual(expectedResult, queryString);
        }
        
        [Test]
        public void BuildQueryWithDeepOrderBy()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$OrderBy={nameof(Video.KPI)}/{nameof(VideoKPI.Likes)} desc");
            
            var query = new Query<Video>();
            var fieldPath = new[] {nameof(Video.KPI), nameof(VideoKPI.Likes)};
            query.SetDeepOrderBy(fieldPath, OrderByType.Descend);

            var queryString = query.BuildQuery();
            
            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildQueryWithFilterSetupObsoleteMethod()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$Filter={nameof(Character.GroupId)} gt 1 and {nameof(Character.GroupId)} lt 100");
            var query = new Query<Character>();

            var filterGreaterThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 1,
                FilterType = FilterType.GreatThan
            };

            var filterLessThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 100,
                FilterType = FilterType.LessThan
            };
            
            query.SetFilters(filterGreaterThan, filterLessThan);

            var queryString = query.BuildQuery();
            Assert.AreEqual(expectedResult, queryString);
        }

        [Test]
        public void BuildQueryWithComplexFilterSetup_And()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$Filter={nameof(Character.GroupId)} gt 1 and {nameof(Character.GroupId)} lt 100");
            var query = new Query<Character>();

            var filterGreaterThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 1,
                FilterType = FilterType.GreatThan
            };

            var filterLessThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 100,
                FilterType = FilterType.LessThan
            };
            
            var complexFilter = new ComplexFilterSetup(filterGreaterThan);
            complexFilter.AppendAnd(filterLessThan);
            query.SetFilters(complexFilter);

            var queryString = query.BuildQuery();
            Assert.AreEqual(expectedResult, queryString);
        }
        
        [Test]
        public void BuildQueryWithComplexFilterSetup_Or()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$Filter={nameof(Character.GroupId)} gt 1 or {nameof(Character.GroupId)} lt 100");
            var query = new Query<Character>();

            var filterGreaterThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 1,
                FilterType = FilterType.GreatThan
            };

            var filterLessThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 100,
                FilterType = FilterType.LessThan
            };
            
            var filterBuilder = new ComplexFilterSetup(filterGreaterThan);
            filterBuilder.AppendOr(filterLessThan);
            query.SetFilters(filterBuilder);

            var queryString = query.BuildQuery();
            Assert.AreEqual(expectedResult, queryString);
        }
        
        [Test]
        public void BuildQueryWithComplexFilterSetup_ComplexOr()
        {
            var expectedResult = ReplaceSpaceByEncodingCharacter($"?$Filter=({nameof(Character.GroupId)} gt 1 and {nameof(Character.GroupId)} lt 100) or ({nameof(Character.GroupId)} gt 1 and {nameof(Character.GroupId)} lt 100)");
            var query = new Query<Character>();

            var filterGreaterThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 1,
                FilterType = FilterType.GreatThan
            };

            var filterLessThan = new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 100,
                FilterType = FilterType.LessThan
            };

            filterGreaterThan.AppendAnd(filterLessThan);
            
            var filterBuilder = new ComplexFilterSetup(filterGreaterThan);
            filterBuilder.AppendOr(filterGreaterThan);
            query.SetFilters(filterBuilder);

            var queryString = query.BuildQuery();
            Assert.AreEqual(expectedResult, queryString);
        }

        private static string ReplaceSpaceByEncodingCharacter(string target)
        {
            return target.Replace(" ", "%20");
        }
    }
}
