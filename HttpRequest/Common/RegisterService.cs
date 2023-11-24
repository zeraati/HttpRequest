using Common.Extension;
using Common.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace HttpRequest.Common;
public static class RegisterService
{
    public static void AddHttpRequest(this IServiceCollection services)
    {
        var assemblyName = typeof(RegisterService).Assembly.GetName();
        services.AddScopedClass<IAddScoped>(assemblyName);
    }
}
