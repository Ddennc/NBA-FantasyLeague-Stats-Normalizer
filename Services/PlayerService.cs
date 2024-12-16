using System.Text.Json;
using NBAFantasyLeague.Models;

namespace NBAFantasyLeague.Services;

public class PlayerService
{
    private List<PlayerModel> playerList;

    public async Task<List<PlayerModel>> GetPlayers()
    {
        if (playerList?.Count > 0)
            return playerList;

        // Загрузка данных из локального файла players.json
        using var stream = await FileSystem.OpenAppPackageFileAsync("players.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        playerList = JsonSerializer.Deserialize<List<PlayerModel>>(contents);

        // Нормализация данных
        NormalizePlayers(playerList);

        // Рассчитать итоговые показатели
        CalculateFinalScores(playerList);

        return playerList ?? new List<PlayerModel>();
    }

    private void NormalizePlayers(List<PlayerModel> allPlayers)
    {
        // Находим максимальные значения для каждого показателя
        double maxPPG = allPlayers.Max(p => p.PPG);
        double maxRPG = allPlayers.Max(p => p.RPG);
        double maxAPG = allPlayers.Max(p => p.APG);
        double maxBPG = allPlayers.Max(p => p.BPG);
        double maxSPG = allPlayers.Max(p => p.SPG);
        double max3PM = allPlayers.Max(p => p._3PA * p._3P / 100); // 3PM = 3PA * 3P%
        double maxFT =  1; // Максимальный FT% для деления
        double maxFG = 0.86; // Максимальный eFG% для деления
        double maxTPG = allPlayers.Max(p => p.TPG);

        foreach (var player in allPlayers)
        {
            // Рассчитываем FTM
            player.FTM = player.FT * player.FTA / 100;
            // Нормализация показателей
            player.NormalizedPPG = player.PPG / maxPPG;
            player.NormalizedRPG = player.RPG / maxRPG;
            player.NormalizedAPG = player.APG / maxAPG;
            player.NormalizedBPG = player.BPG / maxBPG;
            player.NormalizedSPG = player.SPG / maxSPG;
            player.Normalized3PM = (player._3PA * player._3P / 100) / max3PM;
            player.NormalizedFT = player.FT / maxFT;
            player.NormalizedFG = player.eFG / maxFG;
            player.NormalizedTPG = player.TPG / maxTPG;
        }
    }

    private void CalculateFinalScores(List<PlayerModel> allPlayers)
    {
        foreach (var player in allPlayers)
        {
            // Суммируем все нормализованные показатели
            player.FinalScore = player.NormalizedPPG
                                + player.NormalizedRPG
                                + player.NormalizedAPG
                                + player.NormalizedBPG
                                + player.NormalizedSPG
                                + player.Normalized3PM
                                + player.NormalizedFT
                                + player.NormalizedFG;

            // Вычитаем NormalizedTPG
            player.FinalScore -= player.NormalizedTPG;
        }
    }
} 