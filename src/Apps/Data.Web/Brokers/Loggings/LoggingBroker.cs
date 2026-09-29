// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System;
using cCoder.CodeAnalysis.Exposures;
using Microsoft.Extensions.Logging;

namespace Data.Web.Brokers.Loggings;

internal sealed class LoggingBroker(ILogger<LoggingBroker> logger) :
    ILoggingBroker,
    IUtilityBroker
{
    public void LogError(Exception exception, string message, params object[] args) =>
        logger.LogError(exception: exception, message: message, args: args);
}