namespace FishingMiniGame.Observer
{
    public interface IObserver
    {
        void OnNotify(GameEvent gameEvent, object data = null);
    }
}

namespace FishingMiniGame.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(GameEvent gameEvent, object data = null);
    }
}

namespace FishingMiniGame.Observer
{
    public enum GameEvent
    {
        BarFishContact,       
        BarFishContactUpdate,
        BarFishLost, 
        GameWin,
        GameLose,
        BarPositionChanged,
        FishPositionChanged
    }
}