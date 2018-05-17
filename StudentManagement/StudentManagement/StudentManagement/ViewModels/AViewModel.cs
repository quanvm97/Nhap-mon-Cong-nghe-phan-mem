using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using StudentManagement.ViewModels.Base;

namespace DemoBottomBar.ViewModels
{
    public class AViewModel : ViewModelBase
    {
        public AViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            PageTitle = "A Page";
        }


    }
}
