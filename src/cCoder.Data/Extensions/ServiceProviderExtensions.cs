// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing.Models;
using cCoder.Security.Models.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace cCoder.Data.Extensions;

internal static class ServiceProviderExtensions
{
    internal static string ResolveSsoUserId(
        this IServiceProvider serviceProvider)
    {
        string eventUserId = serviceProvider
            .GetService<IEventAuthInfo>()
            ?.SSOUserId;

        if (!string.IsNullOrWhiteSpace(value: eventUserId))
        {
            return eventUserId;
        }

        string ssoUserId = serviceProvider
            .GetService<ISSOAuthInfo>()
            ?.SSOUserId;

        return string.IsNullOrWhiteSpace(value: ssoUserId)
            ? "Guest"
            : ssoUserId;
    }
}