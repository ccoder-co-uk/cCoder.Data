// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.CodeAnalysis.Exposures;

namespace Data.Web.Brokers.Loggings;

internal sealed class LoggingBroker(ILogger<LoggingBroker> logger) :
    ILoggingBroker,
    IUtilityBroker
{
    public void LogError(Exception exception, string message, params object[] args) =>
        logger.LogError(exception: exception, message: message, args: args);
}