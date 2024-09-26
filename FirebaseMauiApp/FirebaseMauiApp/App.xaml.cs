namespace FirebaseMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            MainPage = new AppShell();
        }
   
    }
}
