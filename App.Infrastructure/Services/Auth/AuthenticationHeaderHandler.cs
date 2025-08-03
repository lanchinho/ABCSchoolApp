using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace App.Infrastructure.Services.Auth;
public class AuthenticationHeaderHandler(ILocalStorageService localStorageService) : DelegatingHandler
{
	private readonly ILocalStorageService _localStorageService = localStorageService;
	private const string AuthScheme = "Bearer";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		try
		{
			if (!AuthScheme.Equals(request.Headers.Authorization?.Scheme))
			{
				var savedToken = await _localStorageService.GetItemAsync<string>(Constants.StorageConstants.AuthToken, cancellationToken);
				if (!string.IsNullOrWhiteSpace(savedToken))
					request.Headers.Authorization = new AuthenticationHeaderValue(AuthScheme, savedToken);
			}

			return await base.SendAsync(request, cancellationToken);
		}
		catch (Exception)
		{
			throw;
		}

	}
}
