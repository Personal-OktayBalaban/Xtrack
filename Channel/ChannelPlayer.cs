using System;
using System.Collections.Generic;

namespace Xtrack.Channel
{
    public class ChannelPlayer : IChannelPlayer
    {
        public bool PlayChannel(List<IChannel> channels, int index)
        {
            if (index < 0 || index >= channels.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid channel index.");
            }

            var channel = channels[index];
            return channel.Play();
        }

        public bool PlayChannel(List<IChannel> channels, int index, TimeSpan time)
        {
            if (!isValidIndex(channels, index))
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid channel index.");
            }

            return channels[index].Play(time);
        }



        public bool PlayAllChannels(List<IChannel> channels)
        {
            bool allPlayed = true;

            foreach (var channel in channels)
            {
                if (!channel.Play())
                {
                    allPlayed = false;
                }
            }

            return allPlayed;
        }

        public bool PlayAllChannels(List<IChannel> channels, TimeSpan time)
        {
            bool allPlayed = true;

            foreach (var channel in channels)
            {
                if (!channel.Play(time))
                {
                    allPlayed = false;
                }
            }

            return allPlayed;
        }

        public bool StopChannel(List<IChannel> channels, int index)
        {
            if (index < 0 || index >= channels.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Invalid channel index.");
            }

            var channel = channels[index];
            return channel.Stop();
        }

        public bool StopAllChannels(List<IChannel> channels)
        {
            bool allStopped = true;

            foreach (var channel in channels)
            {
                if (!channel.Stop())
                {
                    allStopped = false;
                }
            }

            return allStopped;
        }

        private bool isValidIndex(List<IChannel> channels, int index)
        {
            return index >= 0 && index < channels.Count;
        }
    }
}
