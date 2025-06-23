using System;
using System.Linq;
using Bridge.AssetManagerServer;
using Bridge.Models.AsseManager;
using UnityEngine;

namespace ApiTests.TemplateTests
{
    public class GetTemplatesWithFilter: EntityApiTest<Template>
    {
        protected override async void RunTestAsync()
        {
            var categoriesResponse = await Bridge.GetAsync(new Query<TemplateCategory>());
            if (categoriesResponse.IsError) throw new InvalidOperationException(categoriesResponse.ErrorMessage);
            
            var subCategoriesResponse = await Bridge.GetAsync(new Query<TemplateSubCategory>());
            if (subCategoriesResponse.IsError) throw new InvalidOperationException(subCategoriesResponse.ErrorMessage);

            var categoryId = categoriesResponse.Models.First().Id;
            var subCategoryId = subCategoriesResponse.Models.First(x => x.TemplateCategoryId == categoryId).Id;
            var templatesResp = await Bridge.GetEventTemplates(1, 0, categoryId, subCategoryId, 1);
            Debug.Log(templatesResp.Models.Length);

        }
    }
}