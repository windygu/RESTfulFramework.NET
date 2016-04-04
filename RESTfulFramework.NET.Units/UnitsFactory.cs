using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Units
{
    internal class UnitsFactory
    {
        internal static IJsonSerialzer JsonSerialzer { get;private set; } = PluginPackage.Factory.GetInstance<IJsonSerialzer>();
        internal static ILogManager LogManager { get;private set; } = PluginPackage.Factory.GetInstance<ILogManager>();
        internal static IConfigManager<SysConfigModel> ConfigManager { get;private set; } = PluginPackage.Factory.GetInstance<IConfigManager<SysConfigModel>>();

    }
}
