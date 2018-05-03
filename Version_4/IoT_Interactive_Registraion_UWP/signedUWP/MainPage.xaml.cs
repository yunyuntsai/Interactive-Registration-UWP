using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using System.Reflection;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace signedUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MainPage : Page
    {

        private ApplicationView currentView;
  

        public MainPage()
        {
           
            this.InitializeComponent();

            (App.Current as App).MainPageInstance = this;

            // This is needed to ensure secondary windows are all closed when this one is
            currentView = ApplicationView.GetForCurrentView();
            currentView.Consolidated += CurrentView_Consolidated;
            //ContentFrame.Navigate(typeof(UserPage));
            ContentFrame.Navigate(typeof(RegisterPage));
            var dateToFormat = System.DateTime.Now;

            var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("month day");
            var timeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("hour minute");

            var date = dateFormatter.Format(dateToFormat);
            var time = timeFormatter.Format(dateToFormat);

            string output = string.Format(date) + " | " + string.Format(time);

            NavView.Header = "    物聯網創新中心                                                                                             " + output;
            
            NavView.Visibility = Visibility.Visible;
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            
            currentView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            currentView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            
        }

        private void CurrentView_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            // Clean up code to shut down secondary windows as this one closes
            Application.Current.Exit();
        }

        private void MoreInfoButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var version = typeof(App).GetTypeInfo().Assembly.GetName().Version;
            NavView.Header = "   About UWP Access SQL DB Demo " + version.ToString();
            ContentFrame.Navigate(typeof(AboutPage));
        }

        private async void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args != null && args.IsSettingsInvoked)
            {
                NavView.Header = "   Settings";
                //ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                object item = "Users";
                if (args != null)
                {
                    item = args.InvokedItem;
                }
                switch (item)
                {

                    case "Registration":
                        ContentFrame.Navigate(typeof(RegisterPage));
                        break;


                    case "Users":
                        var dateToFormat = System.DateTime.Now;

                        var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("month day");
                        var timeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("hour minute");

                        var date = dateFormatter.Format(dateToFormat);
                        var time = timeFormatter.Format(dateToFormat);

                        string output = string.Format(date) + " | " + string.Format(time);

                        NavView.Header = "    物聯網創新中心                                " + output;
                        
                        ContentFrame.Navigate(typeof(UserPage));
                        break;

                    case "Lists":
                        NavView.Header = "    物聯網創新中心";
                        
                        ContentFrame.Navigate(typeof(OrderPage));
                        break;
                    /*case "import CSV":
                        NavView.Header = "   Export to CSV";
                        ContentFrame.Navigate(typeof(ExportPage));
                        break;*/
                    /*case "Chart":
                        NavView.Header = "   Sales by Category";
                        //ContentFrame.Navigate(typeof(ChartPage));
                        break;

                      case "New order":
                        object o = ContentFrame.Content;
                        if (o is ProductsPage)
                        {
                            await (o as ProductsPage).GetNewOrder();
                        }
                        else
                        {
                            NavView.Header = "   Northwind Traders Products";
                           // ContentFrame.Navigate(typeof(ProductsPage), "new");
                        }
                        break;

                    case "Export to CSV":
                        NavView.Header = "   Export to CSV";
                        //ContentFrame.Navigate(typeof(ExportPage));
                        break;*/
                }
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            //NavView.MenuItems.Add(new NavigationViewItem()
            //{ Content = "Users", Icon = new SymbolIcon(Symbol.Shop), Tag = "Users", AccessKey = "P" });
            //NavView.MenuItems.Add(new NavigationViewItem()
            //{ Content = "Orders", Icon = new SymbolIcon(Symbol.ViewAll), Tag = "orders", AccessKey = "O" });
            //NavView.MenuItems.Add(new NavigationViewItem()
            //{ Content = "import CSV", Icon = new SymbolIcon(Symbol.Import), Tag = "import", AccessKey = "X" });

            /*NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "Chart", Icon = new SymbolIcon(Symbol.ThreeBars), Tag = "chart", AccessKey = "C" });
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "Export to CSV", Icon = new SymbolIcon(Symbol.Download), Tag = "export", AccessKey = "X" });
            NavView.MenuItems.Add(new NavigationViewItem()
            { Content = "New order", Icon = new SymbolIcon(Symbol.Add), Tag = "order", AccessKey = "N" });
            NavView_ItemInvoked(NavView, null);*/
        }

        private async void button_Click(object sender, RoutedEventArgs e)

        {

            MediaElement mediaElement = new MediaElement();

            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Hello, World!");

            mediaElement.SetSource(stream, stream.ContentType);

            mediaElement.Play();

        }
    }
}
