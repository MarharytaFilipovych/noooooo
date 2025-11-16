using System.ComponentModel;

namespace Layout;

public enum LayoutType
{
    [Description("classic")]
    Classic,
    
    [Description("center-block")]
    CenterBlock,

    [Description("cross")]
    Cross
}