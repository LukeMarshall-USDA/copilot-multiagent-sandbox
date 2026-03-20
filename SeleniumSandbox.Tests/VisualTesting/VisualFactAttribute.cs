using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SeleniumSandbox.Tests.VisualTesting;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[XunitTestCaseDiscoverer("SeleniumSandbox.Tests.VisualTesting.VisualTestCaseDiscoverer", "SeleniumSandbox.Tests")]
[TraitDiscoverer("SeleniumSandbox.Tests.VisualTesting.VisualTraitDiscoverer", "SeleniumSandbox.Tests")]
public sealed class VisualFactAttribute : FactAttribute, ITraitAttribute
{
    internal const string ModeTraitName = "Mode";
    internal const string VisualModeValue = "Visual";
}

public sealed class VisualTraitDiscoverer : ITraitDiscoverer
{
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
        yield return new KeyValuePair<string, string>(VisualFactAttribute.ModeTraitName, VisualFactAttribute.VisualModeValue);
    }
}
