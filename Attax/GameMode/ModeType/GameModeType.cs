using System.ComponentModel;

namespace GameMode.ModeType;

public enum GameModeType
{
    [Description("Player vs Player")] 
    PvP, 
    
    [Description("Player vs Bot")] 
    PvE
}