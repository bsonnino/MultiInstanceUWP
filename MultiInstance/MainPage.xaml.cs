using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MultiInstance
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e?.Parameter;
            MainTxt.Text = param?.ToString() ?? "";
            base.OnNavigatedTo(e);
        }
    }
}
