﻿using Microsoft.Extensions.Logging;
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
        builder.Services.AddTransient<Pages.ServicesPage>();
        builder.Services.AddTransient<Pages.SpacePicturesPage>();
        builder.Services.AddTransient<Pages.EditProfile>();
        builder.Services.AddSingleton<AddSpaceViewModel>(); // for managing the state of the AddSpaceViewModel across the app lifecycle 
        //builder.Services.AddTransient<Pages.ExplorePage>();
        
        
            return builder.Build();
        }
    }
}
