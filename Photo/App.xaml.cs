using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Photo.HostBuilders;

namespace Photo
{
    public partial class App : Application
    {
        private readonly IHost _host;
        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddViewModels()
                .AddViews();
        }
        public App()
        {
            this.InitializeComponent();
            _host = CreateHostBuilder().Build();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Activate();
        }

        public static MainWindow MainWindow { get; private set; }
    }
}
