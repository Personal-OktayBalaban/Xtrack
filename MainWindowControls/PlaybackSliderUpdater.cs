using System;
using System.Windows.Threading;

namespace Xtrack.MainWindowControls
{
    public class PlaybackSliderUpdater : IPlaybackSliderUpdater
    {
        private readonly Func<TimeSpan> _getTrackLength;
        private readonly Action<TimeSpan, TimeSpan> _onUpdateTime;

        private readonly DispatcherTimer _timer;
        private TimeSpan _currentTime;

        private bool _isPlaying;

        public PlaybackSliderUpdater(Func<TimeSpan> getTrackLength, Action<TimeSpan, TimeSpan> onUpdateTime)
        {
            _getTrackLength = getTrackLength ?? throw new ArgumentNullException(nameof(getTrackLength));
            _onUpdateTime = onUpdateTime ?? throw new ArgumentNullException(nameof(onUpdateTime));

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += Timer_Tick;

            _currentTime = TimeSpan.Zero;
            _isPlaying = false;
        }

        public void Start(TimeSpan initialTime = default)
        {
            _currentTime = initialTime;
            _isPlaying = true;
            _timer.Start();
        }

        public void Stop()
        {
            _isPlaying = false;
            _timer.Stop();
        }

        public void SetCurrentTime(TimeSpan time)
        {
            _currentTime = time;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!_isPlaying) return;

            var totalTime = _getTrackLength();
            if (_currentTime >= totalTime)
            {
                _currentTime = totalTime;
                Stop(); 
            }
            else
            {
                _currentTime = _currentTime.Add(_timer.Interval);
            }

            _onUpdateTime(_currentTime, totalTime);
        }
    }
}
