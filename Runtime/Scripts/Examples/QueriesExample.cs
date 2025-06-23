using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Authorization.Models;
using Bridge.Models.AsseManager;
using UnityEngine;

namespace Bridge.Examples
{
    public class QueriesExample : MonoBehaviour
    {
        private IBridge _bridge;

        private async Task Start()
        {
            _bridge = new ServerBridge();
            //deep list query
            await _bridge.LogInAsync(new EmailCredentials()
            {
                VerificationCode = "123456"//put here the code from email
            }, false);

            var query = new Query<Character>();
            //ordering
            query.SetOrderBy(nameof(Character.Id), OrderByType.Descend);
            //filtering: deprecated
            query.SetFilters(GetTestFilters());
            //setup which fields we are waiting on callback
            query.SetSelectedFieldsNames(nameof(Character.Id), nameof(Character.GroupId),
                nameof(Character.AssetStoreInfo));
            //set max return results in list
            query.SetMaxTop(3);

            //one level deep
            query.ExpandField(nameof(SetLocation.Group));
            //few levels deep
            query.ExpandField(nameof(SetLocation.SetLocationBundle))
                .ThenExpand(nameof(SetLocationBundle.CharacterSpawnPosition))
                .ThenExpand(nameof(CharacterSpawnPosition.SpawnPositionSpaceSize));

            //or the same but in another way
            var expandHierarchy = new[]
            {
                nameof(SetLocation.SetLocationBundle),
                nameof(SetLocationBundle.CharacterSpawnPosition),
                nameof(CharacterSpawnPosition.SpawnPositionSpaceSize)
            };
            var expandQuerySetup = new ExpandQuerySetup<SetLocation>(expandHierarchy);

            query.SetExpandFieldsNames(nameof(Character.AssetStoreInfo));
            await _bridge.GetAsync(query);
        }

        private FilterSetup[] GetTestFilters()
        {
            var filters = new List<FilterSetup>();

            filters.Add(new FilterSetup(nameof(Character.Id), FilterType.LessThanOrEquals, 50));

            filters.Add(new FilterSetup(nameof(Character.ModifiedTime), FilterType.LessThanOrEquals, DateTime.Now));

            return filters.ToArray();
        }

        private FilterSetup GetFilterSetupExample()
        {
            //result: Name.Contains("viktor") && GroupId>10
            
            var filterSetup = new FilterSetup()
            {
                FieldName = nameof(Character.Name),
                FilterValue = "viktor",
                FilterType = FilterType.Contains,
                CaseSensitive = true
            };

            filterSetup.AppendAnd(new FilterSetup()
            {
                FieldName = nameof(Character.GroupId),
                FilterValue = 10,
                FilterType = FilterType.GreatThan
            });
            return filterSetup;
        }
        
        private ComplexFilterSetup GetComplexFilter()
        {
            //result will be: (Id <= 100 || Name.Contains("serhii")) || (GroupId == 10 && Name.Contains("ivan"))
            
            var complexExpression1 = new FilterSetup(nameof(Character.Id), FilterType.LessThan, 100);
            complexExpression1.AppendOr(
                new FilterSetup(nameof(Character.Name), FilterType.Contains, "serhii")
            );

            var complexExpression2 = new FilterSetup(nameof(Character.GroupId), FilterType.Equals, 10);
            complexExpression2.AppendAnd(
                new FilterSetup(nameof(Character.Name), FilterType.Contains, "ivan")
            );

            var complexFilter = new ComplexFilterSetup(complexExpression1);
            complexFilter.AppendOr(complexExpression2);
           
            return complexFilter;
        }
    }
}