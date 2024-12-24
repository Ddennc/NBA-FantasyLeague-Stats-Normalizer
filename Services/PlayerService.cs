using System.Net.Http;
using System.Text.Json;
using NBAFantasyLeague.Models;

namespace NBAFantasyLeague.Services;

public class PlayerService
{
    private List<PlayerModel> playerList;
    private readonly HttpClient httpClient = new HttpClient();
    private readonly string apiUrl = "https://api.sportradar.com/nba/trial/v8/en/seasons/2024/REG/leaders.json?api_key=ztdjHZvjvAoccAiRgvcxYrIs4xT68nuzJg5NHo15"; 

    public async Task<List<PlayerModel>> GetPlayers()
    {
        if (playerList?.Count > 0)
            return playerList;

        // Запрос данных с API
        var response = await httpClient.GetStringAsync(apiUrl);
        playerList = ParsePlayers(response);

        // Нормализация данных
        NormalizePlayers(playerList);

        // Рассчитать итоговые показатели
        CalculateFinalScores(playerList);

        return playerList ?? new List<PlayerModel>();
    }

    private List<PlayerModel> ParsePlayers(string jsonResponse)
{
    var playerList = new List<PlayerModel>();
    var seenPlayers = new HashSet<string>(); // Хранит имена игроков для проверки
    var json = JsonDocument.Parse(jsonResponse);
    var categories = json.RootElement.GetProperty("categories");

    foreach (var category in categories.EnumerateArray())
    {
        var ranks = category.GetProperty("ranks");
        foreach (var rank in ranks.EnumerateArray())
        {
            var player = rank.GetProperty("player");
            var playerName = player.GetProperty("full_name").GetString();

            // Проверка на дубликаты
            if (seenPlayers.Contains(playerName))
                continue; // Игрок уже добавлен, пропускаем

            seenPlayers.Add(playerName); // Добавляем имя в HashSet

            var team = rank.GetProperty("teams")[0];
            var total = rank.GetProperty("total");
            var average = rank.GetProperty("average");

            playerList.Add(new PlayerModel
            {
                NAME = playerName,
                TEAM = team.GetProperty("name").GetString(),
                POS = player.GetProperty("primary_position").GetString(),
                GP = total.GetProperty("games_played").GetInt32(),
                MPG = total.GetProperty("minutes").GetDouble(),
                PPG = average.GetProperty("points").GetDouble(),
                RPG = average.GetProperty("rebounds").GetDouble(),
                APG = average.GetProperty("assists").GetDouble(),
                SPG = average.GetProperty("steals").GetDouble(),
                BPG = average.GetProperty("blocks").GetDouble(),
                TPG = average.GetProperty("turnovers").GetDouble(),
                _3PM = average.GetProperty("three_points_made").GetDouble(),
                FTM = average.GetProperty("free_throws_made").GetDouble(),
                FT = total.GetProperty("free_throws_pct").GetDouble(),
                FG = total.GetProperty("field_goals_pct").GetDouble(),
                FGM = average.GetProperty("field_goals_made").GetDouble(),
                _3P = total.GetProperty("three_points_pct").GetDouble(),
                ORPG = average.GetProperty("off_rebounds").GetDouble(),
                DD = total.GetProperty("double_doubles").GetDouble(),
            });
        }
    }

    return playerList;
}


    private void NormalizePlayers(List<PlayerModel> allPlayers)
    {
        double maxPPG = allPlayers.Max(p => p.PPG);
        double maxRPG = allPlayers.Max(p => p.RPG);
        double maxAPG = allPlayers.Max(p => p.APG);
        double maxBPG = allPlayers.Max(p => p.BPG);
        double maxSPG = allPlayers.Max(p => p.SPG);
        double max3PM = allPlayers.Max(p => p._3PM);
        double maxFT = 1, maxFG = 0.733, maxTPG = allPlayers.Max(p => p.TPG);
        double maxFGM = allPlayers.Max(p => p.FGM);
        double max_3P = allPlayers.Max(p => p._3PM > 1 ? p._3P : 0);
        double maxORPG = allPlayers.Max(p => p.ORPG);
        double maxDD = allPlayers.Max(p => p.DD);
        double maxFTM = allPlayers.Max(p => p.FTM);
        

        foreach (var player in allPlayers)
        {
            player.NormalizedPPG = player.PPG / maxPPG;
            player.NormalizedRPG = player.RPG / maxRPG;
            player.NormalizedAPG = player.APG / maxAPG;
            player.NormalizedBPG = player.BPG / maxBPG;
            player.NormalizedSPG = player.SPG / maxSPG;
            player.Normalized3PM = (player._3PM) / max3PM;
            player.NormalizedFT = player.FT / maxFT;
            player.NormalizedFG = player.FG / maxFG;
            player.NormalizedTPG = player.TPG / maxTPG;
            player.NormalizedFGM = player.FGM / maxFGM;
            player.Normalized_3P = player._3PM > 1 ? player._3P / max_3P : 0;
            player.NormalizedORPG = player.ORPG / maxORPG;
            player.NormalizedDD = player.DD / maxDD;
            player.NormalizedFTM = player.FTM / maxFTM;
        }
    }

    private void CalculateFinalScores(List<PlayerModel> allPlayers)
    {
        foreach (var player in allPlayers)
        {
            player.FinalScore = player.NormalizedPPG
                                + player.NormalizedRPG
                                + player.NormalizedAPG
                                + player.NormalizedBPG
                                + player.NormalizedSPG
                                + player.Normalized3PM
                                + player.NormalizedFT
                                + player.NormalizedFG
                                + player.NormalizedFGM
                                + player.Normalized_3P
                                + player.NormalizedORPG
                                + player.NormalizedDD
                                + player.NormalizedFTM;
        }
    }
}
