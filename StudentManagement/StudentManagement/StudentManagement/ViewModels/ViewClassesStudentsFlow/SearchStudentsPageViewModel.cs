﻿using System;
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

namespace StudentManagement.ViewModels.ViewClassesStudentsFlow
{
    public class SearchStudentsPageViewModel : ViewModelBase
    {
        public SearchStudentsPageViewModel(INavigationService navigationService = null, IPageDialogService dialogService = null, ISQLiteHelper sqLiteHelper = null) : base(navigationService, dialogService, sqLiteHelper)
        {
            // Set values
            PageTitle = "Tìm học sinh";
            DoB = new DateTime(2001, 1, 1);
            ButtonName = "Tiếp theo";

            // Commands
            SearchCommand = new DelegateCommand(SearchExcute);

            ListClasses = new List<string>();
            ListClasses.Add("Tất cả");
            var classes = Database.GetList<Class>(i => i.Id >= 0);
            foreach (var item in classes)
            {
                ListClasses.Add(item.Name);
            }
        }

        

        #region private properties

        private string _fullName;
        private DateTime _doB;
        private string _gender;
        private string _email;
        private string _address;
        private string _className;
        private List<string> _listClasses;
        private AddNewStudentType _type;

        private string _buttonName;
        private Student _student;
        #endregion

        #region public properties
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }
        public DateTime DoB
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

        public string ClassName
        {
            get => _className;
            set => SetProperty(ref _className, value);
        }

        public List<string> ListClasses
        {
            get => _listClasses;
            set => SetProperty(ref _listClasses, value);
        }
        public string ButtonName
        {
            get => _buttonName;
            set => SetProperty(ref _buttonName, value);
        }

        public ICommand SearchCommand { get; }

        #endregion

        #region Search Excute

        private async void SearchExcute()
        {
            if (string.IsNullOrEmpty(FullName) &&
                string.IsNullOrEmpty(DoB.ToString()) &&
                string.IsNullOrEmpty(Address))
            {
                await Dialog.DisplayAlertAsync("Thông báo","Vui lòng nhập ít nhất một trường","Ok");

                return;

            }
            _student = new Student();
            _student.FullName = FullName;
            _student.ClassName = ClassName!="Tất cả"?ClassName:null;
            _student.DoB = DoB;
            //_student.Gender = Gender;
            switch (Gender)
            {
                case "Nam":
                {
                    _student.Gender = 1;
                    break;
                }
                case "Nữ":
                {
                    _student.Gender = 0;
                        break;
                }
                default:
                {
                    _student.Gender = 2;
                        break;
                }
            }
            _student.Email = Email;
            var param = new NavigationParameters()
            {
                {ParamKey.SearchResult.ToString(),true},
                {ParamKey.ExpectedResult.ToString(), _student }
            };

            await NavigationService.NavigateAsync("StudentsPage",param);
        }

        #endregion

    }
}
