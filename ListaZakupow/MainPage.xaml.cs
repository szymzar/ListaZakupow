using ListaZakupow.ViewModels;

namespace ListaZakupow
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (BindingContext is MainViewModel vm)
            {
                App.MainViewModel = vm;
            }
        }

        private void AddProduct_Clicked(object sender, System.EventArgs e)
        {
            if (BindingContext is MainViewModel vm)
            {
                vm.AddProduct();
            }
        }

        private async void AddCategory_Clicked(object sender, System.EventArgs e)
        {
            if (BindingContext is MainViewModel vm)
            {
                var success = vm.AddCategory();
                if (!success)
                {
                    await DisplayAlert("Błąd", "Kategoria o tej nazwie już istnieje lub nazwa jest pusta.", "OK");
                }
            }
        }
    }
}
