using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Firebase;
using Firebase.Auth;

namespace FirebaseMauiApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        internal static readonly string Channel_ID = "TestChannel";
        internal static readonly int NotificationID = 101;
        GoogleSignInOptions googleSigninOption;
        GoogleSignInClient googleSignInClient;
        FirebaseAuth mAuth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FirebaseApp.InitializeApp(this);
            mAuth = FirebaseAuth.Instance;
            googleSigninOption = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
              .RequestIdToken(GetString(Resource.String.default_web_client_id))
              .RequestEmail()
              .Build();
            var s = Resource.String.default_web_client_id;
            googleSignInClient = GoogleSignIn.GetClient(this, googleSigninOption);

            CreateNotificationChannel();

        }
        public void SignInWithGoogle()
        {
            var signInIntent = googleSignInClient.SignInIntent;
            StartActivityForResult(signInIntent, 1);
        }
        public async void SignOutWithGoogle()
        {
            FirebaseAuth.Instance.SignOut();
            googleSignInClient.SignOut();
            await Shell.Current.GoToAsync("..");
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
            {
                var task = GoogleSignIn.GetSignedInAccountFromIntent(data);
                HandleSignInResult(task);
            }
        }
        private async void HandleSignInResult(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                GoogleSignInAccount account = (GoogleSignInAccount)task.Result;
                FirebaseAuthWithGoogle(account);
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                Console.WriteLine("Google Sign-In failed.");
            }
        }
      
        private void FirebaseAuthWithGoogle(GoogleSignInAccount acct)
        {
            var credential = GoogleAuthProvider.GetCredential(acct.IdToken, null);
            mAuth.SignInWithCredential(credential)
                .AddOnCompleteListener(this, (Android.Gms.Tasks.IOnCompleteListener)this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    if (key == "NavigationID")
                    {
                        string idValue = intent.Extras.GetString(key);
                        if (Preferences.ContainsKey("NavigationID"))
                            Preferences.Remove("NavigationID");

                        Preferences.Set("NavigationID", idValue);
                    }
                }
            }
        }

        private void CreateNotificationChannel()
        {
            if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
            {
                var channel = new NotificationChannel(Channel_ID, "Test Notfication Channel", NotificationImportance.Default);

                var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
                notificaitonManager.CreateNotificationChannel(channel);

            }
        }
    }
}
