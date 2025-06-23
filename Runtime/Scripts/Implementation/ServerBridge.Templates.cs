using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bridge.ClientServer.Template;
using Bridge.Models.ClientServer.Level.Full;
using Bridge.Models.ClientServer.Template;
using Bridge.Results;

namespace Bridge
{
    public sealed partial class ServerBridge
    {
        private ITemplateService _templateService;
        
        public Task<ArrayResult<TemplateInfo>> GetDiscoveryEventTemplates(int top, int skip, int? characterCount = null, CancellationToken cancellationToken = default)
        {
            return _templateService.GetDiscoveryEventTemplates(top, skip, characterCount, cancellationToken);
        }

        public Task<ArrayResult<TemplateInfo>> GetTrendingEventTemplates(int top, int skip, int? characterCount = null, CancellationToken cancellationToken = default)
        {
            return _templateService.GetTrendingEventTemplates(top, skip, characterCount, cancellationToken);
        }

        public Task<ArrayResult<TemplateInfo>> GetPersonalEventTemplates(int top, int skip, IDictionary<string,string> headers, CancellationToken token = default)
        {
            return _templateService.GetPersonalEventTemplates(top, skip, headers, token);
        }

        public Task<ArrayResult<TemplateInfo>> GetEventTemplates(int top, int skip, long? categoryId, long? subCategoryId, int? characterCount,
            string filter, CancellationToken cancellationToken)
        {
            return _templateService.GetEventTemplates(top, skip, categoryId, subCategoryId, characterCount, filter,
                cancellationToken);
        }

        public Task<Result<TemplateInfo>> GetEventTemplate(long id, CancellationToken cancellationToken = default)
        {
            return _templateService.GetEventTemplate(id, cancellationToken);
        }

        public Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplates(int top, int skip, CancellationToken cancellationToken = default)
        {
            return _templateService.GetOnBoardingEventTemplates(top, skip, cancellationToken);
        }

        public Task<ArrayResult<TemplateInfo>> GetOnBoardingEventTemplatesRandomized(CancellationToken cancellationToken = default)
        {
            return _templateService.GetOnBoardingEventTemplatesRandomized(cancellationToken);
        }
        
        public Task<Result<LevelFullData>> GetEventForEventTemplate(long templateId, CancellationToken cancellationToken = default)
        {
            return _templateService.GetEventForEventTemplate(templateId, cancellationToken);
        }
        
        public Task<Result<TemplateInfo>> RenameTemplate(long templateId, string newName)
        {
            return _templateService.RenameTemplate(templateId, newName);
        }
    }
}