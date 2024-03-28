namespace Blanner.Localizations;

public interface ILocalizationManager {
	string? CurrentLocalization { get; set; }

	void Set(SupportedLocalizations localization);

    public IEnumerable<string> Localizations => [
        SupportedLocalizations.Russian.ToString(),
        SupportedLocalizations.English.ToString(),
        ];
}
