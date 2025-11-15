using Layout.LayoutType;

namespace Command;

using Layout;


public class ChooseLayoutCommand(LayoutType layoutType) : ICommand
{
    public LayoutType LayoutType { get; } = layoutType;
}