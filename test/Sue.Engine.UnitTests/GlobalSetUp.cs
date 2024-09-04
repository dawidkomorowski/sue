using NLog.Config;
using NLog.Targets;
using NLog;
using NUnit.Framework;

namespace Sue.Engine.UnitTests;

[SetUpFixture]
public sealed class GlobalSetUp
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var loggingConfiguration = new LoggingConfiguration();
        loggingConfiguration.AddRuleForAllLevels(new ConsoleTarget());
        LogManager.Configuration = loggingConfiguration;
    }
}