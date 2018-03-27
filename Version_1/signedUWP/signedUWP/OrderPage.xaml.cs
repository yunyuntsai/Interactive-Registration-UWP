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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
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

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Delete All!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);

            var json = "";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.DeleteAsync(baseAPIUrl + "api/Delete/").Result;
        }
        public async Task ClearAsync(string UserId)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);

            var json = "";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl + "api/Clear/" + UserId, content).Result;

        }

        public async Task DeleteAsync(string UserId)
        {
            Debug.WriteLine("Delete!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl);

            var json = "";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.DeleteAsync(baseAPIUrl + "api/Delete/" + UserId).Result;

        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
            Debug.WriteLine("can clear///");
            if (InventoryList.SelectedItem != null)
            {
                var users = (Users)InventoryList.SelectedItem;
                e.Data.SetText(users.Id.ToString());
                Debug.WriteLine(users.Id.ToString());
                ClearAsync(users.Id.ToString());
                Task.Delay(2000);
                LoadUserList();
            }
        }

        private void Grid_DragOver1(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
            Debug.WriteLine("can delete///");
            if (InventoryList.SelectedItem != null)
            {
                var users = (Users)InventoryList.SelectedItem;
                e.Data.SetText(users.Id.ToString());
                Debug.WriteLine(users.Id.ToString());
                DeleteAsync(users.Id.ToString());
                Task.Delay(2000);
                LoadUserList();
            }
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                /*var items = await e.DataView.GetStorageItemsAsync();
                if (items.Count > 0)
                {
                    Debug.WriteLine("DROP 1111");
                    var storageFile = items[0] as StorageFile;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(await storageFile.OpenAsync(FileAccessMode.Read));
                    // Set the image on the main page to the dropped image
                    Image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/GreenDot.png"));
                }*/

                if (InventoryList.SelectedItem != null)
                {
                    var users = (Users)InventoryList.SelectedItem;
                    e.Data.SetText(users.Id.ToString());
                    e.Data.RequestedOperation = DataPackageOperation.Copy;
                    Debug.WriteLine(users.Name.ToString());

                }
            }
        }

        #region Drag and drop handling code       
        private void InventoryList_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            Debug.WriteLine("Drag~~~~OVER");

        }

        IList<object> draggingItems;
        private void InventoryList_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            draggingItems = e.Items;
            Debug.WriteLine("Drag~~~~");
            Debug.WriteLine(InventoryList.SelectedItem);
            if (InventoryList.SelectedItem != null)
            {
                var users = (Users)InventoryList.SelectedItem;
                e.Data.SetText(users.Id.ToString());
               
                
            }
        }

        private void cnvDrop_Drop(object sender, DragEventArgs e)
        {
            
            if (InventoryList.SelectedItem != null)
            {
                //TODO: hanle the deletion here
                Debug.WriteLine("delete");
            }
        }

        private void InventoryList_DropItemsStarting(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {

                if (InventoryList.SelectedItem != null)
                {
                    var users = (Users)InventoryList.SelectedItem;
                    e.Data.SetText(users.Id.ToString());
                    //e.Data.RequestedOperation = DataPackageOperation.Copy;
                    Debug.WriteLine("drop.........");

                }
            }
        }


        #endregion
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

    }
}
