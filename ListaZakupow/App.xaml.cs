using ListaZakupow.ViewModels;

namespace ListaZakupow
{
    public partial class App : Application
    {
        public static MainViewModel MainViewModel { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}