using System;
using System.IO;
using System.Reflection;

namespace JClientBot.Commons
{
    /// <summary>
    /// 앱 시작/종료 및 오류 시 Log 폴더의 JClientBot.log에 기록
    /// LOG_DIR 환경 변수가 있으면 해당 경로, 없으면 실행 파일 기준 ..\..\Log 사용
    /// </summary>
    public static class AppLog
    {
        private static string _logFilePath;
        private static readonly object _lock = new object();

        public static string LogDirectory
        {
            get
            {
                var logDir = Environment.GetEnvironmentVariable("LOG_DIR");
                if (!string.IsNullOrEmpty(logDir))
                    return logDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.GetFullPath(Path.Combine(exeDir ?? ".", "..", "..", "Log"));
            }
        }

        public static string LogFilePath
        {
            get
            {
                if (_logFilePath != null)
                    return _logFilePath;
                var dir = LogDirectory;
                try
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
                catch { /* ignore */ }
                _logFilePath = Path.Combine(dir, "JClientBot.log");
                return _logFilePath;
            }
        }

        public static void Write(string message)
        {
            try
            {
                lock (_lock)
                {
                    var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, line);
                }
            }
            catch { /* ignore */ }
        }

        public static void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }
    }
}
