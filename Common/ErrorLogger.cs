using System;
using System.Windows;

namespace Common
{
    public static class ErrorLogger
    {
        private static string _lastMessage = string.Empty;

        public static void LogError(string message)
        {
            _lastMessage = message;
        }

        public static string GetLastErrorMessage()
        {
            return _lastMessage;
        }

        public static void ClearLastError()
        {
            _lastMessage = string.Empty;
        }

        public static void RaiseError(Window window)
        {
            string errorMessage = GetLastErrorMessage();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(window, errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                ClearLastError();
            }
            else
            {
                MessageBox.Show(window, "An Error Occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
