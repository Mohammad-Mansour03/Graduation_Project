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

            builder.UseMauiApp<App>().UseMauiCommunityToolkit();
            builder.UseMauiApp<App>().UseMauiMaps();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register ViewModels and Pages
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
