using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Interfejsy.Weryfikatory;
using UkrywanieWiadomosci.Aplikacja.Serwisy.Kontrola;
using UkrywanieWiadomosci.Aplikacja.Serwisy.ObslugaPlikow;
using UkrywanieWiadomosci.Aplikacja.Serwisy.Weryfikatory;
using UkrywanieWiadomosci.Prezentacja.Interfejsy;
using UkrywanieWiadomosci.Prezentacja.KlasyPomocnicze;
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
            //Aplikacja
            services.AddTransient<IKontrolaProcesuUkrywania, KontrolaProcesuUkrywaniaSerwis>();
            services.AddTransient<IObslugaPlikow, ObslugaPlikowSerwisy>();
            services.AddTransient<IWeryfikacjaDanych, WeryfikacjaDanychSerwisy>();

            //Prezentacja
            services.AddTransient<MainWindow>();
            services.AddTransient<DialogiPlikow>();
            services.AddSingleton<WskaznikiDoGUISerwis>();
            services.AddTransient<IZarzadzanieGui, ZarzadzanieGuiSerwis>();
            services.AddTransient<PrzygotowanieDanychPomocniczych>();
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
