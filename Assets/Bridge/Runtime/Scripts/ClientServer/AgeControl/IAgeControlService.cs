using System;
using System.Threading;
using System.Threading.Tasks;
using Bridge.Authorization;
using Bridge.Models.ClientServer;
using Bridge.Modules.Serialization;
using Bridge.Results;

namespace Bridge.ClientServer.AgeControl
{
    public interface IAgeControlService
    {
        Task<Result<AgeConfirmationQuizResponse>> GetQuizStatus(CancellationToken token);
        Task<Result<AgeConfirmationQuizResponse>> GetAgeConfirmationQuestions(CancellationToken token = default);
        Task<Result<AgeConfirmationQuizResponse>> ConfirmAgeWithAnswers(AgeConfirmationAnswer[] answers);
    }

    internal sealed class AgeControlService : ServiceBase, IAgeControlService
    {
    
        public AgeControlService(string host, IRequestHelper requestHelper, ISerializer serializer) : base(host, requestHelper, serializer)
        {
        }

        public Task<Result<AgeConfirmationQuizResponse>> GetQuizStatus(CancellationToken token)
        {
            return SendGetAgeConfirmationRequest($"/auth/age-confirmation/quiz/status", token);
        }

        public Task<Result<AgeConfirmationQuizResponse>> GetAgeConfirmationQuestions(CancellationToken token = default)
        {
            return SendGetAgeConfirmationRequest($"/auth/age-confirmation/quiz", token);
        }

        public async Task<Result<AgeConfirmationQuizResponse>> ConfirmAgeWithAnswers(AgeConfirmationAnswer[] answers)
        {
            try
            {
                var url = ConcatUrl(Host, $"/auth/age-confirmation/quiz");
                return await SendPostRequest<AgeConfirmationQuizResponse>(url, answers);

            }
            catch (Exception e)
            {
                return Result<AgeConfirmationQuizResponse>.Error(e.Message);
            }
        }

        private async Task<Result<AgeConfirmationQuizResponse>> SendGetAgeConfirmationRequest(string endPoint,
            CancellationToken token = default)
        {
            try
            {
                var url = ConcatUrl(Host, endPoint);
                return await SendRequestForSingleModel<AgeConfirmationQuizResponse>(url, token);
            }
            catch (Exception e)
            {
                return e is OperationCanceledException
                    ? Result<AgeConfirmationQuizResponse>.Cancelled()
                    : Result<AgeConfirmationQuizResponse>.Error(e.Message);
            }
        }
    }
}