using Microsoft.Extensions.Logging;

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

            return builder.Build();
        }
    }
}
