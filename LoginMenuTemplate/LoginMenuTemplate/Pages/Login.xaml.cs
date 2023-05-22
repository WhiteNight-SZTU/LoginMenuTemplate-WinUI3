// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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
using Windows.Foundation.Metadata;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoginMenuTemplate.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            if(logged == true)
            {
                LoginMenu.Visibility = Visibility.Collapsed;
                ExitLoginMenu.Visibility = Visibility.Visible;
            }else
            {
                LoginMenu.Visibility = Visibility.Visible;
                ExitLoginMenu.Visibility = Visibility.Collapsed;
            }
        }
        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                passwordBox.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                passwordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void LoginCheck(object sender, RoutedEventArgs e)
        {
             accountName = accountBox.Password;
             password = passwordBox.Password;
             
            try
            {
                var connectionString = $"{IPAddress.Parse(Settings.database_ip)},password={Settings.database_password}";
                var connection = ConnectionMultiplexer.Connect(connectionString);
                var database = connection.GetDatabase();

                RedisValue redisPassword = database.StringGet(accountName);
                if (redisPassword.HasValue && redisPassword == password)
                {
                    // 用户已经找到
                    logged = true;
                    LoginMenu.Visibility = Visibility.Collapsed;
                    ExitLoginMenu.Visibility = Visibility.Visible;
                    CantFindUser.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // 用户未找到
                    CantFindUser.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage_CantFindRedis();
            }
            
        }
        private void ExitLogin(object sender, RoutedEventArgs e)
        {
            logged=false;
            LoginMenu.Visibility = Visibility.Visible;
            ExitLoginMenu.Visibility=Visibility.Collapsed;

        }
        private async void ErrorMessage_CantFindRedis()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Error!\n错误原因：未能连接至数据库。\n";
            dialog.PrimaryButtonText = "返回";
            dialog.DefaultButton = ContentDialogButton.Primary;
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = this.Content.XamlRoot;
            }
            var result = await dialog.ShowAsync();
        }
        public static string accountName;
        public static string password;
        public static bool logged = false;
    }
}
