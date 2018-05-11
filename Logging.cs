using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace UCExtend
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// USE: Logging logging = Logging.Instance; 
    /// USE: logging.WriteToLog(message);
    /// </summary>
    public class Logging
    {
        private static Logging instance;
        private static Queue<Log> logQueue;
        private static string logDir = Settings.settingsFolderBase + @"\Logs\"; //<Path to your Log Dir or Config Setting>
        private static string logFile = "App.log"; //<Your Log File Name or Config Setting>
        private static int maxLogAge = int.Parse("15"); //<Max Age in seconds or Config Setting>
        private static int queueSize = int.Parse("10"); //<Max Queue Size or Config Setting
        private static DateTime LastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private Logging()
        {
            //Create log dir if it doesnt exist
            Directory.CreateDirectory(logDir);
        }
 
        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static Logging Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (instance == null)
                {
                    instance = new Logging();
                    logQueue = new Queue<Log>();
                }
                return instance;
            }
        }
 
        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (logQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(message);
                logQueue.Enqueue(logEntry);
 
                // If we have reached the Queue Size then flush the Queue
                if (logQueue.Count >= queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }            
        }
 
        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= maxLogAge)
            {
                LastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }
 
        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog()
        {
            while (logQueue.Count > 0)
            {
                Log entry = logQueue.Dequeue();
                string logPath = logDir + entry.LogDate + "_" + logFile;
 
		// This could be optimised to prevent opening and closing the file for each write
                using (FileStream fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter log = new StreamWriter(fs))
                    {
                        log.WriteLine(string.Format("{0}\t{1}",entry.LogTime,entry.Message));
                    }
                }
            }            
        }
    }
 
    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }
 
        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("hh:mm:ss.fff tt");
        }
    }
}
