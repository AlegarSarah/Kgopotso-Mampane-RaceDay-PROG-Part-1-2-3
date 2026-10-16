using System.Linq;
using Microsoft.AspNetCore.Authorization;
using RaceDay.API.Controllers;
using Xunit;

namespace RaceDay.Tests;

public class UnitTest1
{
    // TEST 1: ResultsController requires authentication.
    [Fact]
    public void ResultsController_RequiresAuthentication()
    {
        var attribute = typeof(ResultsController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
    }

    // TESTS 2-4: Only organisers can manage results.
    [Theory]
    [InlineData("CreateResult")]
    [InlineData("UpdateResult")]
    [InlineData("DeleteResult")]
    public void ResultManagement_RequiresOrganiserRole(
        string methodName)
    {
        var method = typeof(ResultsController)
            .GetMethod(methodName);

        Assert.NotNull(method);

        var attribute = method!
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("Organiser", attribute!.Roles);
    }

    // TEST 5: GET results has no additional role restriction.
    [Fact]
    public void GetResults_AllowsAuthenticatedUsers()
    {
        var method = typeof(ResultsController)
            .GetMethod("GetResults");

        Assert.NotNull(method);

        var roleAttribute = method!
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.Null(roleAttribute);
    }

    // TEST 6: EnrolmentsController requires authentication.
    [Fact]
    public void EnrolmentsController_RequiresAuthentication()
    {
        var attribute = typeof(EnrolmentsController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
    }

    // TEST 7: Only organisers can update enrolments.
    [Fact]
    public void UpdateEnrolment_RequiresOrganiserRole()
    {
        var method = typeof(EnrolmentsController)
            .GetMethod("UpdateEnrolment");

        Assert.NotNull(method);

        var attribute = method!
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("Organiser", attribute!.Roles);
    }

    // TEST 8: Organisers and participants can create enrolments.
    [Fact]
    public void CreateEnrolment_AllowsOrganisersAndParticipants()
    {
        var method = typeof(EnrolmentsController)
            .GetMethod("CreateEnrolment");

        Assert.NotNull(method);

        var attribute = method!
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
        Assert.Equal("Organiser,Participant", attribute!.Roles);
    }
    // TEST 9: EventsController requires authentication.
    [Fact]
    public void EventsController_RequiresAuthentication()
    {
        var attribute = typeof(EventsController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attribute);
    }

    // TESTS 10-12: Only organisers can manage events.
    [Theory]
    [InlineData("CreateEvent", "Organiser")]
    [InlineData("UpdateEvent", "Organiser")]
    [InlineData("DeleteEvent", "Organiser")]
    public void EventManagement_RequiresOrganiserRole(
         string methodName,
         string expectedRole)
    {
        var method = typeof(EventsController).GetMethod(methodName);

        Assert.NotNull(method);

        var attributes = method!
            .GetCustomAttributes(
                typeof(AuthorizeAttribute), true)
            .Cast<AuthorizeAttribute>()
            .ToArray();

        Assert.Contains(
            attributes,
            attribute => attribute.Roles == expectedRole);
    }
}