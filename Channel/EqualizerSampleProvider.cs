using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Dsp;
using NAudio.Wave;

namespace Xtrack.Channel
{
    public class EqualizerSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider _sourceProvider;
        private BiQuadFilter[] _filters;

        public EqualizerSampleProvider(ISampleProvider sourceProvider, EqualizerBand[] bands)
        {
            if (sourceProvider == null)
            {
                throw new ArgumentNullException(nameof(sourceProvider));
            }

            if (bands == null)
            {
                throw new ArgumentNullException(nameof(bands));
            }

            _sourceProvider = sourceProvider;
            _filters = CreateFilters(bands);
        }

        public WaveFormat WaveFormat => _sourceProvider.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = _sourceProvider.Read(buffer, offset, count);

            for (int i = 0; i < samplesRead; i++)
            {
                buffer[offset + i] = ApplyFilters(buffer[offset + i]);
            }

            return samplesRead;
        }

        private float ApplyFilters(float sample)
        {
            for (int i = 0; i < _filters.Length; i++)
            {
                sample = _filters[i].Transform(sample);
            }
            return sample;
        }

        private BiQuadFilter[] CreateFilters(EqualizerBand[] bands)
        {
            var filters = new BiQuadFilter[bands.Length];
            for (int i = 0; i < bands.Length; i++)
            {
                filters[i] = BiQuadFilter.PeakingEQ(44100, bands[i].Frequency, bands[i].Bandwidth, bands[i].Gain);
            }
            return filters;
        }

        public void UpdateBands(EqualizerBand[] bands)
        {
            if (bands == null)
            {
                throw new ArgumentNullException(nameof(bands));
            }

            _filters = CreateFilters(bands);
        }
    }
}


