namespace Xtrack.MainWindowControls
{
    public interface IPlaybackSliderUpdater
    {
        void Start();
        void Start(TimeSpan initialTime);
        void Stop();
        void SetCurrentTime(TimeSpan time);
    }
}
