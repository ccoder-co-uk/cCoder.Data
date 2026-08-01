// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class CoreDataContextPrivilegeTests
{
    [Fact]
    public void ShouldExposePrivilegesRequiredByPrivilegeEndpoints()
    {
        // Given
        CoreDataContext context = new(
            auth: new CoreAuthInfo(),
            configuration: new DataConfiguration(),
            log: NullLogger<CoreDataContext>.Instance);

        // When
        string[] privilegeIds = context.GetAllPrivileges()
            .Select(selector: privilege => privilege.Id)
            .ToArray();

        // Then
        Assert.Contains(expected: "privilege_create", collection: privilegeIds);
        Assert.Contains(expected: "privilege_delete", collection: privilegeIds);
        Assert.Contains(expected: "privilege_read", collection: privilegeIds);
        Assert.Contains(expected: "privilege_update", collection: privilegeIds);
    }
}