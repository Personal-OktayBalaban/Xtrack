using System.IO;
using Xtrack.Channel;

namespace Xtrack.Common
{
    public static class TrackLoader
    {
        public static ITrack LoadTrack(string trackPath)
        {
            // Validate trackPath
            if (string.IsNullOrWhiteSpace(trackPath))
            {
                throw new ArgumentException("Track path cannot be null or empty.", nameof(trackPath));
            }

            // Check if the file exists
            if (!File.Exists(trackPath))
            {
                throw new FileNotFoundException("The specified file was not found: " + trackPath);
            }

            // Create and return a track instance
            return new Track(trackPath);
        }
    }
}

