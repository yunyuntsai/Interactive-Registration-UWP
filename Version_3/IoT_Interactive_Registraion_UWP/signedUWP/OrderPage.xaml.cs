using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
        private XmlDocument worksheet;

        static List<string> _sharedStrings;

        static List<Dictionary<string, string>> _derivedData;

        ObservableCollection<object> data1 = new ObservableCollection<object> { };

        public static List<Dictionary<string, string>> DerivedData
        {
            get
            {
                return _derivedData;
            }
        }

        static List<string> _header;

        public static List<string> Headers { get { return _header; } }

        private ApplicationView currentView;

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

        private void CurrentView_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            // Clean up code to shut down secondary windows as this one closes
            Application.Current.Exit();
        }
        private async Task<UserList> LoadUserList()
        {
            
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";

            UserList nu = new UserList();
            UserList empty = new UserList();
            using (var client = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine("Connect Http Client");
                client.BaseAddress = new Uri(baseAPIUrl2);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(baseAPIUrl2 + "api/Users/");
                Debug.WriteLine(response);
                string httpResponseBody = "";

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Response Success!!!");
                    httpResponseBody = await response.Content.ReadAsStringAsync();

                    if (httpResponseBody.ToString() != "[]")
                    {
                        //var outObject = JsonConvert.DeserializeObject<Users>(s);
                        //string body = httpResponseBody.Trim(new Char[] { '[', ']' });
                        Users user = new Users();
                        //TextBlock2.Text = "Result: "; //+ httpResponseBody;
                        UserList u = DataHelper.GetUsers(httpResponseBody);

                        if (u.Count > 0 )
                        {
                            for (int i = 0; i < u.Count; i++)
                            {
                                if (u[i].Arrived == "Yes")
                                {
                                    string time1 = u[i].UpdateTime.Substring(11, 5);
                                    u[i].UpdateTime = time1;
                                    Debug.WriteLine(u[i].UpdateTime);

                                }
                                else
                                {
                                    u[i].UpdateTime = null;

                                }
                                nu.Add(u[i]);
                            }
                        }
                        if (nu != null) InventoryList.ItemsSource = nu;

                        Debug.WriteLine(httpResponseBody);
                    }
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
            
            if (nu!=null) return nu;
            else return empty;
        }

        public async Task ClearAsync(string UserId)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            var json = "";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.PutAsync(baseAPIUrl2 + "api/Clear/" + UserId, content).Result;

        }

        public async Task DeleteAsync(string UserId)
        {
            Debug.WriteLine("Delete!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            var json = "";
            StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var result = httpClient.DeleteAsync(baseAPIUrl2 + "api/Delete/" + UserId).Result;

        }

        public async Task PostAsync(string JsonString)
        {
            Debug.WriteLine("Post!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl2);

            StringContent content = new System.Net.Http.StringContent(JsonString, Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(baseAPIUrl2 + "api/Users/" ,content).Result;
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

        private void liv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var selectedItem in e.AddedItems)
            {
                data1.Add(selectedItem);
               
            }
            foreach (var unSelectedItem in e.RemovedItems)
            {
                data1.Remove(unSelectedItem);
            }
           foreach(Users item in data1)
            {
                Debug.WriteLine(item.Name.ToString());
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

        private async void restate_Click(object sender, RoutedEventArgs e)
        {
            foreach (Users item in data1)
            {
                Debug.WriteLine(item.Name.ToString());
                ClearAsync(item.Id.ToString());
            }
            Task.Delay(3000);
            await LoadUserList();
            reload();
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (Users item in data1)
            {
                Debug.WriteLine(item.Name.ToString());
                DeleteAsync(item.Id.ToString());
            }
            Task.Delay(3000);
            await LoadUserList();
            reload();
        }

        private async void deleteall_Click(object sender, RoutedEventArgs e)
        {
            UserList u = await LoadUserList();
            IList<object> addlist = new List<object>();
            IList<object> removelist = new List<object>();

            Debug.WriteLine("Delete All!!!!");
            var httpClient = new HttpClient();
            String baseAPIUrl = "http://webapplication2201802.azurewebsites.net/";
            String baseAPIUrl2 = "http://iotregistapi.azurewebsites.net/";
            httpClient.BaseAddress = new Uri(baseAPIUrl2);
            
            for(int i = 0 ; i <u.Count ; i++)
            {
                addlist.Add(u[i]);
                SelectionChangedEventArgs args = new SelectionChangedEventArgs(removelist,addlist);
                liv_SelectionChanged(sender, args);
            }
            MessageDialog m  = new MessageDialog("Delete all...");
            foreach (Users item in data1)
            {
                Debug.WriteLine(item.Name.ToString());
                DeleteAsync(item.Id.ToString());
            }
            Task.Delay(3000);
            await LoadUserList();
            reload();
        }

        private async void reload()
        {
            await LoadUserList();
        }
        private async void btnopenfile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.FileTypeFilter.Add(".xlsx");
            opener.FileTypeFilter.Add(".xlsm");
            StorageFile file = await opener.PickSingleFileAsync();
            Debug.WriteLine("Choose a file to browse!");
            if (file != null)
            {
                using (var fileStream = await file.OpenReadAsync())
                {
                    using (ZipArchive archive = new ZipArchive(fileStream.AsStream(), ZipArchiveMode.Read))
                    {
                        //worksheet = this.GetSheet(archive, "sheet1");

                        List<Dictionary<string, string>> list =  GetSheet(archive, "sheet1");
         
                        foreach (var dic in list)
                        {
                            var outList = new Dictionary<string, string>();
                            StringBuilder builder = new StringBuilder();
                            string ss;
                            foreach (var keyValue in dic)
                            {
                                if (keyValue.Key.Contains("Id") && !keyValue.Key.Contains("VisitorId")) outList.Add(keyValue.Key, keyValue.Value);
                                else if (keyValue.Key.Contains("VisitorName")) outList.Add(keyValue.Key, keyValue.Value);
                                else if (keyValue.Key.Contains("VisitorCompany")) outList.Add(keyValue.Key, keyValue.Value);
                                else if (keyValue.Key.Contains("Arrived")) outList.Add(keyValue.Key, keyValue.Value);
                                else continue;
                               
                            }
                            foreach (var item in outList)
                            {

                                if (item.Key == "Id")
                                {
                                    ss = string.Format("\"{0}\":{1}", item.Key, string.Join(",", item.Value));
                                    builder.Append(ss);
                                }
                                else
                                {
                                    builder.Append(",");
                                    ss = string.Format("\"{0}\":\"{1}\"", item.Key, string.Join(",", item.Value));
                                    builder.Append(ss);
                                }
                            }
                            string JsonString = "{" + builder + "}";
                            Debug.WriteLine(JsonString);
                            await PostAsync(JsonString);
                            outList.Clear();
                            Debug.WriteLine("----------------------");
                        }
                     
                        Debug.WriteLine("Upload!!!" );
                    }
                }
            }
            await LoadUserList();
        }
               
        private List<Dictionary<string,string>> GetSheet(ZipArchive archive, string sheetName)
        {
            XmlDocument sheet = new XmlDocument();
            ZipArchiveEntry archiveEntry = archive.GetEntry("xl/worksheets/" + sheetName + ".xml");
            var sharedString = archive.GetEntry("xl/sharedStrings.xml");

            //get shared string
            _sharedStrings = new List<string>();
            using (var sr = sharedString.Open())
            {
                XDocument xdoc = XDocument.Load(sr);
                _sharedStrings =
                    (
                    from e in xdoc.Root.Elements()
                    select e.Elements().First().Value
                    ).ToList();
            }

            //get header
            using (var sr = archiveEntry.Open())
            {
                XDocument xdoc = XDocument.Load(sr);
                //get element to first sheet data
                XNamespace xmlns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
                XElement sheetData = xdoc.Root.Element(xmlns + "sheetData");

                _header = new List<string>();
                //build header first
                var firstRow = sheetData.Elements().First();
                //full of c
                foreach (var c in firstRow.Elements())
                {
                    //the c element, if have attribute t, will need to consult sharedStrings
                    string val = c.Elements().First().Value;
                    if (c.Attribute("t") != null)
                    {
                        _header.Add(_sharedStrings[Convert.ToInt32(val)]);
                    }
                    else
                    {
                        _header.Add(val);
                    }

                }

                //build content now
                _derivedData = new List<Dictionary<string, string>>();

                foreach (var row in sheetData.Elements())
                {
                    //skip row 1
                    if (row.Attribute("r").Value == "1")
                        continue;
                    Dictionary<string, string> rowData = new Dictionary<string, string>();
                    int i = 0;
                    foreach (var c in row.Elements())
                    {
                        //down to each c element
                        string val = c.Elements().First().Value;
                        if (c.Attribute("t") != null)
                        {
                            rowData.Add(_header[i], _sharedStrings[Convert.ToInt32(val)]);
                        }
                        else
                        {
                            rowData.Add(_header[i], val);
                        }
                        i++;
                    }
                  
                    _derivedData.Add(rowData);
                }
            }
          
            return _derivedData;
        }

    }
}
