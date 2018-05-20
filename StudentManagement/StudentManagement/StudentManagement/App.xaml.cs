using System;
using DryIoc;
using Prism.DryIoc;
using Prism.Navigation;
using StudentManagement.Enums;
using StudentManagement.Helpers;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.Services.LocalDatabase;
using StudentManagement.Views;
using StudentManagement.Views.AddStudentsFlow;
using StudentManagement.Views.ViewClassesStudentsFlow;
using Xamarin.Forms;

namespace StudentManagement
{
    public partial class App : PrismApplication
    {
        #region Properties

        private ISQLiteHelper _sqLiteHelper;
        #endregion

        public App()
        {
            
        }

        protected override async void OnInitialized()
        {
            InitDatabase();
            InitMockData();
            InitializeComponent();

            await NavigationService.NavigateAsync("MainPage");
            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
            //StartApp();
        }

        protected override void RegisterTypes()
        {
            // Register Pages
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            //Home Flow
            Container.RegisterTypeForNavigation<HomePage>();
            Container.RegisterTypeForNavigation<ClassesPage>();
            Container.RegisterTypeForNavigation<StudentsPage>();
            Container.RegisterTypeForNavigation<ClassDetailPage>();
            Container.RegisterTypeForNavigation<StudentDetailPage>();
            Container.RegisterTypeForNavigation<ScoreBoardPage>();
            Container.RegisterTypeForNavigation<AddNewStudentPage>();
            Container.RegisterTypeForNavigation<ChooseClassPage>();


            Container.RegisterTypeForNavigation<A>();
            Container.RegisterTypeForNavigation<B>();
            Container.RegisterTypeForNavigation<C>();

            // Register Services
            Container.Register<ISQLiteHelper, SQLiteHelper>(Reuse.ScopedOrSingleton);
        }

        private void InitDatabase()
        {
            var connectionService = DependencyService.Get<IDatabaseConnection>();
            _sqLiteHelper = new SQLiteHelper(connectionService);
        }

        private void InitMockData()
        {
            var setting = _sqLiteHelper.Get<Setting>("1");
            if (setting != null)
            {
                if (!setting.IsInitData)
                {
                    var mockData = new MockData(_sqLiteHelper);
                    mockData.InitMockData();
                }
            }
            else
            {
                var mockData = new MockData(_sqLiteHelper);
                mockData.InitMockData();
            }
        }

        //private async void StartApp()
        //{
        //    var user = _sqLiteHelper.GetUser();

        //    string uri = PageManager.MultiplePage(new[]
        //    {
        //        PageManager.HomePage, PageManager.NavigationPage,
        //    });
        //    var navParam = new NavigationParameters();

        //    if (user == null)
        //        uri = PageManager.LoginPage; 
        //    // If PrincipalRole
        //    else if (user.Role.Equals(RoleManager.PrincipalRole))
        //        uri += "/" + PageManager.ListClassesPage;
        //    // If TeacherRole
        //    else if (user.Role.Equals(RoleManager.TeacherRole))
        //    {
        //        uri += "/" + PageManager.DetailClassPage;
        //        var classInfo = _sqLiteHelper.Get<Class>(c => c.Id == user.ClassId);
        //        classInfo.CountStudent(_sqLiteHelper);
        //        navParam.Add(ParamKey.DetailClassPageType.ToString(), DetailClassPageType.ClassInfo);
        //        navParam.Add(ParamKey.ClassInfo.ToString(), classInfo);
        //    }
        //    // If StudentRole
        //    else
        //    {
        //        uri += "/" + PageManager.DetailStudentPage;
        //        navParam.Add(ParamKey.DetailStudentPageType.ToString(), DetailStudentPageType.StudentInfo);
        //        navParam.Add(ParamKey.StudentInfo.ToString(), _sqLiteHelper.Get<Student>(s => s.Id == user.Id));
        //    }

        //    await NavigationService.NavigateAsync(new Uri($"https://kienhht.com/{uri}"), navParam);
        //}
    }
}
