using System.ComponentModel;

namespace GameMode.BotType;

public enum BotType
{
    [Description("Easy Bot - Random moves")]
    Easy,
    
    [Description("Hard Bot - Strategic moves")]
    Hard
}