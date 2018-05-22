using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class C : ContentPage
    {
        public C()
        {
            InitializeComponent();
        }

        private async void OnTapped_BackCommand(object sender, EventArgs e)
        {

            //await DeviceExtension.BeginInvokeOnMainThreadAsync(async () => { await Navigation.PopToRootAsync(); });
            await Navigation.PopToRootAsync();
        }
    }
}
