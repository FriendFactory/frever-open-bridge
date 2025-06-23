using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Template;
using Bridge.Results;

namespace Bridge.ClientServer.Template
{
    public interface ITemplateService
    {
        Task<ArrayResult<TemplateInfo>> GetDiscoveryEventTemplates(int top, int skip, int? characterCount = null, CancellationToken cancellationToken = default);
        Task<ArrayResult<TemplateInfo>> GetTrendingEventTemplates(int top, int skip, int? characterCount = null, CancellationToken cancellationToken = default);
        Task<ArrayResult<TemplateInfo>> GetPersonalEventTemplates(int top, int skip, IDictionary<string,string> headers, CancellationToken token = default);
        Task<ArrayResult<TemplateInfo>> GetEventTemplates(int top, int skip, long? categoryId = null, long? subCategoryId = null,
            int? characterCount = null, string filter = null, CancellationToken cancellationToken = default);
        Task<Result<TemplateInfo>> GetEventTemplate(long id, CancellationToken cancellationToken = default);
        Task<Result<LevelFullData>> GetEventForEventTemplate(long templateId, CancellationToken cancellationToken = default);
        Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplates(int top, int skip, CancellationToken cancellationToken = default);
        Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplatesRandomized(CancellationToken cancellationToken = default);
        Task<Result<TemplateInfo>> RenameTemplate(long templateId, string newName);
    }
}