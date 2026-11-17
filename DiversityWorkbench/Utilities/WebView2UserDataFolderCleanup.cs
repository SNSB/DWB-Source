using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Utilities
{
    /// <summary>
    /// Cleans up stale WebView2 user data folders from previous application runs.
    /// </summary>
    public static class WebView2UserDataFolderCleanup
    {
        private const string SessionPrefix = "Session_";
        private const string RetryPrefix = "Retry_";

        public static void CleanupCurrentSessionFolder()
        {
            try
            {
                var processId = Process.GetCurrentProcess().Id;
                var processName = GetSafeFolderName(Process.GetCurrentProcess().ProcessName);

                var folderToDelete = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "DiversityWorkbench",
                    "NETWebView",
                    processName,
                    $"{SessionPrefix}{processId}");

                if (Directory.Exists(folderToDelete))
                {
                    try
                    {
                        Directory.Delete(folderToDelete, true);
                    }
                    catch
                    {
                        // Ignore failures - WebView2 prozesse könnten noch Dateien halten
                    }
                }
            }
            catch
            {
                // Silently ignore cleanup errors during shutdown
            }
        }

        public static void CleanupStaleSessions()
        {
            var root = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DiversityWorkbench",
                "NETWebView");

            CleanupStaleSessions(root);
        }

        private static void CleanupStaleSessions(string root)
        {
            if (!Directory.Exists(root))
                return;

            try
            {
                // Recursively find all Session_* and Retry_* folders
                foreach (var directory in Directory.GetDirectories(root, $"{SessionPrefix}*", SearchOption.AllDirectories))
                {
                    TryDeleteStaleSessionFolder(directory);
                }

                // Also clean up old retry folders (these are safe to delete if old enough)
                foreach (var directory in Directory.GetDirectories(root, $"{RetryPrefix}*", SearchOption.AllDirectories))
                {
                    TryDeleteOldRetryFolder(directory);
                }
            }
            catch (Exception ex)
            {
                // Log but don't crash on cleanup errors
                System.Diagnostics.Debug.WriteLine($"WebView2 cleanup warning: {ex.Message}");
            }
        }

        private static void TryDeleteStaleSessionFolder(string folderPath)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(folderPath);
                var folderName = directoryInfo.Name;

                if (!folderName.StartsWith(SessionPrefix, StringComparison.OrdinalIgnoreCase))
                    return;

                var processIdText = folderName.Substring(SessionPrefix.Length);

                if (!int.TryParse(processIdText, out var processId))
                    return;

                // Keep if process is still running
                if (IsProcessRunning(processId))
                    return;

                // Delete the stale session folder
                Directory.Delete(folderPath, true);
            }
            catch
            {
                // Silently ignore deletion failures
                // WebView2, antivirus, indexing, or file locks may prevent deletion
                // It will be cleaned up on a future startup
            }
        }

        private static void TryDeleteOldRetryFolder(string folderPath)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(folderPath);

                // Only delete retry folders older than 1 day
                // (in case a retry folder is being actively used)
                if (DateTime.UtcNow - directoryInfo.LastWriteTimeUtc < TimeSpan.FromHours(24))
                    return;

                Directory.Delete(folderPath, true);
            }
            catch
            {
                // Silently ignore deletion failures
            }
        }

        private static bool IsProcessRunning(int processId)
        {
            try
            {
                using var process = Process.GetProcessById(processId);
                return !process.HasExited;
            }
            catch
            {
                // Process doesn't exist or can't be queried
                return false;
            }
        }

        private static string GetSafeFolderName(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                folderName = "Application";

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                folderName = folderName.Replace(invalidChar, '_');
            }

            return folderName;
        }
    }
}