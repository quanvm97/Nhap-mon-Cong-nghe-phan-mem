using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Models;
using StudentManagement.ViewModels.ViewClassesStudentsFlow;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentManagement.Views.ViewClassesStudentsFlow
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        public StudentsPage()
        {
            InitializeComponent();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var student = (Student)e.Item;
            ListViewStudents.SelectedItem = null;
            var vm = (StudentsPageViewModel)BindingContext;
            vm.StudentItemTapped(student);
        }
    }
}