using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using StudentManagement.Interfaces;

namespace StudentManagement.ViewModels.Base
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        public INavigationService NavigationService { get; private set; }
        public IPageDialogService Dialog { get; private set; }
        public ISQLiteHelper Database { get; private set; }
        public ICommand BCommand { get; set; }

        private async void BExecute()
        {
            await NavigationService.NavigateAsync("NavigationPage/B");
        }

        public ICommand ACommand { get; set; }

        private async void AExecute()
        {
            await NavigationService.NavigateAsync("NavigationPage/A");
        }

        public ICommand CCommand { get; set; }

        private async void CExecute()
        {
            await NavigationService.NavigateAsync("C");
        }
        public ViewModelBase(
            INavigationService navigationService = null,
            IPageDialogService dialogService = null,
            ISQLiteHelper sqLiteHelper = null)
        {
            if (navigationService != null) NavigationService = navigationService;
            if (dialogService != null) Dialog = dialogService;
            if (sqLiteHelper != null) Database = sqLiteHelper;

            BCommand = new DelegateCommand(BExecute);

            ACommand = new DelegateCommand(AExecute);

            CCommand = new DelegateCommand(CExecute);
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("__NavigationMode"))
                {
                    var navMode = (NavigationMode)parameters["__NavigationMode"];
                    switch (navMode)
                    {
                        case NavigationMode.New: OnNavigatedNewTo(parameters); break;
                        case NavigationMode.Back: OnNavigatedBackTo(parameters); break;
                    }
                }
            }
        }

        public virtual void OnNavigatedNewTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedBackTo(NavigationParameters parameters)
        {

        }

        #region General Properties

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        #endregion
    }
}
