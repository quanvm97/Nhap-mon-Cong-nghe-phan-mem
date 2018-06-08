using Prism.Navigation;
using StudentManagement.ViewModels.Base;

namespace StudentManagement.ViewModels.MainPageTabbed
{
    public class AdminRoleMainPageViewModel : ViewModelBase
    {
        public AdminRoleMainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            PageTitle = "Main Page";
        }
    }
}
