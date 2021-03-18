using Reviewer.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Reviewer.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MonkeyCache.FileStore.Barrel.ApplicationId = "buildreviewer";

            DependencyService.Register<IStorageService, StorageService>();
            DependencyService.Register<IDataService, CosmosDataService>();
            DependencyService.Register<IIdentityService, IdentityService>();
            DependencyService.Register<IAPIService, WebAPIService>();

            var tabbedPage = new TabbedPage();

            tabbedPage.Children.Add(new NavigationPage(new BusinessListPage()) { Title = "Reviews" });
            tabbedPage.Children.Add(new NavigationPage(new AccountPage()) { Title = "Me" });

            MainPage = tabbedPage;
        }
    }
}
