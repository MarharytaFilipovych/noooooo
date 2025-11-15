using System.ComponentModel;

namespace Layout.LayoutType;

public enum LayoutType
{
    [Description("classic")]
    Classic,
    
    [Description("center-block")]
    CenterBlock,

    [Description("cross")]
    Cross
}