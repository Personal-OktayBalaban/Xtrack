using System;
using NAudio.Wave;

namespace Xtrack.Channel
{
    public class ChannelEqualizer : IChannelEqualizer
    {
        private EqualizerBand[] _bands;
        private ITrack? _track; // Çalan şarkıya eşzamanlı güncelleme için Track referansı

        public ChannelEqualizer(ITrack track)
        {
            _track = track ?? throw new ArgumentNullException(nameof(track));

            _bands = new[]
            {
                new EqualizerBand { Frequency = 100, Gain = 0, Bandwidth = 0.8f },   // Bass
                new EqualizerBand { Frequency = 1000, Gain = 0, Bandwidth = 0.8f },  // Mid
                new EqualizerBand { Frequency = 5000, Gain = 0, Bandwidth = 0.8f }   // Treble
            };
        }

        public EqualizerBand[] GetBands()
        {
            return _bands;
        }

        public bool UpdateBand(float frequency, float gain)
        {
            var band = Array.Find(_bands, b => b.Frequency == frequency);

            if (band != null)
            {
                band.Gain = gain;
                Console.WriteLine($"Updated Equalizer: {frequency}Hz -> {gain}dB");

                _track?.UpdateEqualizer();

                return true;
            }
            else
            {
                Console.WriteLine($"Frequency could not be found: ({frequency}Hz)");
                return false;
            }
        }

        public void SetTrack(ITrack track)
        {
            _track = track ?? throw new ArgumentNullException(nameof(track));
        }
    }
}
