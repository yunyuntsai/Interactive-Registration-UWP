using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class OrderPage : Page
    {
        public OrderPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoadUserList();
            //textBox1.Focus(FocusState.Programmatic);
            if (e.Parameter is string)
            {
                await LoadUserList();
            }
        }

        private async Task LoadUserList()
        {

            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";

            /*textBox1.Focus(FocusState.Programmatic);
            TextBlock1.Text = ""; TextBlock2.Text = "";
            textBox1.Text = ""; textBox2.Text = "";
            textBox2.IsEnabled = false;
            EnterButton1.IsEnabled = false;
            EnterButton1.Background = (SolidColorBrush)Resources["LightGrey"];
            EnterButton.Background = (SolidColorBrush)Resources["LightGrey"];
            ArrivedUser = 0;
            TotalUser = 0;*/

            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl + "api/Users/");
                Debug.WriteLine(response);
                string httpResponseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();


                    //var outObject = JsonConvert.DeserializeObject<Users>(s);
                    //string body = httpResponseBody.Trim(new Char[] { '[', ']' });
                    Users user = new Users();
                    //TextBlock2.Text = "Result: "; //+ httpResponseBody;
                    UserList u = DataHelper.GetUsers(httpResponseBody);
                    /*TotalUser = u.Count();
                    for (int i = 0; i < u.Count; i++)
                    {
                        if (u[i].Arrived == "Yes")
                        {
                            ArrivedUser += 1;
                        }
                    }
                    TextBlock5.Text = TotalUser.ToString();
                    TextBlock6.Text = ArrivedUser.ToString();*/

                    //httpResponseBody.Replace("[", "").Replace("]", "");
                    InventoryList.ItemsSource = u;
                    Debug.WriteLine(httpResponseBody);
                    try
                    {
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
    }
}
