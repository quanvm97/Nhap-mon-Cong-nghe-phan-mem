using Prism.Navigation;
using Prism.Services;
using StudentManagement.Enums;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels.ViewClassesStudentsFlow
{
    public class StudentsPageViewModel : ViewModelBase
    {
        public StudentsPageViewModel(INavigationService navigationService = null, IPageDialogService dialogService = null, ISQLiteHelper sqLiteHelper = null) :
            base(navigationService, dialogService, sqLiteHelper)
        {
            SetListStudentData();
            Title = "Danh sách";
        }

        #region property

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        private Class _class;


        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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
            Students = new ObservableCollection<Student>(Database.GetList<Student>(s => s.ClassId == _class.Id));
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
                Students = new ObservableCollection<Student>(students);
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
