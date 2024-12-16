using Microsoft.Extensions.Logging;
using NBAFantasyLeague.Services;
using NBAFantasyLeague.View;
using NBAFantasyLeague.ViewModel;

namespace NBAFantasyLeague;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); // Дополнительный шрифт, если потребуется
            });
       
        builder.Services.AddSingleton<PlayerService>(); // Для доступа к данным о игроках
        builder.Services.AddSingleton<PlayersViewModel>();// Для отображения списка игроков
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}