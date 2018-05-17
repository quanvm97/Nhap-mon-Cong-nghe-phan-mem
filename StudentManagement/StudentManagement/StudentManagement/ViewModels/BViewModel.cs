using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using StudentManagement.ViewModels.Base;

namespace StudentManagement.ViewModels
{
    public class BViewModel : ViewModelBase
    {
        public BViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            PageTitle = "A Page";
        }


    }
}
