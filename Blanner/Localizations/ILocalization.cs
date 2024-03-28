using Microsoft.AspNetCore.Components;

namespace Blanner.Localizations;

public interface ILocalization {
    string HomePageTitle { get; }
    string HomePageWelcome { get; }

    string AuthMenuAdminDashboard { get; }
    string AuthMenuBtnLogout { get; }
    string AuthMenuBtnRegister { get; }
    string AuthMenuBtnLogin { get; }

    string BtnAbout { get; }
    string BtnReload { get; }
    string MessageUnhadledErrorOccured { get; }

    string NavMenuAppName { get; }
    string NavMenuHomePage { get; }
    string NavMenuGoals { get; }
    string NavMenuContractors { get; }
    string NavMenuJobs { get; }
    string NavMenuSticky { get; }

    string BtnOk { get; }

    string ErrorPageTitle { get; }
    string ErrorPageSubHeader { get; }
    string ErrorPageLabelRequestId { get; }
    string ErrorPageMessage { get; }

    string JobsPageTitle { get; }
    string ContractorNoneName { get; }
    string ContractorsPageTitle { get; }
    string NameInputLabel { get; }
    string ContractorInputLabel { get; }
    string BtnAdd { get; }
    string BtnSave { get; }
    string BtnClear { get; }
    string OptionEmpty { get; }
    string ActiveGoalsTimesLabel { get; }
    string IdLabel { get; }
    string FromTimeLabel { get; }
    string ToTimeLabel { get; }
    string TimeLabel { get; }
    string NameLabel { get; }
    string TotalTimeLabel { get; }
    string ElapsedTimeLabel { get; }
    string BtnDelete { get; }
    string BtnPersist { get; }
    string GoalEditorHeader { get; }
    string BtnSaveAndClose { get; }
    string BtnCreate { get; }
    string GoalsPageTitle { get; }
    string BtnGoals { get; }
    string BtnCompleteJob { get; }
    string BtnBuildJob { get; }
}
