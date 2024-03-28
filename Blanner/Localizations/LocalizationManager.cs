using Microsoft.AspNetCore.Components;

namespace Blanner.Localizations;

public sealed class LocalizationManager : ILocalization, ILocalizationManager {
    private SupportedLocalizations _localization;
    private ILocalization _localizationStrategy;
    public LocalizationManager()
    {

		_localizationStrategy = new RussianLocalization();
    }

    public string? CurrentLocalization {
        get { return _localization.ToString(); }
        set {
            if(Enum.TryParse(value, out _localization)) {
                Set(_localization);
            }
        }
    }

    public void Set(SupportedLocalizations localization) {
		(_localizationStrategy, _localization) = localization switch {
            SupportedLocalizations.Russian => new ValueTuple<ILocalization, SupportedLocalizations>(new RussianLocalization(), SupportedLocalizations.Russian),
            SupportedLocalizations.English => (new EnglishLocalization(), SupportedLocalizations.English),
            _ => (new RussianLocalization(), SupportedLocalizations.Russian)
        };
    }

    public string HomePageTitle => _localizationStrategy.HomePageTitle;
    public string HomePageWelcome => _localizationStrategy.HomePageWelcome;
    public string AuthMenuAdminDashboard => _localizationStrategy.AuthMenuAdminDashboard;
    public string AuthMenuBtnLogout => _localizationStrategy.AuthMenuBtnLogout;
    public string AuthMenuBtnRegister => _localizationStrategy.AuthMenuBtnRegister;
    public string AuthMenuBtnLogin => _localizationStrategy.AuthMenuBtnLogin;
    public string BtnAbout => _localizationStrategy.BtnAbout;
    public string BtnReload => _localizationStrategy.BtnReload;
    public string MessageUnhadledErrorOccured => _localizationStrategy.MessageUnhadledErrorOccured;
    public string NavMenuAppName => _localizationStrategy.NavMenuAppName;
    public string NavMenuHomePage => _localizationStrategy.NavMenuHomePage;
    public string NavMenuGoals => _localizationStrategy.NavMenuGoals;
    public string NavMenuContractors => _localizationStrategy.NavMenuContractors;
    public string NavMenuJobs => _localizationStrategy.NavMenuJobs;
    public string NavMenuSticky => _localizationStrategy.NavMenuSticky;
    public string BtnOk => _localizationStrategy.BtnOk;
    public string ErrorPageTitle => _localizationStrategy.ErrorPageTitle;
    public string ErrorPageSubHeader => _localizationStrategy.ErrorPageSubHeader;
    public string ErrorPageLabelRequestId => _localizationStrategy.ErrorPageLabelRequestId;
    public string ErrorPageMessage => _localizationStrategy.ErrorPageMessage;
    public string JobsPageTitle => _localizationStrategy.JobsPageTitle;
    public string ContractorNoneName => _localizationStrategy.ContractorNoneName;
    public string ContractorsPageTitle => _localizationStrategy.ContractorsPageTitle;
    public string NameInputLabel => _localizationStrategy.NameInputLabel;
    public string ContractorInputLabel => _localizationStrategy.ContractorInputLabel;
    public string BtnAdd => _localizationStrategy.BtnAdd;
    public string BtnSave => _localizationStrategy.BtnSave;
    public string BtnClear => _localizationStrategy.BtnClear;
    public string OptionEmpty => _localizationStrategy.OptionEmpty;
    public string ActiveGoalsTimesLabel => _localizationStrategy.ActiveGoalsTimesLabel;
    public string IdLabel => _localizationStrategy.IdLabel;
    public string FromTimeLabel => _localizationStrategy.FromTimeLabel;
    public string ToTimeLabel => _localizationStrategy.ToTimeLabel;
    public string TimeLabel => _localizationStrategy.TimeLabel;
    public string NameLabel => _localizationStrategy.NameLabel;
    public string TotalTimeLabel => _localizationStrategy.TotalTimeLabel;
    public string ElapsedTimeLabel => _localizationStrategy.ElapsedTimeLabel;
    public string BtnDelete => _localizationStrategy.BtnDelete;
    public string BtnPersist => _localizationStrategy.BtnPersist;
    public string GoalEditorHeader => _localizationStrategy.GoalEditorHeader;
    public string BtnSaveAndClose => _localizationStrategy.BtnSaveAndClose;
    public string BtnCreate => _localizationStrategy.BtnCreate;
    public string GoalsPageTitle => _localizationStrategy.GoalsPageTitle;
    public string BtnGoals => _localizationStrategy.BtnGoals;
    public string BtnCompleteJob => _localizationStrategy.BtnCompleteJob;
    public string BtnBuildJob => _localizationStrategy.BtnBuildJob;
}

public sealed class RussianLocalization : ILocalization {
    public string HomePageTitle { get; } = "Главная";
    public string HomePageWelcome { get; } = "Добро пожаловать!";

