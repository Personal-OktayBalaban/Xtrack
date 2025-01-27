using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrack.Channel
{
    public interface ITrack
    {
        string GetFilepath();
        void SetFilepath(string filePath);

        void SetChannelEqualizer(IChannelEqualizer channelEqualizer);
        void UpdateEqualizer();

        bool Play();
        bool Play(TimeSpan time);
        bool Stop();

        TimeSpan GetLength();

        public void SetVolume(float volume);
        public float GetVolume();

        ISampleProvider GetSampleProvider();

    }
}

