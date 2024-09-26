using Android.Gms.Auth.Api.SignIn;
using Firebase.Auth;

namespace FirebaseMauiApp;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
#if ANDROID
        var mainActivity = (MainActivity)Platform.CurrentActivity;
        mainActivity.SignOutWithGoogle();
#endif
    }
}