using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.IO;

namespace Xtrack.Channel
{
    public class ChannelLoader : IChannelLoader
    {
        public List<IChannel> LoadChannels(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new ArgumentException("Folder path cannot be null or empty.", nameof(folderPath));
            }

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException("The specified folder does not exist: " + folderPath);
            }

            List<string> trackPaths = getTrackPaths(folderPath);

            return createChannels(trackPaths);
        }



        private List<string> getTrackPaths(string folderPath)
        {
            var allFiles = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
            return filterSupportedFiles(allFiles);
        }

        private List<string> filterSupportedFiles(IEnumerable<string> files)
        {
            var supportedExtensions = new[] { ".wav", ".mp3" };
            var supportedFiles = new List<string>();

            foreach (var file in files)
            {
                if (supportedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                {
                    supportedFiles.Add(file);
                }
            }

            return supportedFiles;
        }

        private List<IChannel> createChannels(List<string> trackPaths)
        {
            var channels = new List<IChannel>();
            foreach (var trackPath in trackPaths)
            {
                channels.Add(new Channel(trackPath));
            }
            return channels;
        }

        
    }
}

