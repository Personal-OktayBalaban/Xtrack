using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using Xtrack.Channel;
using Xtrack.Saving;

namespace Xtrack.TrackSaving
{
    public class TrackMerger : ITrackMerger
    {
        public bool MergeAndSave(IEnumerable<IChannel> channels, string outputPath)
        {
            try
            {
                var waveMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

                foreach (var channel in channels)
                {
                    if (channel.GetVolume() == 0)
                    {
                        continue;
                    }

                    var equalizedSampleProvider = ApplyEqualizer(channel);

                    if (equalizedSampleProvider != null)
                    {
                        waveMixer.AddMixerInput(equalizedSampleProvider);
                    }
                }

                SaveToFile(waveMixer, outputPath);

                Console.WriteLine($"Tracks merged and saved to {outputPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while merging tracks: {ex.Message}");
                return false;
            }
        }

        private ISampleProvider? ApplyEqualizer(IChannel channel)
        {
            var track = channel.GetTrack();

            var sampleProvider = track.GetSampleProvider();
            if (sampleProvider != null)
            {
                Console.WriteLine($"Sample provider successfully retrieved for channel {channel.GetTrackName()}.");
                return sampleProvider;
            }

            Console.WriteLine($"Sample provider is null for channel {channel.GetTrackName()}.");
            return null;
        }

        private void SaveToFile(ISampleProvider sampleProvider, string outputPath)
        {
            using (var waveFileWriter = new WaveFileWriter(outputPath, sampleProvider.WaveFormat))
            {
                float[] buffer = new float[4096];
                int read;

                while ((read = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                {
                    waveFileWriter.WriteSamples(buffer, 0, read);
                }
            }
        }
    }
}
