using Toolbelt.Blazor;

namespace App.Infrastructure.Services.Interceptors;
public interface IHttpRefreshTokenInterceptor
{
    Task InterceptBeforeHttpRequestAsync(object sender, HttpClientInterceptorEventArgs eventArgs);
    void RegisterEvent();
}
