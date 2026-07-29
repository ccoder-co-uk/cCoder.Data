// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Eventing.Models;
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

        string ssoUserId;

        try
        {
            Type authInfoType = Type.GetType(
                typeName:
                    "cCoder.Security.Models.ISSOAuthInfo, " +
                    "cCoder.Security.Data",
                throwOnError: false);

            object authInfo = authInfoType is null
                ? null
                : serviceProvider.GetService(serviceType: authInfoType);

            ssoUserId = authInfo
                ?.GetType()
                .GetProperty(name: "SSOUserId")
                ?.GetValue(obj: authInfo)
                ?.ToString();
        }
        catch
        {
            ssoUserId = "Guest";
        }

        return string.IsNullOrWhiteSpace(value: ssoUserId)
            ? "Guest"
            : ssoUserId;
    }
}