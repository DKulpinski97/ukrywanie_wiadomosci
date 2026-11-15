using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;
using UkrywanieWiadomosci.Prezentacja.Serwisy;
using UkrywanieWiadomosci.Prezentacja.Wskazniki;

namespace UkrywanieWiadomosci
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Presentation
            services.AddTransient<MainWindow>();

            // Application

            //Domain
            services.AddSingleton<WskaznikiDoGUISerwis>();
            services.AddTransient<IZarzadzanieGui, ZarzadzanieGuiSerwis>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider.Dispose();
            base.OnExit(e);
        }
    }

}
