// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoginMenuTemplate.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            if (database_ip != null)
            {
                accountBox.Password = database_ip;
            }
            if(database_password != null)
            {
                passwordBox.Password = database_password;
            }
            if(showDatabase_ip==true)
            {
                revealModeCheckBox_ip.IsChecked = true;
                accountBox.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            if(showDatabase_password==true)
            {
               revealModeCheckBox.IsChecked = true;
               passwordBox.PasswordRevealMode= PasswordRevealMode.Visible;
            }

            preLoadSetting = false;

        }
        private async void changeAppBackground(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.current);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            filePicker.FileTypeFilter.Add("*");
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);
                bitmapImage.SetSource(stream);
                MainWindow.backgroundImage.ImageSource = bitmapImage;
            }
        }
        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (preLoadSetting == false)
            {
                if (revealModeCheckBox.IsChecked == true)
                {
                    passwordBox.PasswordRevealMode = PasswordRevealMode.Visible;
                    showDatabase_password = true;
                }
                else
                {
                    passwordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
                    showDatabase_password = false;
                }
            }
        }
        private void RevealModeCheckbox_ip_Changed(object sender, RoutedEventArgs e)
        {
            if(preLoadSetting == false)
            {
                if (revealModeCheckBox_ip.IsChecked == true)
                {
                    accountBox.PasswordRevealMode = PasswordRevealMode.Visible;
                    showDatabase_ip = true;
                }
                else
                {
                    accountBox.PasswordRevealMode = PasswordRevealMode.Hidden;
                    showDatabase_password = false;
                }
            }
        }

        private void SETIP(object sender, RoutedEventArgs e)
        {
            database_ip = accountBox.Password;
        }

        private void SETPassword(object sender, RoutedEventArgs e)
        {
            database_password= passwordBox.Password;
            
        }

        public static string database_ip="127.0.0.1";
        public static string database_password;
        private static bool showDatabase_ip=false;
        private static bool showDatabase_password=false;
        private bool preLoadSetting = true;

    }
}
