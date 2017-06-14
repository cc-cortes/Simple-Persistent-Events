using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp;

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const String mUserPresentTaskName = "User Present Persistent Event";
        private const String mTimeZoneChangeTaskName = "Time Zone Change Persistent Event";
        private const String mUserAwayTaskName = "User Away Persistent Event";
        private const String mMaintenanceTaskName = "Maintenance Window Persistent Event";
        private bool? mIsGroupApiPresent;


        public MainPage()
        {
            this.InitializeComponent();

            //Check if the necessary API is available on this OS Build for Persistent Events
            if(IsGroupApiPresent())
            {
                UpdateEventRegistrationText();
            }
            else
            {
                ShowUnsupportedBuildMessage();
            }
        }

        private void ShowUnsupportedBuildMessage()
        {
            //Persistent Events are not supported on this build, hide content and show the unsupported message
            ContentGrid.Visibility = Visibility.Collapsed;
            UnsupportedBuildPanel.Visibility = Visibility.Visible;
        }

        private void UpdateEventRegistrationText()
        {
            //Check if the tasks for each persistent event are registered and update the text accordingly
            UserPresentRegistered.Text = IsTaskRegistered(mUserPresentTaskName) ? "True" : "False";
            TimeZoneChangeRegistered.Text = IsTaskRegistered(mTimeZoneChangeTaskName) ? "True" : "False";
            UserAwayRegistered.Text = IsTaskRegistered(mUserAwayTaskName) ? "True" : "False";
            MaintenanceWindowRegistered.Text = IsTaskRegistered(mMaintenanceTaskName) ? "True" : "False";
        }

        private bool IsTaskRegistered(String name)
        {
            //Look through all background task registration groups to see if there is a task matching the name provided
            var groups = BackgroundTaskRegistration.AllTaskGroups;

            foreach (var group in groups)
            {
                foreach(var reg in group.Value.AllTasks)
                {
                    if(reg.Value.Name == name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsGroupApiPresent()
        {
            // Save a cached version of the API Information check
            if (mIsGroupApiPresent == null)
            {
                mIsGroupApiPresent = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.BackgroundTaskRegistrationGroup");
            }

            return (bool)mIsGroupApiPresent;
        }

        private void Register_TimeZoneChange(object sender, RoutedEventArgs e)
        {
            PersistentEvents.TimeZoneChange += BackgroundActivity.TimeZoneChange;
            UpdateEventRegistrationText();
        }
        private void Unregister_TimeZoneChange(object sender, RoutedEventArgs e)
        {
            PersistentEvents.TimeZoneChange -= BackgroundActivity.TimeZoneChange;
            UpdateEventRegistrationText();
        }
        private void Register_UserPresent(object sender, RoutedEventArgs e)
        {
            PersistentEvents.UserPresent += BackgroundActivity.UserPresent;
            UpdateEventRegistrationText();
        }
        private void Unregister_UserPresent(object sender, RoutedEventArgs e)
        {
            PersistentEvents.UserPresent -= BackgroundActivity.UserPresent;
            UpdateEventRegistrationText();
        }

        private void Register_UserAway(object sender, RoutedEventArgs e)
        {
            PersistentEvents.UserAway += BackgroundActivity.UserAway;
            UpdateEventRegistrationText();
        }
        private void Unregister_UserAway(object sender, RoutedEventArgs e)
        {
            PersistentEvents.UserAway -= BackgroundActivity.UserAway;
            UpdateEventRegistrationText();
        }

        private void Register_MaintenanceWindow(object sender, RoutedEventArgs e)
        {
            PersistentEvents.MaintenanceWindow += BackgroundActivity.MaintenanceWindow;
            UpdateEventRegistrationText();
        }
        private void Unregister_MaintenanceWindow(object sender, RoutedEventArgs e)
        {
            PersistentEvents.MaintenanceWindow -= BackgroundActivity.MaintenanceWindow;
            UpdateEventRegistrationText();
        }
    }
}
