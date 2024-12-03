using Bunit;
using Bunit.TestDoubles;

namespace Shopping.List.Tests.Components;

public static class TestContextExtensions
{
    public static void SetupTestContext(this TestContext testContext)
    {
        testContext.AddFakePersistentComponentState();
        testContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }
}