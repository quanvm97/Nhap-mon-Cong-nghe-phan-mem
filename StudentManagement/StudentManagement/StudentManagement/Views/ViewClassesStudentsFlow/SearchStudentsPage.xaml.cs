using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.ViewModels.ViewClassesStudentsFlow;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentManagement.Views.ViewClassesStudentsFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchStudentsPage : ContentPage
    {
        public SearchStudentsPage()
        {
            InitializeComponent();

            SetPickerValue();
        }

        private void SetPickerValue()
        {
            var vm = BindingContext as SearchStudentsPageViewModel;

            if (vm != null)
            {
                PickerClass.ItemsSource = vm.ListClasses;
                PickerClass.SelectedIndex = 0;
            }


            PickerNamHoc.ItemsSource = new List<string>()
            {
                "Tất cả",
                "2014-2015",
                "2015-2016",
                "2016-2017",
                "2017-2018"
            };

            PickerNamHoc.SelectedIndex = 0;
        }
    }
}