using Microsoft.Extensions.Logging;
using HojozatyCode.ViewModels; // Add this line if CategoryVenuesViewModel is in the ViewModels namespace

namespace HojozatyCode
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>();
            

#if DEBUG
    		builder.Logging.AddDebug();
#endif

        
        builder.Services.AddTransient<AddSpaceViewModel>();
        builder.Services.AddTransient<Pages.SpaceTypeSelectionPage>();
        builder.Services.AddTransient<Pages.SpaceInformationPage>();
        builder.Services.AddTransient<Pages.CategoryVenuesPage>();
        builder.Services.AddTransient<CategoryVenuesViewModel>();
            return builder.Build();
        }
    }
}
