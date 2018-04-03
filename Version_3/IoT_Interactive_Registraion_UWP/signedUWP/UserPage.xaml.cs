using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        public int Delay_Param ;
        private ImageSource mainImage;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private MediaElement media;
        private int counter = 1,counter3=3,counter2=1;

        DispatcherTimer Timer1 = new DispatcherTimer();
        DispatcherTimer Timer2 = new DispatcherTimer();
        DispatcherTimer Timer3 = new DispatcherTimer();
        DispatcherTimer Timer4 = new DispatcherTimer();

        public UserPage()
        {
            this.InitializeComponent();
            CalendarDatePicker arrivalCalendarDatePicker = new CalendarDatePicker();
            Timer1.Tick += Timer_Tick;
            Timer1.Interval = new TimeSpan(0, 0, 1);
            Timer1.Start();
      
        }


        private void Timer_Tick(object sender, object e)
        {
            Timer.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }


        private void timer1_Tick(object sender, object e)
        {

          
            Debug.WriteLine("i counter : "+counter);
            if (counter == 0)
            {
                line1.Visibility = Visibility.Visible;
                counter = 1;
            }
            else 
            {
                counter--;
                line1.Visibility = Visibility.Collapsed;
            }
          
        }
        private void timer2_Tick(object sender, object e)
        {


            Debug.WriteLine("o counter : " + counter2);
            if (counter2 == 0)
            {
                line2.Visibility = Visibility.Visible;
                counter2 = 1;
            }
            else
            {
                counter2--;
                line2.Visibility = Visibility.Collapsed;
            }

        }


        private void TextBox1_GetFocus(object sender, RoutedEventArgs e)
        {
            // Set the TexBox focus background color
            textBox1.Background = (SolidColorBrush)Resources["Green1"];
        }
        private void TextBox2_GetFocus(object sender, RoutedEventArgs e)
        {
            // Set the TexBox focus background color
            textBox2.Background = (SolidColorBrush)Resources["Green1"];
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dateToFormat = System.DateTime.Now;

            var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("month day");
            var timeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("hour minute");

            var date = dateFormatter.Format(dateToFormat);
            var time = timeFormatter.Format(dateToFormat);

            string output = string.Format(date) + " | " + string.Format(time);

            textBox2.IsEnabled = false;
            textBox2.Visibility = Visibility.Collapsed;
            Check1.Visibility = Visibility.Collapsed;
            Check2.Visibility = Visibility.Collapsed;
            textBox1.IsEnabled = true;
            textBox1.Visibility = Visibility.Visible;
            textBox1.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            textBox1.Background = (SolidColorBrush)Resources["Green1"];
            TitleName.Text = "IoT Innovation Center";
    
            Delay_Param = 5;
            await CountUserList();
     
            //TextBlock11.Text = "1. 請以Barcode Reader掃描報到單您的條碼.\n2. 請隨機領取UWB Tag一張，並以NFC Reader感應.\n3. 配對開通完成.";
            if (e.Parameter is string)
            {
                textBox1.Focus(FocusState.Programmatic);
            }
        }


     

        private async void button5_Click(object sender, RoutedEventArgs e)
        {
            Delay_Param = 5;
            Debug.WriteLine(Delay_Param);
            Image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT111.png"));
            Image2.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT22.png"));
            Image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT33.png"));
            textBox1.Focus(FocusState.Programmatic);

        }

        private async void button10_Click(object sender, RoutedEventArgs e)
        {
            Delay_Param = 10;
            Debug.WriteLine(Delay_Param);
            Image2.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT222.png"));
            Image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT11.png"));
            Image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT33.png"));
            textBox1.Focus(FocusState.Programmatic);
        }
        private async void button15_Click(object sender, RoutedEventArgs e)
        {
            Delay_Param = 15;
            Debug.WriteLine(Delay_Param);
            Image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT333.png"));
            Image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT11.png"));
            Image2.Source = new BitmapImage(new Uri("ms-appx:///Assets/IoT22.png"));
            textBox1.Focus(FocusState.Programmatic);
        }

   

        private async void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                EnterButton.Background = (SolidColorBrush)Resources["BlueColor"];
                textBox1.Visibility = Visibility.Collapsed;
                TextBlock22.Visibility = Visibility.Visible;
                BarcodeId = textBox1.Text;
                TextBlock22.Text = textBox1.Text;
                Timer3.Tick += timer2_Tick;
                Timer3.Interval = new TimeSpan(0, 0, 1);
                Timer3.Start();
                Timer2.Stop();
              
                textBox2.IsEnabled = true;
                textBox2.Visibility = Visibility.Visible;
                textBox2.Focus(FocusState.Programmatic);
                EnterButton1.IsEnabled = true;
                Check1.Visibility = Visibility.Visible;
                Num111.Visibility = Visibility.Collapsed;
                textBox1.Visibility = Visibility.Collapsed;
                line1.Visibility = Visibility.Collapsed;
            }
            else
            {
                EnterButton.Background = (SolidColorBrush)Resources["LightGrey"];
                TextBlock22.Text = textBox1.Text;
                Check1.Visibility = Visibility.Collapsed;
                textBox1.Visibility = Visibility.Visible;
                TextBlock22.Visibility = Visibility.Collapsed;
            }
        }

        private async void OnKeyDownHandler1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                EnterButton1.Background = (SolidColorBrush)Resources["BlueColor"];
                ScanTagId = textBox2.Text;
                //TextBlock2.Text = ScanTagId;
                Check2.Visibility = Visibility.Visible;
                Num222.Visibility = Visibility.Collapsed;
                textBox2.Visibility = Visibility.Collapsed;
                await PostAsync(BarcodeId, ScanTagId);
                await GetUsersDetailAsync(BarcodeId);
                await CountUserList();
                Timer3.Stop();
            }
            else
            {
                EnterButton1.Background = (SolidColorBrush)Resources["LightGrey"];
                //TextBlock1.Text = textBox1.Text;
                Check2.Visibility = Visibility.Collapsed;
            }
        }


        private async Task CountUserList()
        {

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";


            ArrivedUser = 0;
            TotalUser = 0;

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = await client.GetAsync(baseAPIUrl + "api/Users/");
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl2 + "api/Users/");
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(httpResponseBody.ToString());
                    if (httpResponseBody.ToString() != "[]") {
                        Users user = new Users();                    
                        UserList u = DataHelper.GetUsers(httpResponseBody);
                        UserList nu = new UserList();                     
                        TotalUser = u.Count();
                        for (int i = 0; i < u.Count; i++)
                        {
                            if (u[i].Arrived == "Yes")
                            {

                                //int hour = u[i].UpdateTime.Hour;
                                //int minute = u[i].UpdateTime.Minute;

                                //int newtime = int.Parse(hour.ToString() + ":" + hour.ToString());             
                                string time1 = u[i].UpdateTime.Substring(11, 5);
                                //Debug.WriteLine(u[i].UpdateTime.Substring(11,5));

                                u[i].UpdateTime = time1;
                                Debug.WriteLine(u[i].UpdateTime);
                                ArrivedUser += 1;
                                nu.Add(u[i]);
                            }
                        }
                        TextBlock5.Text = TotalUser.ToString();
                        TextBlock6.Text = ArrivedUser.ToString();
                        if (nu != null) InventoryList.ItemsSource = nu;
                        //httpResponseBody.Replace("[", "").Replace("]", "");
                        Debug.WriteLine(httpResponseBody);
                    }

                }
            }
            Debug.WriteLine("Delay : " + Delay_Param);
            await Task.Delay(Delay_Param*1000);
            Timer2.Tick += timer1_Tick;
            Timer2.Interval = new TimeSpan(0, 0, 1);
            Timer2.Start();
            Check1.Visibility = Visibility.Collapsed;
            Check2.Visibility = Visibility.Collapsed;
            Check3.Visibility = Visibility.Collapsed;
            Num111.Visibility = Visibility.Visible;
            Num222.Visibility = Visibility.Visible;
            line2.Visibility = Visibility.Collapsed;
            TextBlock1.Text = ""; TextBlock2.Text = "";            
            textBox1.Text = ""; textBox2.Text = "";
            textBox2.IsEnabled = false;
            textBox1.Visibility = Visibility.Visible;
            textBox2.Visibility = Visibility.Collapsed;
            textBox1.Focus(FocusState.Programmatic);
            EnterButton1.IsEnabled = false;
            EnterButton1.Background = (SolidColorBrush)Resources["LightGrey"];
            EnterButton.Background = (SolidColorBrush)Resources["LightGrey"];
            Num333.Visibility = Visibility.Visible;
            TextBlock10.Visibility = Visibility.Visible;
            TextBlock30.Visibility = Visibility.Visible;
            TextBlock22.Text = ""; 
            TextBlock3.Text = "";
            TextBlock24.Text = "";
            TextBlock26.Text = "";


        }

        private async Task GetUsersDetailAsync(String id)
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl2 +"api/Users/" + id);
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();
                    string body = httpResponseBody.Trim(new Char[] { '[', ']' });
                    Users user = new Users();

                    Debug.WriteLine(body);
                    try
                    {
                        var dyn = JsonConvert.DeserializeObject<JObject>(body);
                        JProperty propName = dyn.Properties().FirstOrDefault(i => i.Name == "Name");
                        JProperty propSerialnum = dyn.Properties().FirstOrDefault(i => i.Name == "TagId");
                        JProperty propTime = dyn.Properties().FirstOrDefault(i => i.Name == "UpdateTime");
                        if ( propName != null)
                        {
                            mediaPlayer = new MediaPlayer();
                            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/crrect_answer3.mp3"));
                            mediaPlayer.Play();
                            string name = propName.Value.ToString();
                            string serialnum = propSerialnum.Value.ToString();
                            TextBlock3.Text = name;
                            TextBlock26.Text = "Welcome!"; 
                            TextBlock24.Text = "Enroll Succeed!" ;
                            TextBlock10.Visibility = Visibility.Collapsed;
                            TextBlock30.Visibility = Visibility.Collapsed;
                            Num333.Visibility = Visibility.Collapsed;
                            Check3.Visibility = Visibility.Visible;

                            await UpdateUwbTag(serialnum, name);
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

        private async Task UpdateUwbTag(string TagSerialnum, string name)
        {
            Debug.WriteLine("Update uwb tag .....");
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://uwbdemo3372964322000.eastasia.cloudapp.azure.com:8090/ms-rtls/tags/tag_id/";

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Uwb Client!!");
                client.BaseAddress = new Uri(baseAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(baseAPIUrl + TagSerialnum + "/modify?name=" + name);
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Update UWB tag Success!!!");
                }
            }
        }


        public async Task PostAsync(string UserId, string ScanId)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            var json = "{\"NFCid\":" + ScanId + "}";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl2 + "api/Users/" + UserId, content).Result;
            
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

        #region Drag and drop handling code       
        private void InventoryList_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private void InventoryList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (InventoryList.SelectedItem != null)
            {
                var users = (Users)InventoryList.SelectedItem;
                e.Data.SetText(users.Id.ToString());
                //e.Data.RequestedOperation = DataPackageOperation.Copy;
                Debug.WriteLine(users.Name.ToString());

            }
        }
        public Users old_users;


        private void InventoryList_ClickItem(object sender, ItemClickEventArgs e)
        {

            if (InventoryList.SelectedItem != null)
            {

                var users = (Users)InventoryList.SelectedItem;
                // Get the list view clicked item text
                if (users != old_users)
                {
                    string clickedItemText = "Hi " + users.Name + " \n Please scan your Tag!";

                    // Initialize a new message dialog
                    MessageDialog dialog = new MessageDialog("Clicked : " + clickedItemText);

                    // Finally, display the clicked item text on dialog
                    //ScanningWindow(users);

                    old_users = users;
                }


            }
        }

        #endregion


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

        private void TextBlock8_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock7_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock3_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
