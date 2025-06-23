using System;
using System.Linq;
using System.Threading.Tasks;
using Bridge.AssetManagerServer;
using Bridge.Models.Common;
using Bridge.Results;
using Newtonsoft.Json;
using UnityEngine;

namespace ApiTests
{
    public abstract class EntityApiTest<T>: AuthorizedUserApiTestBase where T:  IEntity
    {
        protected async Task<long> GetAnyAvailableEntityId<TK>() where TK : class, IEntity
        {
            var q = new Query<TK>();
            q.SetSelectedFieldsNames(nameof(IEntity.Id));
            q.SetOrderBy(nameof(IEntity.Id), OrderByType.Descend);
            q.SetMaxTop(1);
            var resp = await Bridge.GetAsync(q);
            if(resp.IsError)
                throw new Exception(resp.ErrorMessage);
            if(resp.Models==null || resp.Models.Length==0)
                throw new Exception($"Not found any {typeof(TK).Name}");

            return resp.Models.Last().Id;
        }

        protected void LogResult(SingleEntityResult<T> res)
        {
            if (res.IsRequestCanceled)
            {
                Debug.Log("Canceled");
            }else
            if (res.IsSuccess)
            {
                Debug.Log("Success: " + JsonConvert.SerializeObject(res.ResultObject));
            }
            else
            {
                Debug.LogError("Error: " + res.ErrorMessage);
            }
        }

        protected void LogResult(SingleObjectResult<T> res)
        {
            if (res.IsSuccess)
            {
                Debug.Log("Success: " + JsonConvert.SerializeObject(res.ResultObject));
            }
            else
            {
                Debug.LogError("Error: " + res.ErrorMessage);
            }
        }

        protected void LogResult(DeleteResult<T> res)
        {
            if (res.IsSuccess)
            {
                Debug.Log("Succeed");
            }
            else
            {
                Debug.LogError("Error: " + res.ErrorMessage);
            }
        }

        protected void LogResult(FileUpdateResult res)
        {
            if (res.IsSuccess)
            {
                Debug.Log("Succeed");
            }
            else
            {
                Debug.LogError("Error: " + res.ErrorMessage);
            }
        }
    }
}