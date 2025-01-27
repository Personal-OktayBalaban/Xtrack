namespace Xtrack.Channel
{
    public interface IChannel
    {
        string GetTrackName();

        ITrack GetTrack();

        IChannelEqualizer GetEqualizer();

        bool Play();
        bool Play(TimeSpan time);
        bool Stop();

        TimeSpan GetTrackLength();

        public void SetVolume(float volume);
        public float GetVolume();
        public void Mute();
        public void Unmute();
        public bool IsMuted();
    }
}
