using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PetsApp.Services;
using PetsApp.ViewModel;
using PetsApp.Pages;
using Syncfusion.Maui.Core.Hosting;
using PetsApp.Repository;

namespace PetsApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Roboto-Light.ttf", "RobotoL");
                fonts.AddFont("Roboto-Thin.ttf", "RobotoT");
                fonts.AddFont("aAbstractGroovy.ttf", "aAbstractGroovy");
                fonts.AddFont("Peanut Butter.ttf", "Butter");
                fonts.AddFont("Hello Whistle.ttf", "Hello");
                fonts.AddFont("Mecha Baby.ttf", "baby");
                fonts.AddFont("Mandala Handmade.otf", "man");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
            AddTypePetsServices(builder.Services);
#endif
            return builder.Build();
        }

        private static IServiceCollection AddTypePetsServices(IServiceCollection services)
        {
            services.AddSingleton<TypePetsServices>();
            services.AddSingleton<BannerServices>();
            services.AddSingleton<PetServices>();
            services.AddSingletonWithShellRoute<Home,HomeVM>(nameof(Home));
            services.AddSingleton<PetsVM>().AddSingleton<PetDetails>();



            return services;
        }
    }
}