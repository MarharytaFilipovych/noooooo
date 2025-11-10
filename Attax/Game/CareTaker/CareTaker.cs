using Model.Game.Game;

namespace Model.Game.CareTaker;

public class CareTaker(AtaxxGame game) : ICareTaker
{
    private IMemento? _memento;
    private DateTime? _backupTime;

    public void BackUp()
    {
        _memento = game.Save();
        _backupTime = DateTime.Now;
    }

    public bool Undo(int timeWindowSeconds)
    {
        if (!CanUndo(timeWindowSeconds)) return false;

        game.Restore(_memento!);
        _memento = null;
        _backupTime = null;
        return true;
    }

    private bool CanUndo(int timeWindowSeconds)
    {
        if (_memento == null || _backupTime == null) return false;

        var elapsed = DateTime.Now - _backupTime.Value;
        return elapsed.TotalSeconds <= timeWindowSeconds;
    }
}