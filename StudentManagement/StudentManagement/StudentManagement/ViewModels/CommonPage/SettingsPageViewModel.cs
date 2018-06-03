using Prism.Commands;
using Prism.Navigation;
using StudentManagement.Helpers;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels.Base;
using System;
using System.Windows.Input;

namespace StudentManagement.ViewModels.CommonPage
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(INavigationService navigationService, ISQLiteHelper sqLiteHelper)
            : base(navigationService, sqLiteHelper: sqLiteHelper)
        {
            // Set values
            PageTitle = "Cài đặt";

            // Commands
            SettingCommand = new DelegateCommand(SettingExecute);
            LogOutCommand = new DelegateCommand(LogOutExecute);

            InitUser();
        }

        #region Properties

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        private bool _isSettingVisible;
        public bool IsSettingVisible
        {
            get => _isSettingVisible;
            set => SetProperty(ref _isSettingVisible, value);
        }

        #endregion

        #region Command

        public ICommand SettingCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        #endregion

        #region InitUser

        private void InitUser()
        {
            CurrentUser = Database.GetUser();

            // Set View depend on user's role
            IsSettingVisible = CurrentUser.Role.Equals(RoleManager.PrincipalRole);
        }

        #endregion

        #region SettingsExecute

        private void SettingExecute()
        {
            NavigationService.NavigateAsync("ChangeRegulationsPage");
        }

        #endregion

        private void LogOutExecute()
        {
            CurrentUser = null;
            UserHelper.Instance.Logout(Database);
            NavigationService.NavigateAsync(new Uri("https://quanvm.com/LoginPage"));
        }
    }
}
