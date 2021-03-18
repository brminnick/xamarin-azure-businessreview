using Android.App;
using Android.Content;
using Android.OS;

namespace Reviewer.Droid
{
    [Activity(Label = "Reviewer", Icon = "@mipmap/ic_eightbit", Theme = "@style/SplashTheme", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(this, typeof(MainActivity)));
            Finish();
        }
    }
}
