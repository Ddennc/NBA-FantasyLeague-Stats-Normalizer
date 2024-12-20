using NBAFantasyLeague.Services;
using NBAFantasyLeague.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace NBAFantasyLeague.ViewModel;

public partial class PlayersViewModel : BaseViewModel
{
    public ObservableCollection<PlayerModel> Players { get; } = new();
    public ObservableCollection<PlayerModel> FilteredPlayers { get; } = new();
    public ObservableCollection<string> Positions { get; } = new()
    {
        "All", "PG", "SG", "G", "SF", "PF", "F", "C"
    };

    public ObservableCollection<string> SortOptions { get; } = new() 
    {
        "Final Score", 
        "PPG", 
        "RPG", 
        "APG", 
        "BPG",
        "SPG",
        "3PM",
        "FT%",
        "FG%",
        "TPG"
    };

    private readonly PlayerService playerService;

    public string SelectedPosition { get; set; } = "All";
    public string SelectedSortOption { get; set; } = "Final Score";
    public string PlayerNameFilter { get; set; } = ""; // Добавляем свойство для фильтрации по имени

    public PlayersViewModel(PlayerService playerService)
    {
        Title = "NBA Fantasy League";
        this.playerService = playerService;
    }

    [RelayCommand]
    public async Task GetPlayersAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var players = await playerService.GetPlayers();

            if (Players.Count != 0)
                Players.Clear();

            foreach (var player in players
                         .OrderByDescending(p => p.FinalScore)
                         .Select((value, index) => { value.Number = index + 1; return value; }))
            {
                Players.Add(player);
            }

            ApplyFilters(); // Применяем фильтры после загрузки данных
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get players: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

  
    [RelayCommand]
    public void ApplyFilters()
    {
        var filtered = Players.AsEnumerable();

        // Фильтрация по имени игрока
        if (!string.IsNullOrEmpty(PlayerNameFilter))
        {
            filtered = filtered.Where(p => p.NAME.Contains(PlayerNameFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Фильтрация по позиции
        if (SelectedPosition != "All")
        {
            filtered = SelectedPosition switch
            {
                "PG" => filtered.Where(p => p.POS == "PG"),
                "SG" => filtered.Where(p => p.POS == "SG"),
                "G" => filtered.Where(p => p.POS == "PG" || p.POS == "SG" || p.POS == "G"),
                "SF" => filtered.Where(p => p.POS == "SF"),
                "PF" => filtered.Where(p => p.POS == "PF"),
                "F" => filtered.Where(p => p.POS == "PF" || p.POS == "SF" || p.POS == "F"),
                "C" => filtered.Where(p => p.POS == "C"),
                _ => filtered
            };
        }

        // Сортировка по выбранному показателю
        filtered = SelectedSortOption switch
        {
            "Final Score" => filtered.OrderByDescending(p => p.FinalScore),
            "PPG" => filtered.OrderByDescending(p => p.NormalizedPPG),
            "RPG" => filtered.OrderByDescending(p => p.NormalizedRPG),
            "APG" => filtered.OrderByDescending(p => p.NormalizedAPG),
            "FG%" => filtered.OrderByDescending(p => p.NormalizedFG),
            "FT%" => filtered.OrderByDescending(p => p.NormalizedFT),
            "SPG" => filtered.OrderByDescending(p => p.NormalizedSPG),
            "BPG" => filtered.OrderByDescending(p => p.NormalizedBPG),
            "3PM" => filtered.OrderByDescending(p => p.Normalized3PM),
            "TPG" => filtered.OrderByDescending(p => p.NormalizedTPG),
            _ => filtered
        };

        // Обновление FilteredPlayers
        FilteredPlayers.Clear();
        foreach (var player in filtered)
        {
            FilteredPlayers.Add(player);
        }
    }

}
