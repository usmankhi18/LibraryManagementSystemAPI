using NLog;

namespace Common.FileLogger
{
    ///<summary>
    /// Static class for logger
    ///</summary>
    public static class Nlog
    {

        /*
         NLog supports the following logging levels:
         -------------------------------------------
        * DEBUG: Additional information about application behavior for cases when that information is necessary to diagnose problems
        * INFO: Application events for general purposes
        * WARN: Application events that may be an indication of a problem
        * ERROR: Typically logged in the catch block a try/catch block, includes the exception and contextual data
        * FATAL: A critical error that results in the termination of an application
        * TRACE: Used to mark the entry and exit of functions, for purposes of performance profiling
        */
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private static Logger successRequestRespLogger = LogManager.GetLogger("SuccessLogRequestResponse");
        private static Logger failureRequestRespLogger = LogManager.GetLogger("FailureLogRequestResponse");


        /// <summary>
        /// Logs application events for general purposes
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        public static void Info(string source, string actorId, string message)
        {
            Logger.Info(("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }

        /// <summary>
        /// Logs additional information about application behavior for cases when that information is necessary to diagnose problems
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        public static void Debug(string source, string actorId, string message)
        {
            Logger.Debug(("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }


        /// <summary>
        /// Typically logged in the catch block a try/catch block, includes the exception and contextual data
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        public static void Error(string source, string actorId, string message)
        {
            Logger.Error(("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }

        /// <summary>
        /// Typically logged in the catch block a try/catch block, includes the exception and contextual data
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        /// <param name="exception">Exception object</param>
        public static void Error(string source, string actorId, string message, Exception exception)
        {
            Logger.Error(exception, ("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }

        /// <summary>
        /// Typically logged in the catch block a try/catch block, includes the exception and contextual data
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="exception">Exception object</param>
        public static void Error(string source, string actorId, Exception exception)
        {
            Logger.Error(exception, ("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70));
        }

        /// <summary>
        /// Used to mark the entry and exit of functions, for purposes of performance profiling
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        public static void Trace(string source, string actorId, string message)
        {
            Logger.Trace(("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }


        /// <summary>
        /// Application events that may be an indication of a problem
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        public static void Warn(string source, string actorId, string message)
        {
            Logger.Warn(("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }


        /// <summary>
        /// A critical error that results in the termination of an application
        /// </summary>
        /// <param name="source">The source (ClassName+Function) which logs</param>
        /// <param name="actorId">Unique identifier for log messages</param>
        /// <param name="message">The message or text that you want to log</param>
        /// <param name="exception">Exception object</param>
        public static void Fatal(string source, string actorId, string message, Exception exception)
        {
            Logger.Fatal(exception, ("Source[" + source + "]").PadRight(100) + ("ActorId[" + actorId + "]").PadRight(70) +
                    "Message[" + message + "]"
                    );
        }

        /// <summary>
        /// Success Request and Response
        /// </summary>
        /// <param name="message"></param>
        public static void InfoSuccessRequestResponse(string message)
        {
            successRequestRespLogger.Info(message);
        }

        /// <summary>
        /// Failure Request and Response
        /// </summary>
        /// <param name="message"></param>
        public static void InfoFailureRequestResponse(string message)
        {
            failureRequestRespLogger.Info(message);
        }

        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Logger.Info(message);
        }


        /// <summary>
        /// Error Logger
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            Logger.Error("ERROR: " + message);
        }
    }
}
