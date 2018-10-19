using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LBHTenancyAPI.Controllers;
using LBHTenancyAPI.Infrastructure.UseCase;

namespace LBHTenancyAPI.UseCases.Versioning
{
    public class GetVersionUseCase:IGetVersionUseCase
    {
        public async Task<GetVersionResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetAssembly(typeof(ServiceController));

            var response = new GetVersionResponse
            {
                Version = assembly.GetName().Version
            };
            return response;
        }
    }

    public interface IGetVersionUseCase: IResponseUseCase<GetVersionResponse>
    {

    }

    public class GetVersionResponse
    {
        public Version Version { get; set; }
    }
}
