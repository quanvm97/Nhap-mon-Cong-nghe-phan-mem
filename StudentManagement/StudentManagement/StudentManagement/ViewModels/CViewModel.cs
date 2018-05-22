using Prism.Navigation;
using StudentManagement.ViewModels.Base;

namespace StudentManagement.ViewModels
{
    public class CViewModel : ViewModelBase
    {
        public CViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            PageTitle = "A Page";
        }


    }
}
