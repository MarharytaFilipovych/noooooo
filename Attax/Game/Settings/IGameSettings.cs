namespace Model.Game.Settings;

public interface IGameSettings
{
    int BoardSize { get; set; }
    void MarkGameAsStarted();
    void Reset();
}