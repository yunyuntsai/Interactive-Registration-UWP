using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace signedUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScanPage : Page
    {
        public Users MyUser;

        public ScanPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

             MyUser = (Users)e.Parameter;

             TextBlock7.Text = "Hi~" + MyUser.Name + ", \nPlease scan the tag !";
             
            // parameters.Name
            // parameters.Text
            // ...
        }

        private async void OnKeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBlock1.Text = "You Entered: " + textBox1.Text;
                
                await GetUsersDetailAsync(MyUser.Id.ToString());
            }
        }
        public async void PostAsync(string UserId, string ScanId)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);
   
            var json = "{\"TagId\":" + ScanId + "}";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl + "api/Users/" + UserId, content).Result;
            
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
                PostAsync(MyUser.Id.ToString(), textBox1.Text);
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl + "api/Users/" + id);
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();

                    //TextBlock2.Text = "Result: ";// + httpResponseBody;

                    //var outObject = JsonConvert.DeserializeObject<Users>(s);
                    string body = httpResponseBody.Trim(new Char[] { '[', ']' });
                    Users user = new Users();
                    //httpResponseBody.Replace("[", "").Replace("]", "");

                    Debug.WriteLine(body);
                    try
                    {
                        var dyn = JsonConvert.DeserializeObject<JObject>(body);

                        JProperty propEnroll = dyn.Properties().FirstOrDefault(i => i.Name == "Arrived");
                        JProperty propName = dyn.Properties().FirstOrDefault(i => i.Name == "Name");
                        JProperty propTime = dyn.Properties().FirstOrDefault(i => i.Name == "UpdateTime");
                        Debug.WriteLine(propEnroll.Value.ToString());
                        Debug.WriteLine(propName.Value.ToString());
                        Debug.WriteLine(propTime.Value.ToString());

                        if (propTime != null && propEnroll != null && propName != null)
                        {
                            
                            string name = propName.Value.ToString();
                            //int age = int.Parse(propTime.Value.ToString());
                            //int en = Int16.Parse(propEnroll.Value.ToString());
                            //Debug.WriteLine(en);
                            TextBlock4.Text = "已簽到";
                            TextBlock5.Text = propName.Value.ToString();
                            TextBlock6.Text = propTime.Value.ToString();

                            
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    //var result  =JsonConvert.DeserializeObject<Users>(httpResponseBody);
                    //user.UserID = result.UserID;
                    //user.UserName = result.UserName;


                }
            }
        }
    }
}
