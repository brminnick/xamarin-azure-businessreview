using Foundation;
using Microsoft.Identity.Client;
using Reviewer.Core;
using UIKit;

namespace Reviewer.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            Xamarin.Forms.DependencyService.Register<IVideoConversion, VideoConverter>();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options) => AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
    }
}
