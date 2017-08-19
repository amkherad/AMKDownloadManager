using System;

namespace AMKDownloadManager.Shell
{
    public static class ShellCommands
    {
        public static string[] ShellActivatorCommand = new []
        {
            "cli",
            "-cli",
            "--cli",
            "--shell",
        };

        /// <summary>
        /// Shows shell help.
        /// </summary>
        public static string[] HelpCommand = new []
        {
            "--help",
            "-h"
        };

        /// <summary>
        /// Download save file.
        /// </summary>
        /// <remarks>>
        /// Example:
        ///     --output=std-output
        ///     --output=C:\file1.txt
        /// Default:
        ///     --output=std-output
        /// </remarks>
        public static string[] OutputFileCommand = new []
        {
            "--output",
            "-o"
        };

        /// <summary>
        /// Determines a list of loaded components.
        /// </summary>
        /// <remarks>>
        /// Example:
        ///     --components=defaults,http1,http-proxy
        /// Default:
        ///     --components=*
        /// </remarks>
        public static string[] ComponentsCommand = new []
        {
            "--components",
            "-plugins"
        };

        /// <summary>
        /// Determines speed limit.
        /// </summary>
        /// <remarks>>
        /// Examples:
        ///     --limit=1kbs
        ///     --limit=1-kbs
        ///     --limit=1kb/s
        /// default:
        ///     --limit=0
        /// </remarks>
        public static string[] SpeedLimitCommand = new []
        {
            "--limit"
        };

        /// <summary>
        /// Determines task start mode.
        /// </summary>
        /// <remarks>
        /// Examples:
        ///     --schedule=start
        ///     --schedule=schedule
        /// Validation:
        ///     Using --shcedule=start with --queue=XXX will cause error.
        /// </remarks>
        public static string[] SchedulerCommand = new []
        {
            "--schedule"
        };

        /// <summary>
        /// Queue name.
        /// </summary>
        /// <remarks>>
        /// Examples:
        ///     --queue=0 (Main queue)
        ///     --queue="Cli queue"
        /// </remarks>
        public static string[] QueueCommand = new []
        {
            "--queue"
        };
    }
}