using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace InstaAutoBot.Localization
{
    public static class InstaAutoBotLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(InstaAutoBotConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(InstaAutoBotLocalizationConfigurer).GetAssembly(),
                        "InstaAutoBot.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
