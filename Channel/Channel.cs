using System;
using System.IO;
using Xtrack.Common;

namespace Xtrack.Channel
{
    internal class Channel : IChannel
    {
        private readonly ITrack _track;
        private readonly string _trackName;
        private readonly IChannelEqualizer _equalizer;

        private bool _isMuted;

        public Channel(string trackPath)
        {
            if (string.IsNullOrWhiteSpace(trackPath))
            {
                throw new ArgumentException("Track path cannot be null or empty.", nameof(trackPath));
            }

            _track = LoadTrack(trackPath);
            _trackName = ExtractTrackName(trackPath);
            _equalizer = new ChannelEqualizer(_track);

            _track.SetChannelEqualizer(_equalizer);
        }

        public string GetTrackName()
        {
            return _trackName;
        }

        public ITrack GetTrack()
        {
            return _track;
        }

        public IChannelEqualizer GetEqualizer()
        {
            return _equalizer;
        }

        public bool Play()
        {
            return _track.Play();
        }

        public bool Play(TimeSpan time)
        {
            return _track.Play(time);
        }

        public bool Stop()
        {
            return _track.Stop();
        }

        private string ExtractTrackName(string trackPath)
        {
            return Path.GetFileNameWithoutExtension(trackPath);
        }

        private ITrack LoadTrack(string trackPath)
        {
            var track = TrackLoader.LoadTrack(trackPath);
            if (track == null)
            {
                throw new InvalidOperationException("Failed to load track: " + trackPath);
            }
            return track;
        }

        public TimeSpan GetTrackLength()
        {
            return _track.GetLength();
        }

        public void SetVolume(float volume)
        {
            _track.SetVolume(volume);
        }

        public void Mute()
        {
            _isMuted = true;
            _track.SetVolume(0.0f);
        }

        public void Unmute()
        {
            _isMuted = false;
        }

        public bool IsMuted()
        {
            return _isMuted;
        }

        public float GetVolume()
        {
            return _track.GetVolume();
        }
    }
}
