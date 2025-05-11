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

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps();

			builder.ConfigureEssentials(essentials =>
			{
				essentials.AddAppAction("reset_password", "Reset Password", icon: "icon.png");
			});
#if DEBUG
			builder.Logging.AddDebug();
#endif

            // Register ViewModels
            builder.Services.AddTransient<AddSpaceViewModel>();
            builder.Services.AddTransient<CategoryVenuesViewModel>();
            builder.Services.AddTransient<AdminApprovalViewModel>();
            builder.Services.AddTransient<EditVenueViewModel>();
            builder.Services.AddSingleton<AddSpaceViewModel>();
            builder.Services.AddSingleton<BookingViewModel>();
            builder.Services.AddSingleton<AdminApprovalViewModel>();

            // Register Pages
            builder.Services.AddTransient<Pages.SpaceTypeSelectionPage>();
            builder.Services.AddTransient<Pages.SpaceInformationPage>();
            builder.Services.AddTransient<Pages.CategoryVenuesPage>();
            builder.Services.AddTransient<Pages.ServicesPage>();
            builder.Services.AddTransient<Pages.SpacePicturesPage>();
            builder.Services.AddTransient<Pages.EditProfile>();
            builder.Services.AddTransient<Pages.ExplorePage>();
            builder.Services.AddTransient<Pages.AdminApprovalPage>();
            builder.Services.AddTransient<Pages.EditPage>();
            builder.Services.AddTransient<Pages.FavouritePage>();
            builder.Services.AddTransient<FavoriteViewModel>();
            builder.Services.AddTransient<Pages.ReviewBooking>();

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
