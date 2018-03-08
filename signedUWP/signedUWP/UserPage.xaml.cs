using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace signedUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        //public UserList Users { get; set; } // products currently displayed on this page

        public string ScanTagId = null;
        public string BarcodeId = null;
        public int TotalUser = 0;
        public int ArrivedUser = 0;
        private ImageSource mainImage;

        public UserPage()
        {
            this.InitializeComponent();
            CalendarDatePicker arrivalCalendarDatePicker = new CalendarDatePicker();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = false;
            Check1.Visibility = Visibility.Collapsed;
            Check2.Visibility = Visibility.Collapsed;
            textBox1.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            textBox1.Select(0, 0);
          
            //TextBlock11.Text = "1. 請以Barcode Reader掃描報到單您的條碼.\n2. 請隨機領取UWB Tag一張，並以NFC Reader感應.\n3. 配對開通完成.";
            if (e.Parameter is string)
            {
                textBox1.Focus(FocusState.Programmatic);
            }
        }


        private async void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                EnterButton.Background = (SolidColorBrush)Resources["BlueColor"];
                
                BarcodeId = textBox1.Text;
                TextBlock1.Text = textBox1.Text;
                textBox2.IsEnabled = true;
                EnterButton1.IsEnabled = true;
                Check1.Visibility = Visibility.Visible;
                textBox2.Focus(FocusState.Programmatic);
            }
            else
            {
                EnterButton.Background = (SolidColorBrush)Resources["LightGrey"];
                TextBlock1.Text = textBox1.Text;
                Check1.Visibility = Visibility.Collapsed;
            }
        }

        private async void OnKeyDownHandler1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                EnterButton1.Background = (SolidColorBrush)Resources["BlueColor"];
                ScanTagId = textBox2.Text;
                TextBlock2.Text = ScanTagId;
                Check2.Visibility = Visibility.Visible;
                await PostAsync(BarcodeId, ScanTagId);
                await GetUsersDetailAsync(BarcodeId);
               
            }
            else
            {
                EnterButton1.Background = (SolidColorBrush)Resources["LightGrey"];
                TextBlock1.Text = textBox1.Text;
                Check2.Visibility = Visibility.Collapsed;
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
       
        }
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {

        }
        private async Task GetUsersDetailAsync(String id)
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl +"api/Users/" + id);
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();

                    //TextBlock2.Text = "Result: "; //+ httpResponseBody;
                  
                    //var outObject = JsonConvert.DeserializeObject<Users>(s);
                    string body = httpResponseBody.Trim(new Char[] { '[', ']' });
                    Users user = new Users();
                    //httpResponseBody.Replace("[", "").Replace("]", "");

                    Debug.WriteLine(body);
                    try
                    {
                        var dyn = JsonConvert.DeserializeObject<JObject>(body);
                        JProperty propName = dyn.Properties().FirstOrDefault(i => i.Name == "Name");
                        JProperty propTime = dyn.Properties().FirstOrDefault(i => i.Name == "UpdateTime");
                        if ( propName != null)
                        {
                            
                            string name = propName.Value.ToString();
                            TextBlock3.Text = "Hello, " + name + " ! Nice to meet you\n Welcome to IoT center.";
                            //int age = int.Parse(propTime.Value.ToString());
                            //int en = Int16.Parse(propEnroll.Value.ToString());
                            //Debug.WriteLine(en);
                            //TextBlock6.Text = "已簽到";
                            //TextBlock4.Text = propName.Value.ToString();
                            //TextBlock5.Text = propTime.Value.ToString();
                            
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    //var result  =JsonConvert.DeserializeObject<Users>(httpResponseBody);
                    //user.UserID = result.UserID;
                    //user.UserName = result.UserName;
                  

                }
            }

            //Users = DataHelper.GetUsers((App.Current as App).ConnectionString);
            /*if (Users is UserList)
            {
                // Store list of all products available at App level
                (App.Current as App).Users = Users;
                InventoryList.ItemsSource = Users;
                //Categories = DataHelper.GetCategories((App.Current as App).ConnectionString);
                //Categories.Insert(0, new Category(0, "<Show all categories>"));
            }
            else
            {
                await new MessageDialog("Unable to connect to SQL Server! Check connection string in Settings.").ShowAsync();
            }*/
        }

        public ImageSource MainImage
        {
            get { return mainImage; }
            set
            {
                if (mainImage != value)
                {
                    mainImage = value;
                  
                }
            }
        }
        public void Post(string id)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);
            if (ScanTagId != null)
            {
                var json = "{\"TagId\":"+ ScanTagId + "}";
                StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                var result = httpClient.PutAsync(baseAPIUrl + "api/Users/" + id, content).Result;
            }
        }


        public async Task PostAsync(string UserId, string ScanId)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);

            var json = "{\"TagId\":" + ScanId + "}";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl + "api/Users/" + UserId, content).Result;
            
        }

        public async Task GetNewOrder()
        {
            // Create New Orders window on new thread
            var newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                // frame.Navigate(typeof(OrderPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                // Save Id of Orders view
                newViewId = ApplicationView.GetForCurrentView().Id;
            });

            // Show the Orders window as standalone window
            if (newViewId != 0)
            {
                bool isShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
                if (isShown)
                {
                    await ApplicationViewSwitcher.SwitchAsync(newViewId);
                }
            }
        }
 

        private void toppingsCheckbox_Click(object sender, RoutedEventArgs e)
        {
            string selectedToppingsText = string.Empty;
           // CheckBox[] checkboxes = new CheckBox[] { pepperoniCheckbox, beefCheckbox,
                                            // mushroomsCheckbox, onionsCheckbox };
           /* foreach (CheckBox c in checkboxes)
            {
                if (c.IsChecked == true)
                {
                    if (selectedToppingsText.Length > 1)
                    {
                        selectedToppingsText += ", ";
                    }
                    selectedToppingsText += c.Content;
                }
            }*/
           // toppingsList.Text = selectedToppingsText;
        }
        public async Task ScanningWindow(Users u)
        {
            var currentAV = ApplicationView.GetForCurrentView();
            var newAV = CoreApplication.CreateNewView();
            await newAV.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal, async () =>
                            {
                                var newWindow = Window.Current;
                                var newAppView = ApplicationView.GetForCurrentView();
                                newAppView.Title = "New window";

                                var frame = new Frame();
                                frame.Navigate(typeof(ScanPage), u);
                                newWindow.Content = frame;
                                newWindow.Activate();
                                
                                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                    newAppView.Id,
                                    ViewSizePreference.UseMinimum,
                                    currentAV.Id,
                                    ViewSizePreference.UseMinimum);
                            });
        }


    }
}