    public string AuthMenuAdminDashboard { get; } = "Администрирование";
    public string AuthMenuBtnLogout { get; } = "Выход";
    public string AuthMenuBtnRegister { get; } = "Регистрация";
    public string AuthMenuBtnLogin { get; } = "Вход";
    public string BtnAbout { get; } = "О программе";
    public string BtnReload { get; } = "Перезагрузить";
    public string MessageUnhadledErrorOccured { get; } = "Случилось непредвиденное!";
    public string NavMenuAppName { get; } = "Бланнер";
    public string NavMenuHomePage { get; } = "Главная";
    public string NavMenuGoals { get; } = "Задачи";
    public string NavMenuContractors { get; } = "Контрагенты";
    public string NavMenuJobs { get; } = "Работы";
    public string NavMenuSticky { get; } = "Прилипалы";
    public string BtnOk { get; } = "Ок";
    public string ErrorPageTitle { get; } = "Ошибка";
    public string ErrorPageSubHeader { get; } = "Ошибка случилась пока вы ползали по сайту";
    public string ErrorPageLabelRequestId { get; } = "Идентификатор запроса";
    public string ErrorPageMessage { get; } = "По идее вы не должны видеть это сообщение, таки дела.";
    public string JobsPageTitle { get; } = "Работы";
    public string ContractorNoneName { get; } = "Остальные";
    public string ContractorsPageTitle { get; } = "Контрагенты";
    public string NameInputLabel { get; } = "Имя";
    public string ContractorInputLabel { get; } = "Контрагент";
    public string BtnAdd { get; } = "Добавить";
    public string BtnSave { get; } = "Сохранить";
    public string BtnClear { get; } = "Очистить";
    public string OptionEmpty { get; } = "Не выбрано";
    public string ActiveGoalsTimesLabel { get; } = "Потерянное время";
    public string IdLabel { get; } = "№";
    public string FromTimeLabel { get; } = "От";
    public string ToTimeLabel { get; } = "До";
    public string TimeLabel { get; } = "Время";
    public string NameLabel { get; } = "Имя";
    public string TotalTimeLabel { get; } = "Всего";
    public string ElapsedTimeLabel { get; } = "Прошло";
    public string BtnDelete { get; } = "Удалить";
    public string BtnPersist { get; } = "Сохранить";
    public string GoalEditorHeader { get; } = "Задача";
    public string BtnSaveAndClose { get; } = "Сохранить и Закрыть";
    public string BtnCreate { get; } = "Создать";
    public string GoalsPageTitle { get; } = "Задачи";
    public string BtnGoals { get; } = "Задачи";
    public string BtnCompleteJob { get; } = "Завершить";
    public string BtnBuildJob { get; } = "Собрать работу";
}

public sealed class EnglishLocalization : ILocalization {
    public string HomePageTitle { get; } = "Home";
    public string HomePageWelcome { get; } = "Welcome in new App!";

    public string AuthMenuAdminDashboard { get; } = "Admin";
    public string AuthMenuBtnLogout { get; } = "Logout";
    public string AuthMenuBtnRegister { get; } = "Register";
    public string AuthMenuBtnLogin { get; } = "Login";
    public string BtnAbout { get; } = "About";
    public string BtnReload { get; } = "Reload";
    public string MessageUnhadledErrorOccured { get; } = "An unhandled error has occurred.";
    public string NavMenuAppName { get; } = "Blanner";
    public string NavMenuHomePage { get; } = "Home";
    public string NavMenuGoals { get; } = "Goals";
    public string NavMenuContractors { get; } = "Contractors";
    public string NavMenuJobs { get; } = "Jobs";
    public string NavMenuSticky { get; } = "Sticky";
    public string BtnOk { get; } = "Ok";
    public string ErrorPageTitle { get; } = "Error";
    public string ErrorPageSubHeader { get; } = "An error occurred while processing your request.";
    public string ErrorPageLabelRequestId { get; } = "Request ID";
    public string ErrorPageMessage { get; } = """
        <h3>Development Mode</h3>
        <p>
            Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.
        </p>
        <p>
            <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
            It can result in displaying sensitive information from exceptions to end users.
            For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
            and restarting the app.
        </p>
        """;
    public string JobsPageTitle { get; } = "Jobs";
    public string ContractorNoneName { get; } = "Other";
    public string ContractorsPageTitle { get; } = "Contractors";
    public string NameInputLabel { get; } = "Name";
    public string BtnAdd { get; } = "Add";
    public string ContractorInputLabel { get; } = "Contractor";
    public string BtnSave { get; } = "Save";
    public string BtnClear { get; } = "Clear";
    public string OptionEmpty { get; } = "Empty";
    public string ActiveGoalsTimesLabel { get; } = "Times";
    public string IdLabel { get; } = "Id";
    public string FromTimeLabel { get; } = "From";
    public string ToTimeLabel { get; } = "To";
    public string TimeLabel { get; } = "Time";
	public string TotalTimeLabel { get; } = "Total time";
    public string ElapsedTimeLabel { get; } = "Elapsed";
    public string BtnDelete { get; } = "Delete";
    public string BtnPersist { get; } = "Persist";
    public string NameLabel { get; } = "Name";
    public string GoalEditorHeader { get; } = "Goal";
    public string BtnSaveAndClose { get; } = "Save and Close";
    public string BtnCreate { get; } = "Create";
    public string GoalsPageTitle { get; } = "Goals";
    public string BtnGoals { get; } = "Goals";
    public string BtnCompleteJob { get; } = "Complete";
    public string BtnBuildJob { get; } = "Build Job";
}