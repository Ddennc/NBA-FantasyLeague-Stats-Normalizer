using System.Text.Json.Serialization;

namespace NBAFantasyLeague.Models;

public class PlayerModel
{ 
    public string? NAME { get; set; }
    public string? TEAM { get; set; }
    public string? POS { get; set; }
    public int GP { get; set; }
    public double MPG { get; set; }
    public double FT { get; set; }
    public double _3PM { get; set; }
    
    public double FG { get; set; }
    public double PPG { get; set; }
    public double RPG { get; set; }
    public double APG { get; set; }
    public double SPG { get; set; }
    public double BPG { get; set; }
    public double TPG { get; set; }
    public double FTM { get; set; }
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
    public double FinalScore { get; set; }
    
    public string NormalizedStats => 
        $"PPG: {NormalizedPPG:F2}, RPG: {NormalizedRPG:F2}, APG: {NormalizedAPG:F2}, BPG: {NormalizedBPG:F2}, SPG: {NormalizedSPG:F2}, 3PM: {Normalized3PM:F2}, FT%: {NormalizedFT:F2}, FG%: {NormalizedFG:F2}, TPG: {NormalizedTPG:F2}";
    public int Number { get; set; }
}