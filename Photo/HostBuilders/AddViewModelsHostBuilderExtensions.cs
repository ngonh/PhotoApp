using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Photo.ViewModels;
using System;

namespace Photo.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddTransient<MainViewModel>();

                services.AddSingleton<CreateViewModel<HomeViewModel>>(services => () => services.GetRequiredService<HomeViewModel>());
                
            });

            return host;
        }
    }
}
