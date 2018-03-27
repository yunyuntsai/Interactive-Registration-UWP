﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.SqlClient;
using System.Diagnostics;

namespace signedUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        // This MobileServiceClient has been configured to communicate with the Azure Mobile Service and
        // Azure Gateway using the application url. You're all set to start working with your Mobile Service!
        public static Microsoft.WindowsAzure.MobileServices.MobileServiceClient uwpapp2018Client = new Microsoft.WindowsAzure.MobileServices.MobileServiceClient(
        "http://uwpapp2018.azurewebsites.net");


        //TODO: replace string with your own SQL server connection string, then reinstall the app
        private string connectionString = @"Data Source=uwptestdatabase.database.windows.net;Initial Catalog=uwpappdb;Persist Security Info=True;User ID=alice;Password=Admin123";


        public string ConnectionString { get; set; }

        public MainPage MainPageInstance { get; set; }

        public UserList Users { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        private void GetAppSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings; 
            var sqlString = localSettings.Values["sqlconnection"];

            var cb = new SqlConnectionStringBuilder();
            cb.DataSource = "uwptestdatabase.database.windows.net";
            cb.UserID = "alice";
            cb.Password = "Admin123";
            cb.InitialCatalog = "uwpappdb";

            if (sqlString is string)
            {
                ConnectionString = cb.ConnectionString;
                localSettings.Values["sqlconnection"] = cb.ConnectionString;
                //ConnectionString = (sqlString as string);
                Debug.WriteLine("Connect : ................ " + ConnectionString);
            }
            else
            {
                ConnectionString = connectionString;
                localSettings.Values["sqlconnection"] = connectionString;
                Debug.WriteLine("Connect !!!! : " + ConnectionString);
            }
        }
      
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                //GetAppSettings();

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
