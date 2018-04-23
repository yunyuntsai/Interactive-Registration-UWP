﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace signedUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddVisitorPage : Page
    {
        public string vCompany = null;
        public string vName = null;
        public int TotalUser = 0;
        public int ArrivedUser = 0;
        public int Delay_Param;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private MediaPlayer mediaPlayer1 = new MediaPlayer();
        private MediaElement media;
        private int counter = 1, counter3 = 3, counter2 = 1;
        private MediaCapture _mediaCapture;
        public string registerName = "";
        public string registorVisitorId = "";
        private int countdown;
        DispatcherTimer Timer1 = new DispatcherTimer();
        DispatcherTimer Timer2 = new DispatcherTimer();
        DispatcherTimer Timer3 = new DispatcherTimer();
        DispatcherTimer Timer4 = new DispatcherTimer();
        private DispatcherTimer count_timer;
        private bool _mirroringPreview;
        private CameraRotationHelper _rotationHelper;
        private bool _externalCamera;
        StorageFile userImageFile;
        BitmapImage userImage;
        private string userImagePath;
        private Windows.ApplicationModel.Contacts.Contact _currentContact;


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public AddVisitorPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dateToFormat = System.DateTime.Now;

            var dateFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("month day");
            var timeFormatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("hour minute");

            var date = dateFormatter.Format(dateToFormat);
            var time = timeFormatter.Format(dateToFormat);

            string output = string.Format(date) + " | " + string.Format(time);

            BackToDefaultStep();
            Delay_Param = 5;
            await CountUserList();

            //TextBlock11.Text = "1. 請以Barcode Reader掃描報到單您的條碼.\n2. 請隨機領取UWB Tag一張，並以NFC Reader感應.\n3. 配對開通完成.";
            if (e.Parameter is string)
            {
                //textBox1.Focus(FocusState.Programmatic);
            }
        }

        private async void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                VisitorName.Visibility = Visibility.Collapsed;
                vName = VisitorName.Text;
                VisitorNameText.Text = vName;
                VisitorNameText.Visibility = Visibility.Visible;

                //var userInfo = await GetUsersDetailInfoAsync();

                step1Circle.Fill = (SolidColorBrush)Resources["successStepBackground"];
                step1Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.Check;

                //registerName = userInfo.registerName;
                displayName.Text = vName;

                visitorCompany.Visibility = Visibility.Visible;

                step1ColorStoryboard.Stop();
                step2ColorStoryboard.Begin();

                visitorCompany.Focus(FocusState.Programmatic);
            }
            else
            {
                //EnterButton.Background = (SolidColorBrush)Resources["LightGrey"];
                //TextBlock22.Text = textBox1.Text;
                //Check1.Visibility = Visibility.Collapsed;
                //textBox1.Visibility = Visibility.Visible;
                //TextBlock22.Visibility = Visibility.Collapsed;
            }
        }

        private async void companyKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                visitorCompany.Visibility = Visibility.Collapsed;
                vCompany = visitorCompany.Text;
                visitorCompanyText.Text = vCompany;
                visitorCompanyText.Visibility = Visibility.Visible;

                step2Circle.Fill = (SolidColorBrush)Resources["successStepBackground"];
                step2Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.Check;
                step2ColorStoryboard.Stop();

                step3ColorStoryboard.Begin();
                await PostAsync(vName, vCompany);
                step3Circle.Fill = (SolidColorBrush)Resources["successStepBackground"];
                step3Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.Check;

                popUpDisplayText.Text = "Add " + vName +" succeed";
                PopUpWidget.IsOpen = true;

                //string name = propName.Value.ToString();
                //string serialnum = propSerialnum.Value.ToString();

                /*MediaElement mediaElement = new MediaElement();
                var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
                Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Welcome " + registerName);
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();*/



                await CountUserList();
                await System.Threading.Tasks.Task.Delay(Delay_Param * 1000);


                BackToDefaultStep();
                //displayName.Text = userInfo.registerName;
                //await CountUserList();
                //Timer3.Stop();
            }
            else
            {
                //EnterButton1.Background = (SolidColorBrush)Resources["LightGrey"];
                //TextBlock1.Text = textBox1.Text;
                //Check2.Visibility = Visibility.Collapsed;
            }
        }

        private async Task UpdateUwbTag(string TagSerialnum, string url)
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

                HttpResponseMessage response = await client.GetAsync(baseAPIUrl + TagSerialnum + "/modify?name=" + registerName);
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Update UWB tag Success!!!");
                }
            }
        }

        public async Task PostAsync(string Name, string Company)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            UserList u = await CountUserList();
            int Tatalnum = u.Count();
            int VisitorNewId = u.Last().Id + 1;
            var json = "{\"ID\":" + VisitorNewId + ",\"VisitorName\":\"" + Name + "\",\"VisitorCompany\":\"" + Company + "\",\"Arrived\":" + "\"No\"" + "}";
            Debug.WriteLine(json);
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl2 + "api/Users", content).Result;

        }

        public async Task PostPhotoAsync(string url)
        {
            Debug.WriteLine("Post Photo!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            var json = "{\"PhotoId\":" + registorVisitorId + ",\"PhotoUrl\":\"" + url + "\",\"PhotoName\":\"" + registerName + "\",\"VisitorId\":" + registorVisitorId + "}";
            Debug.WriteLine(json);
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(baseAPIUrl2 + "api/Photo", content).Result;

        }

        private async Task<dynamic> GetUsersDetailInfoAsync(String id)
        {

            string registerName = null;

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(baseAPIUrl2 + "api/Users/" + id);
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
                        JProperty propId = dyn.Properties().FirstOrDefault(i => i.Name == "Id");
                        JProperty propName = dyn.Properties().FirstOrDefault(i => i.Name == "Name");
                        JProperty propSerialnum = dyn.Properties().FirstOrDefault(i => i.Name == "TagId");
                        JProperty propTime = dyn.Properties().FirstOrDefault(i => i.Name == "UpdateTime");
                        if (propName != null)
                        {

                            dynamic userInfo = new JObject();

                            userInfo.registerName = propName.Value.ToString();
                            userInfo.serialnum = propSerialnum.Value.ToString();
                            userInfo.time = propTime.Value.ToString();
                            userInfo.ID = propId.Value.ToString();

                            registerName = propName.Value.ToString();
                            registorVisitorId = propId.Value.ToString();
                            return userInfo;
                            //Debug.WriteLine(regiserName);

                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        return "error";
                    }


                }
                return "error";
            }


        }

        private async Task<UserList> CountUserList()
        {

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";
            UserList u;

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
                    if (httpResponseBody.ToString() != "[]")
                    {
                        Users user = new Users();
                        u = DataHelper.GetUsers(httpResponseBody);
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
                        totalDisplayText.Text = TotalUser.ToString();
                        arrivedDisplayText.Text = ArrivedUser.ToString();
                        if (nu != null) InventoryList.ItemsSource = nu;
                        //httpResponseBody.Replace("[", "").Replace("]", "");
                        Debug.WriteLine(httpResponseBody);
                        return u;
                    }
                    else
                    {
                       
                    }

                }
        
            }
            return null;
        }

        public Windows.ApplicationModel.Contacts.Contact CurrentContact
        {
            get => _currentContact;
            set
            {
                _currentContact = value;
                PropertyChanged?.Invoke(this,
                    new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentContact)));
            }

        }
        public static async System.Threading.Tasks.Task<Windows.ApplicationModel.Contacts.Contact> CreateContact(string imagePath)
        {
            Debug.WriteLine(imagePath);
            var contact = new Windows.ApplicationModel.Contacts.Contact();
            contact.FirstName = "Betsy";
            contact.LastName = "Sherman";
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(imagePath);
            Debug.WriteLine(file.Path);
            // Get the app folder where the images are stored.
            /*var appInstalledFolder =
                Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assets = await appInstalledFolder.GetFolderAsync("picture");
            var imageFile = await assets.GetFileAsync(imagePath);*/
            contact.SourceDisplayPicture = file;

            return contact;
        }
        public static async System.Threading.Tasks.Task<Windows.ApplicationModel.Contacts.Contact> CreateDefaultContact()
        {

            var contact = new Windows.ApplicationModel.Contacts.Contact();

            // Get the app folder where the images are stored.
            var appInstalledFolder =
                Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assets = await appInstalledFolder.GetFolderAsync("Assets");
            var imageFile = await assets.GetFileAsync("nullframe.png");


            return contact;
        }

        private async void CameraPictureShowing()
        {

            Debug.WriteLine("Initial CaMERA ......... ");

            await InitializeCameraAsync();
        }
        private async Task InitializeCameraAsync()
        {
            if (_mediaCapture == null)
            {
                Debug.WriteLine("open device");
                // Get the camera devices
                var cameraDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                // try to get the back facing device for a phone
                var backFacingDevice = cameraDevices
                    .FirstOrDefault(c => c.EnclosureLocation?.Panel == Windows.Devices.Enumeration.Panel.Back);

                // but if that doesn't exist, take the first camera device available
                var preferredDevice = backFacingDevice ?? cameraDevices.FirstOrDefault();

                // Create MediaCapture
                _mediaCapture = new MediaCapture();

                // Initialize MediaCapture and settings
                await _mediaCapture.InitializeAsync(
                    new MediaCaptureInitializationSettings
                    {
                        VideoDeviceId = preferredDevice.Id
                    });

                if (PopUpWidget1.IsOpen == false)
                {
                    PopUpWidget1.IsOpen = true;
                }

                // Handle camera device location
                if (preferredDevice.EnclosureLocation == null ||
                    preferredDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                {
                    _externalCamera = true;
                }
                else
                {
                    _externalCamera = false;
                    _mirroringPreview = (preferredDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
                }

                _rotationHelper = new CameraRotationHelper(preferredDevice.EnclosureLocation);
                _rotationHelper.OrientationChanged += RotationHelper_OrientationChanged;
                _mediaCapture.GetPreviewRotation();
                // Set the preview source for the CaptureElement
                PreviewControl.Visibility = Visibility.Visible;
                PreviewControl.Source = _mediaCapture;
                PreviewControl.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                popUpDisplayText1.Text = "Take a Picture!";
                // Start viewing through the CaptureElement 
                await _mediaCapture.StartPreviewAsync();
                await SetPreviewRotationAsync();
                StartCapture();
            }
        }
        private async void StartCapture()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

            sw.Reset();//碼表歸零

            sw.Start();//碼表開始計時

            Debug.WriteLine("StopWatch init");
            countdown = 0;
            count_timer = new DispatcherTimer();
            count_timer.Interval = new TimeSpan(0, 0, 1);
            count_timer.Tick += timer_TickAsync;
            count_timer.Start();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            /*System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            sw.Reset();//碼表歸零
            sw.Start();//碼表開始計時
            Debug.WriteLine("StopWatch init");
            countdown = 0;
            count_timer = new DispatcherTimer();
            count_timer.Interval = new TimeSpan(0, 0, 1);
            count_timer.Tick += timer_TickAsync;
            count_timer.Start();*/
            if (_mediaCapture == null)
            {
                CurrentContact = await CreateContact(userImagePath);
                UploadToAzureStorage(userImageFile);
                PopUpWidget1.IsOpen = false;
                PopUpWidget2.IsOpen = false;
                userImage.Stop();
                imagePreview.Visibility = Visibility.Collapsed;

            }
        }
        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine("Retake Picture");
            imagePreview.Visibility = Visibility.Collapsed;
            PreviewControl.Visibility = Visibility.Visible;
            CameraPictureShowing();
        }
        private async void SnapButton_Click(object sender, RoutedEventArgs e)
        {
            CameraPictureShowing();
        }
        private async void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            PopUpWidget2.IsOpen = false;
        }


        private async void timechangeMessageShowing(int seconds)
        {

            Delay_Param = seconds;
            popUpDisplayText.Text = "Timer Changed to " + seconds + " seconds";
            PopUpWidget.IsOpen = true;
            await System.Threading.Tasks.Task.Delay(3000);
            PopUpWidget.IsOpen = false;
        }
        private void timerChangeI(object sender, TappedRoutedEventArgs e)
        {
            timerO.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];
            timerT.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];

            timerI.Foreground = (SolidColorBrush)Resources["focusTimerColor"];

            timechangeMessageShowing(5);
        }
        private void timerChangeO(object sender, TappedRoutedEventArgs e)
        {
            timerI.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];
            timerT.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];

            timerO.Foreground = (SolidColorBrush)Resources["focusTimerColor"];

            timechangeMessageShowing(10);
        }
        private void timerChangeT(object sender, TappedRoutedEventArgs e)
        {
            timerI.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];
            timerO.Foreground = (SolidColorBrush)Resources["defaultTimerColor"];

            timerT.Foreground = (SolidColorBrush)Resources["focusTimerColor"];

            timechangeMessageShowing(15);
        }
        private async void BackToDefaultStep()
        {
            step1Circle.Fill = (SolidColorBrush)Resources["defaultStepBackground"];
            step1Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.User;
            step1ColorStoryboard.Begin();

            step2Circle.Fill = (SolidColorBrush)Resources["defaultStepBackground"];
            step2Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.IdBadge;
            step2ColorStoryboard.Stop();

            step3Circle.Fill = (SolidColorBrush)Resources["defaultStepBackground"];
            step3Icon.Icon = FontAwesome.UWP.FontAwesomeIcon.Check;
            step3ColorStoryboard.Stop();


            VisitorName.Text = String.Empty;
            VisitorName.Visibility = Visibility.Visible;
            VisitorNameText.Text = String.Empty;
            VisitorNameText.Visibility = Visibility.Collapsed;

            displayName.Text = "-";

            visitorCompany.Text = String.Empty;
            visitorCompany.Visibility = Visibility.Collapsed;
            visitorCompanyText.Text = String.Empty;
            visitorCompanyText.Visibility = Visibility.Collapsed;

            PopUpWidget.IsOpen = false;
            CurrentContact = await CreateDefaultContact();
            VisitorName.Focus(FocusState.Programmatic);
        }


        private void OnLayoutUpdated(object sender, object e)
        {
            if (gdChild.ActualWidth == 0 && gdChild.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.PopUpWidget.HorizontalOffset;
            double ActualVerticalOffset = this.PopUpWidget.VerticalOffset;

            double a = gdChild.ActualWidth;
            double b = gdChild.ActualHeight;

            double NewHorizontalOffset = (RegisterArea.ActualWidth - gdChild.ActualWidth) / 2 - 24;
            double NewVerticalOffset = (RegisterArea.ActualHeight - gdChild.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.PopUpWidget.HorizontalOffset = NewHorizontalOffset;
                this.PopUpWidget.VerticalOffset = NewVerticalOffset;
            }
        }
        private void OnLayoutUpdated1(object sender, object e)
        {
            if (gdChild1.ActualWidth == 0 && gdChild1.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.PopUpWidget1.HorizontalOffset;
            double ActualVerticalOffset = this.PopUpWidget1.VerticalOffset;

            double a = gdChild1.ActualWidth;
            double b = gdChild1.ActualHeight;

            double NewHorizontalOffset = (RegisterArea.ActualWidth - gdChild1.ActualWidth) / 2 - 24;
            double NewVerticalOffset = (RegisterArea.ActualHeight - gdChild1.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.PopUpWidget1.HorizontalOffset = NewHorizontalOffset;
                this.PopUpWidget1.VerticalOffset = NewVerticalOffset;
            }
        }
        private void OnLayoutUpdated2(object sender, object e)
        {
            if (gdChild2.ActualWidth == 0 && gdChild2.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.PopUpWidget2.HorizontalOffset;
            double ActualVerticalOffset = this.PopUpWidget2.VerticalOffset;

            double a = gdChild2.ActualWidth;
            double b = gdChild2.ActualHeight;

            double NewHorizontalOffset = (RegisterArea.ActualWidth - gdChild2.ActualWidth) / 2 - 24;
            double NewVerticalOffset = (RegisterArea.ActualHeight - gdChild2.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.PopUpWidget2.HorizontalOffset = NewHorizontalOffset;
                this.PopUpWidget2.VerticalOffset = NewVerticalOffset;
            }
        }
        private void OnLayoutUpdated3(object sender, object e)
        {
            if (gdChild3.ActualWidth == 0 && gdChild3.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.PopUpWidget3.HorizontalOffset;
            double ActualVerticalOffset = this.PopUpWidget3.VerticalOffset;

            double a = gdChild3.ActualWidth;
            double b = gdChild3.ActualHeight;

            double NewHorizontalOffset = (RegisterArea.ActualWidth - gdChild3.ActualWidth) / 2 - 24;
            double NewVerticalOffset = (RegisterArea.ActualHeight - gdChild3.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.PopUpWidget3.HorizontalOffset = NewHorizontalOffset;
                this.PopUpWidget3.VerticalOffset = NewVerticalOffset;
            }
        }

        private async void timer_TickAsync(object sender, object e)
        {
            countdown++;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/crrect_answer1.mp3"));
            PopUpWidget3.IsOpen = true;
            if (countdown == 1)
            {
                mediaPlayer.Play();
                popUpDisplayText3.Text = "3";
            }
            if (countdown == 2)
            {

                mediaPlayer.Play();
                popUpDisplayText3.Text = "2";
            }
            if (countdown == 3)
            {

                mediaPlayer.Play();
                popUpDisplayText3.Text = "1";

            }
            if (countdown == 4)
            {
                PopUpWidget3.IsOpen = false;
                mediaPlayer1 = new MediaPlayer();
                mediaPlayer1.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/crrect_answer3.mp3"));
                mediaPlayer1.Play();
                ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

                // create storage file in local app storage
                string time = System.DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
                //string myPhotos = Environment.GetFolderPath(Environ
                string p = System.IO.Path.Combine(registerName + "-" + time + ".jpg");
                // create storage file in local app storage
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    p,
                    CreationCollisionOption.GenerateUniqueName);
                //var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                //Debug.WriteLine(appInstalledFolder.Path);               
                //var picture = await appInstalledFolder.GetFolderAsync("picture");
                // var imageFile = await picture.CreateFileAsync(p);

                using (var captureStream = new InMemoryRandomAccessStream())
                {
                    await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

                    using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        var decoder = await BitmapDecoder.CreateAsync(captureStream);
                        var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

                        var properties = new BitmapPropertySet {
                     { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.FlipHorizontal, PropertyType.UInt16) }
                     };
                        await encoder.BitmapProperties.SetPropertiesAsync(properties);

                        await encoder.FlushAsync();
                    }
                }
                // take 
                //await _mediaCapture.CapturePhotoToStorageFileAsync(imgFormat, file);

                // Get photo as a BitmapImage
                Debug.WriteLine(file.Path);
                userImage = new BitmapImage(new Uri(file.Path));
                userImagePath = file.Name;
                userImageFile = file;
                Debug.WriteLine(userImagePath);
                // imagePreview is a <Image> object defined in XAML
                PreviewControl.Visibility = Visibility.Collapsed;
                imagePreview.Visibility = Visibility.Visible;
                //imagePreview.FlowDirection = FlowDirection.RightToLeft;
                imagePreview.Source = userImage;
                _mediaCapture = null;
                count_timer.Stop();
                //await System.Threading.Tasks.Task.Delay(5000);

            }
        }

        private async Task SetPreviewRotationAsync()
        {
            if (!_externalCamera)
            {
                // Add rotation metadata to the preview stream to make sure the aspect ratio / dimensions match when rendering and getting preview frames
                var rotation = _rotationHelper.GetCameraPreviewOrientation();
                var props = _mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
                Guid RotationKey = new Guid("C380465D-2271-428C-9B83-ECEA3B4A85C1");
                props.Properties.Add(RotationKey, CameraRotationHelper.ConvertSimpleOrientationToClockwiseDegrees(rotation));
                await _mediaCapture.SetEncodingPropertiesAsync(MediaStreamType.VideoPreview, props, null);
            }
        }

        private async void RotationHelper_OrientationChanged(object sender, bool updatePreview)
        {
            if (updatePreview)
            {
                await SetPreviewRotationAsync();
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                // Rotate the buttons in the UI to match the rotation of the device
                var angle = CameraRotationHelper.ConvertSimpleOrientationToClockwiseDegrees(_rotationHelper.GetUIOrientation());
                var transform = new RotateTransform { Angle = angle };

                // The RenderTransform is safe to use (i.e. it won't cause layout issues) in this case, because these buttons have a 1:1 aspect ratio
                //CapturePhotoButton.RenderTransform = transform;
                //CapturePhotoButton.RenderTransform = transform;
            });
        }

        private async Task<int> UploadToAzureStorage(StorageFile file)
        {
            try
            {
                //  create Azure Storage
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=visitorimageblob;AccountKey=/z6meWEOmL2znURclm+n4q6p2+IQWA0l2EXogWfRCfx/SttPXJkoHrLMI5BQuqGA15VmYQnNwaxx43nZkn0KBQ==;EndpointSuffix=core.windows.net");

                //  create a blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // create storage file in local app storage
                string time = System.DateTime.Now.ToString("yyyy'-'MM'-'dd", CultureInfo.CurrentUICulture.DateTimeFormat);

                //string myPhotos = Environment.GetFolderPath(Environ
                string p = System.IO.Path.Combine("picture-" + time);

                //  create a container 
                CloudBlobContainer container = blobClient.GetContainerReference(p);

                await container.CreateIfNotExistsAsync();

                // Set the permissions so the blobs are public. 
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await container.SetPermissionsAsync(permissions);
                CloudBlockBlob blob = null;
                string filename = null;

                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                if (null != fileStream)
                {
                    filename = file.Name;
                    blob = container.GetBlockBlobReference(filename);
                    await blob.DeleteIfExistsAsync();
                    await blob.UploadFromFileAsync(file);
                    var blobUrl = blob.Uri.AbsoluteUri;

                    Debug.WriteLine("Upload picture succeed! Filename: " + filename);
                    Debug.WriteLine("Url : " + blobUrl);
                    await PostPhotoAsync(blobUrl);
                }
                return 1;
            }
            catch
            {
                //  return error
                return 0;
            }
        }
    }
}
