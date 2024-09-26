


namespace FirebaseMauiApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }
 
       
        private void OnCounterClicked(object sender, EventArgs e)
        {
#if ANDROID
            var mainActivity = (MainActivity)Platform.CurrentActivity;
            mainActivity.SignInWithGoogle();
#endif
        }
    }

}
