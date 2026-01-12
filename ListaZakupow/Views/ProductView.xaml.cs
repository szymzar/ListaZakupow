using ListaZakupow.Models;

namespace ListaZakupow.Views;

public partial class ProductView : ContentView
{
	public ProductView()
	{
		InitializeComponent();
	}

    private void Decrease_Clicked(object sender, System.EventArgs e)
    {
        if (BindingContext is Product product && App.MainViewModel != null)
        {
            App.MainViewModel.DecreaseQuantity(product);
        }
    }

    private void Increase_Clicked(object sender, System.EventArgs e)
    {
        if (BindingContext is Product product && App.MainViewModel != null)
        {
            App.MainViewModel.IncreaseQuantity(product);
        }
    }

    private void Delete_Clicked(object sender, System.EventArgs e)
    {
        if (BindingContext is Product product && App.MainViewModel != null)
        {
            App.MainViewModel.DeleteProduct(product);
        }
    }
}