using System.Text.Json.Serialization;

namespace NBAFantasyLeague.Models;

public class PlayerModel
{
    public string? RANK { get; set; }
    public string? NAME { get; set; }
    public string? TEAM { get; set; }
    public string? POS { get; set; }
    public double AGE { get; set; }
    public int GP { get; set; }
    public double MPG { get; set; }

    [JsonPropertyName("USG%")]
    public double USG { get; set; }

    [JsonPropertyName("TO%")]
    public double TO { get; set; }
    public int FTA { get; set; }

    [JsonPropertyName("FT%")]
    public double FT { get; set; }

    [JsonPropertyName("2PA")]
    public int _2PA { get; set; }

    [JsonPropertyName("2P%")]
    public double _2P { get; set; }

    [JsonPropertyName("3PA")]
    public int _3PA { get; set; }

    [JsonPropertyName("3P%")]
    public double _3P { get; set; }

    [JsonPropertyName("eFG%")]
    public double eFG { get; set; }

    [JsonPropertyName("TS%")]
    public double TS { get; set; }
    public double PPG { get; set; }
    public double RPG { get; set; }
    public double APG { get; set; }
    public double SPG { get; set; }
    public double BPG { get; set; }
    public double TPG { get; set; }

    [JsonPropertyName("P+R")]
    public double PR { get; set; }

    [JsonPropertyName("P+A")]
    public double PA { get; set; }

    [JsonPropertyName("P+R+A")]
    public double PRA { get; set; }
    public double VI { get; set; }
    public double ORtg { get; set; }
    public double DRtg { get; set; }

    // Нормализованные показатели
    public double NormalizedPPG { get; set; }
    public double NormalizedRPG { get; set; }
    public double NormalizedAPG { get; set; }
    public double NormalizedBPG { get; set; }
    public double NormalizedSPG { get; set; }
    public double Normalized3PM { get; set; }
    public double NormalizedFT { get; set; }
    public double NormalizedFG { get; set; }
    public double NormalizedTPG { get; set; }
    public double FTM { get; set; }
    public double FinalScore { get; set; }
    
    public string NormalizedStats => 
        $"PPG: {NormalizedPPG:F2}, RPG: {NormalizedRPG:F2}, APG: {NormalizedAPG:F2}, BPG: {NormalizedBPG:F2}, SPG: {NormalizedSPG:F2}, 3PM: {Normalized3PM:F2}, FT%: {NormalizedFT:F2}, FG%: {NormalizedFG:F2}, TPG: {NormalizedTPG:F2}";
    public int Number { get; set; }
}