using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StudentManagement.Enums;
using StudentManagement.Helpers;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels.Base;
using StudentManagement.Views.AddStudentsFlow;
using StudentManagement.Views.Popups;
using Xamarin.Forms;

namespace StudentManagement.ViewModels.ViewClassesStudentsFlow
{
    public class ClassesPageViewModel : ViewModelBase
    {
        public ClassesPageViewModel(INavigationService navigationService = null, IPageDialogService dialogService = null, ISQLiteHelper sqLiteHelper = null) :
            base(navigationService, dialogService, sqLiteHelper)
        {
            SearchToolbarItemsCommand = new DelegateCommand(SearchToolbarItemsExecute);

            var classes = sqLiteHelper.GetList<Class>(c => c.Id > 0);
            foreach (var c in classes) c.CountStudent(sqLiteHelper);
            Classes = new ObservableCollection<Class>(classes);
        }

        

        #region property

        private ObservableCollection<Class> _classes;

        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set => SetProperty(ref _classes, value);
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private bool _showSearchBox;

        public bool ShowSearchBox
        {
            get => _showSearchBox;
            set => SetProperty(ref _showSearchBox, value);
        }

        #endregion

        #region Search Command

        public ICommand SearchToolbarItemsCommand { get; set; }

        private void SearchToolbarItemsExecute()
        {
            //var serchResult = Classes.Where(s => StringHelper.RemoveUnicodeCharacter(s.Name.ToLower()).Contains(StringHelper.RemoveUnicodeCharacter(SearchText.ToLower())));
            //Classes = new ObservableCollection<Class>(serchResult);

            ConfirmPasswordPopup.Instance.Show("A");
        }

        #endregion


        public void ClassesItemTapped(Class _class)
        {
            var navParam = new NavigationParameters
            {
                { ParamKey.DetailClassPageType.ToString(), DetailClassPageType.ClassInfo },
                { ParamKey.ClassInfo.ToString(), _class }
            };
            NavigationService.NavigateAsync("ClassDetailPage", navParam);
        }
    }

}
