using System.ComponentModel;

namespace GameMode;

public enum ModeType
{
    [Description("Player vs Player")] 
    PvP, 
    
    [Description("Player vs Bot")] 
    PvE
}