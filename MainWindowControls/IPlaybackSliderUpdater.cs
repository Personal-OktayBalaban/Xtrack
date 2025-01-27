namespace Xtrack.MainWindowControls
{
    public interface IPlaybackSliderUpdater
    {
        void Start(TimeSpan initialTime = default);
        void Stop();
        void SetCurrentTime(TimeSpan time);
    }
}
