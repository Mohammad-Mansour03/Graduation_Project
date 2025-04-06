using Microsoft.Extensions.Logging;
using HojozatyCode.ViewModels;
using CommunityToolkit.Maui;

#if ANDROID
using Android.Graphics.Drawables;
#endif

namespace HojozatyCode
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Chain all UseMaui* calls together
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Add toolkit BEFORE other configurations
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register ViewModels and Pages (your existing registrations)
            builder.Services.AddTransient<AddSpaceViewModel>();
            builder.Services.AddTransient<Pages.SpaceTypeSelectionPage>();
            builder.Services.AddTransient<Pages.SpaceInformationPage>();
            builder.Services.AddTransient<Pages.CategoryVenuesPage>();
            builder.Services.AddTransient<CategoryVenuesViewModel>();
            builder.Services.AddTransient<Pages.ServicesPage>();
            builder.Services.AddTransient<Pages.SpacePicturesPage>();
            builder.Services.AddTransient<Pages.EditProfile>();
            builder.Services.AddSingleton<AddSpaceViewModel>();
            builder.Services.AddTransient<Pages.ExplorePage>();
            builder.Services.AddTransient<Pages.AdminApprovalPage>();
            builder.Services.AddTransient<AdminApprovalViewModel>();
            builder.Services.AddSingleton<AdminApprovalViewModel>();

#if ANDROID
            // Remove underline from Picker on Android
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                handler.PlatformView.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            });
#endif

            return builder.Build();
        }
    }
}