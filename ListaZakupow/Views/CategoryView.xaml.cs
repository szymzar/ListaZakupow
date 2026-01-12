using ListaZakupow.Models;

namespace ListaZakupow.Views;

public partial class CategoryView : ContentView
{
	public CategoryView()
	{
		InitializeComponent();
	}

    private void ToggleExpansion_Tapped(object sender, System.EventArgs e)
    {
        if (BindingContext is Category category)
        {
            category.IsExpanded = !category.IsExpanded;
        }
    }
}
