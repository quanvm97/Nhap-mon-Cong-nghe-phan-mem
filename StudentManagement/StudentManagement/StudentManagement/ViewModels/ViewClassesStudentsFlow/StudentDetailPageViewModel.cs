using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using StudentManagement.Enums;
using StudentManagement.Interfaces;
using StudentManagement.Models;
using StudentManagement.ViewModels.Base;
using StudentManagement.Views.Popups;

namespace StudentManagement.ViewModels.ViewClassesStudentsFlow
{
    public class StudentDetailPageViewModel : ViewModelBase
    {
        public StudentDetailPageViewModel(INavigationService navigationService = null, IPageDialogService dialogService = null, ISQLiteHelper sqLiteHelper = null) :
            base(navigationService, dialogService, sqLiteHelper)
        {
            ViewScoreBoardCommand = new DelegateCommand(ViewScoreBoardExecute);
            RemoveStudentCommand = new DelegateCommand(RemoveStudentExecute);
            EditStudentCommand = new DelegateCommand(EditStudentExecute);

            Instance = this;
        }

        #region Instance 

        public static StudentDetailPageViewModel Instance;

        #endregion

        #region private properties

        private Student _student;
        private string _className;
        private string _fullName;
        private string _doB;
        private string _gender;
        private string _email;
        private string _address;
        private string _avatar;
        private bool _isStudentInfo;
        private bool _isAddNewStudent;

        private const string Semseter1 = "Học kỳ 1";
        private const string Semseter2 = "Học kỳ 2";
        private const string AllYear = "Cả năm";

        #endregion

        #region public properties
        public string ClassName
        {
            get => _className;
            set => SetProperty(ref _className, value);
        }
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }
        public string DoB
        {
            get => _doB;
            set => SetProperty(ref _doB, value);
        }
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string Avatar
        {
            get => _avatar;
            set => SetProperty(ref _avatar, value);
        }
        public bool IsStudentInfo
        {
            get => _isStudentInfo;
            set => SetProperty(ref _isStudentInfo, value);
        }
        public bool IsAddNewStudent
        {
            get => _isAddNewStudent;
            set => SetProperty(ref _isAddNewStudent, value);
        }


        // Commands
        public ICommand ViewClassInfoCommand { get; set; }
        public ICommand ViewScoreBoardCommand { get; set; }
        public ICommand RemoveStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        #endregion

        #region override

        public override void OnNavigatedBackTo(NavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.NeedReload.ToString()))
                {
                    if ((bool)parameters[ParamKey.NeedReload.ToString()])
                        SetStudentInfo(Database.Get<Student>(s => s.Id == _student.Id));
                }
            }
        }

        public override void OnNavigatedNewTo(NavigationParameters parameters)
        {
            base.OnNavigatedNewTo(parameters);

            if (parameters != null)
            {
                if (parameters.ContainsKey(ParamKey.StudentInfo.ToString()))
                {
                    IsStudentInfo = true;
                    SetStudentInfo((Student)parameters[ParamKey.StudentInfo.ToString()]);
                }
                if (parameters.ContainsKey(ParamKey.DetailStudentPageType.ToString()))
                {
                    //SwitchPageMode((DetailStudentPageType)parameters[ParamKey.DetailStudentPageType.ToString()]);
                }
            }
        }

        private void SetStudentInfo(Student student)
        {
            _student = student;
            ClassName = student.ClassName;
            FullName = student.FullName;
            DoB = student.DoB.ToString("dd/MM/yyyy");
            Gender = student.GenderString;
            Email = student.Email;
            Address = student.Address;
            Avatar = student.Avatar;
        }

        #endregion


        #region Methods

        private void ViewClassInfoExecute()
        {
            var navParam = new NavigationParameters
            {
                { ParamKey.DetailClassPageType.ToString(), DetailClassPageType.ClassInfo }
            };
            //NavigationService.NavigateAsync(PageManager.DetailClassPage, navParam);
        }

        private async void ViewScoreBoardExecute()
        {
            // Choose semester
            var action = await Dialog.DisplayActionSheetAsync("Chọn học kỳ", "Hủy", null, Semseter1, Semseter2, AllYear);
            int semester;
            switch (action)
            {
                case Semseter1: semester = 1; break;
                case Semseter2: semester = 2; break;
                case AllYear: semester = 0; break;
                default: return;
            }

            // Navigate to PersonalScoreListPage
            var navParam = new NavigationParameters
            {
                { ParamKey.Semester.ToString(), semester },
                { ParamKey.StudentInfo.ToString(), _student }
            };
            await NavigationService.NavigateAsync("PersonalScoreListPage", navParam);
        }

        private void SwitchPageMode(DetailStudentPageType type)
        {
            switch (type)
            {
                case DetailStudentPageType.StudentInfo:
                    IsAddNewStudent = false;
                    IsStudentInfo = true;
                    break;

                case DetailStudentPageType.AddNewStudent:
                    IsAddNewStudent = true;
                    IsStudentInfo = false;
                    break;
            }
        }

        private async void RemoveStudentExecute()
        {
            bool isAccept = await Dialog.DisplayAlertAsync("Xóa học sinh", "Bạn có chắc muốn xóa học sinh này?", "Có", "Không");
            if (isAccept)
            {
                var user = Database.GetUser();
                ConfirmPasswordPopup.Instance.Show(user.Name);
            }
        }

        public async void OnReceiveConfirmPasswordResult(bool isCorrectPassword)
        {
            if (isCorrectPassword)
            {
                Database.Delete(_student);
                await Dialog.DisplayAlertAsync("Thông báo", "Xóa học sinh thành công", "OK");
                var param = new NavigationParameters()
                {
                    {ParamKey.NeedReload.ToString(), true},
                    {ParamKey.ClassId.ToString(), _student.ClassId }
                };
                await NavigationService.GoBackAsync(param);

                //string uri = PageManager.MultiplePage(new[]
                //{
                //    PageManager.HomePage, PageManager.NavigationPage, PageManager.ListClassesPage
                //});
                //await NavigationService.NavigateAsync(new Uri($"https://kienhht.com/{uri}"));
            }
            else
            {
                await Dialog.DisplayAlertAsync("Thông báo", "Mật khẩu không chính xác hoặc người dùng không tồn tại. Vui lòng thử lại", "OK");
            }

            
        }



        private void EditStudentExecute()
        {
            var navParam = new NavigationParameters
            {
                { ParamKey.AddNewStudentType.ToString(), AddNewStudentType.UpdateInfo },
                { ParamKey.StudentInfo.ToString(), _student }
            };
            NavigationService.NavigateAsync("AddNewStudentPage", navParam);
        }
        #endregion
    }
}
