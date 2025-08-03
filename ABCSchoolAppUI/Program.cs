using App.Infrastructure;
using App.Infrastructure.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ABCSchoolAppUI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

		builder.AddClientServices();
		builder.Services.AddSingleton(builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>());
		
		await builder.Build().RunAsync();
    }
}
