using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StudentManagement.Enums;
using StudentManagement.Helpers;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudentManagement.ViewModels.ViewClassesStudentsFlow
{
    public class StudentsPageViewModel : ViewModelBase
    {
        public StudentsPageViewModel(INavigationService navigationService = null, IPageDialogService dialogService = null, ISQLiteHelper sqLiteHelper = null) :
            base(navigationService, dialogService, sqLiteHelper)
        {
            Title = "Danh sách";

            user = Database.GetUser();

            if (user.Role.Equals(RoleManager.StudentRole))
                GetStudentsInClass();
            else if (!user.Role.Equals(RoleManager.TeacherRole))
                SetListStudentData();

            // Commands
            SearchToolbarItemsCommand = new DelegateCommand(SearchToolbarItemsExecute);
            SearchIconCommand = new DelegateCommand(SearchIconExecute);
            FilterIconCommand = new DelegateCommand(FilterIconExecute);
        }

        #region Properties

        private bool _showSearchBox;
        public bool ShowSearchBox
        {
            get => _showSearchBox;
            set => SetProperty(ref _showSearchBox, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchExecute(value);
            }
        }

        #endregion

        #region Search Execute

        private void SearchExecute(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var serchResult = ListStudents.Where(s =>
                    StringHelper.RemoveUnicodeCharacter(s.FullName.ToLower())
                        .Contains(StringHelper.RemoveUnicodeCharacter(text.ToLower())));
                Students = new ObservableCollection<Student>(serchResult);
            }
            else
                Students = ListStudents;

        }

        #endregion

        #region BackgroundColor

        private Color _clickedSearchBackgroundColor = Color.FromHex("#F1EFEF");

        public Color ClickedSearchBackgroundColor
        {
            get => _clickedSearchBackgroundColor;
            set => this.SetProperty(ref _clickedSearchBackgroundColor, value);
        }

        private Color _clickedFilterBackgroundColor = Color.FromHex("#F1EFEF");

        public Color ClickedFilterBackgroundColor
        {
            get => _clickedFilterBackgroundColor;
            set => this.SetProperty(ref _clickedFilterBackgroundColor, value);
        }

        #endregion

        #region Search student

        // Commands
        public ICommand SearchToolbarItemsCommand { get; set; }
        public ICommand SearchIconCommand { get; set; }
        public ICommand FilterIconCommand { get; set; }

        private void SearchToolbarItemsExecute()
        {
            ShowSearchBox = !ShowSearchBox;
        }

        private async void SearchIconExecute()
        {

            ClickedSearchBackgroundColor = Color.White;

            await Task.Delay(200);

            ClickedSearchBackgroundColor = Color.FromHex("#F1EFEF");

            //var serchResult = Students.Where(s =>
            //    StringHelper.RemoveUnicodeCharacter(s.FullName.ToLower())
            //        .Contains(StringHelper.RemoveUnicodeCharacter(SearchText.ToLower())));
            //Students = new ObservableCollection<Student>(serchResult);
        }

        private async void FilterIconExecute()
        {
            ClickedFilterBackgroundColor = Color.White;

            await Task.Delay(200);

            ClickedFilterBackgroundColor = Color.FromHex("#F1EFEF");

            await NavigationService.NavigateAsync("SearchPage");

            //var serchResult = Students.Where(s =>
            //    StringHelper.RemoveUnicodeCharacter(s.FullName.ToLower())
            //        .Contains(StringHelper.RemoveUnicodeCharacter(SearchText.ToLower())));
            //Students = new ObservableCollection<Student>(serchResult);
        }

        #endregion

        private void GetStudentsInClass()
        {
            var classStudent = Database.Get<Class>(s => s.Id == user.ClassId);
            SetClassInfo(classStudent);
            PageTitle = "Thông tin học sinh";
        }

        #region property

        private User user;

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        private ObservableCollection<Student> _listStudents;

        public ObservableCollection<Student> ListStudents
        {
            get => _listStudents;
            set => SetProperty(ref _listStudents, value);
        }

        private Class _class;


        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _showOneClassOnly = false;
        public bool ShowOneClassOnly
        {
            get => _showOneClassOnly;
            set => SetProperty(ref _showOneClassOnly, value);
        }

        #endregion

        #region override

        public override void OnNavigatedNewTo(NavigationParameters parameters)
        {
            base.OnNavigatedNewTo(parameters);

            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.ClassInfo.ToString()))
                {
                    ShowOneClassOnly = (bool)parameters[ParamKey.ShowOneClassOnly.ToString()];
                    SetClassInfo((Class)parameters[ParamKey.ClassInfo.ToString()]);
                    return;
                }
            }

            SetListStudentData();
            Title = "Học sinh";
        }

        private void SetClassInfo(Class classInfo)
        {
            _class = classInfo;
            Title = "Danh sách lớp " + _class.Name;
            ListStudents = Students =
                new ObservableCollection<Student>(Database.GetList<Student>(s => s.ClassId == _class.Id));
        }



        private async void SetListStudentData()
        {
            //LoadingPopup.Instance.ShowLoading();
            await Task.Run(() =>
            {
                var students = Database.GetList<Student>(s => s.Id > 0);
                foreach (var student in students)
                {
                    student.GetAvgScore(Database);
                }

                if (!ShowOneClassOnly)
                    ListStudents = Students = new ObservableCollection<Student>(students);

            });
            //LoadingPopup.Instance.HideLoading();
        }

        #endregion

        public void StudentItemTapped(Student student)
        {
            var navParam = new NavigationParameters
            {
                { ParamKey.StudentInfo.ToString(), student }
            };
            NavigationService.NavigateAsync("StudentDetailPage", navParam);
        }
    }
}
