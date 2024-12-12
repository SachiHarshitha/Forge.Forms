using System;
using Forge.Application.AvaloniaUI.Localization;

namespace Forge.Application.AvaloniaUI.Infrastructure
{
    public interface ILocalizationService
    {
        Language CurrentLanguage { get; }

        string GetString(string key);

        event EventHandler LanguageChanged;

        void SwitchLanguage(string languageKey);

        void RegisterLanguage(string languageKey, Language language);

        Language GetLanguage(string languageKey);
    }

    public static class LocalizationServiceExtensions
    {
        public static void SubscribeLanguageChanged(this ILocalizationService localizationService,
            EventHandler<EventArgs> handler)
        {
            WeakEventManager<ILocalizationService, EventArgs>.AddHandler(localizationService,
                nameof(ILocalizationService.LanguageChanged), handler);
        }
    }
}
