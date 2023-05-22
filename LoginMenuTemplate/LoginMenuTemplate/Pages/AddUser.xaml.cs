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
    public sealed partial class AddUser : Page
    {
        public AddUser()
        {
            this.InitializeComponent();
        }
       private void AddUserToRedis(object sender, RoutedEventArgs e)
       {
            if(Login.logged==false)
            {
                ErrorMessage_Unlogged();
            }else
            {
                string accountName = accountBox.Password;
                string password = passwordBox.Password;
                try
                {
                    var connectionString = $"{IPAddress.Parse(Settings.database_ip)},password={Settings.database_password}";
                    var connection = ConnectionMultiplexer.Connect(connectionString);
                    var database = connection.GetDatabase();

                    RedisValue redisPassword = database.StringGet(accountName);
                    if (redisPassword.HasValue && redisPassword == password)
                    {
                        // 用户已存在
                        ExUser.Visibility = Visibility.Visible;
                        Success.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        // 添加用户
                        var redisResult=database.StringSet(accountName, password);
                        if(redisResult)
                        {
                            Success.Visibility = Visibility.Visible;
                            ExUser.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            ErrorMessage_FailToAddUser();
                        }
                    }
                }catch (Exception ex)
                {

                }
            }
       }
       private async void ErrorMessage_Unlogged()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Error!\n错误原因：未登录。\n请先登录\n";
            dialog.PrimaryButtonText = "返回";
            dialog.DefaultButton = ContentDialogButton.Primary;
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = this.Content.XamlRoot;
            }
            var result = await dialog.ShowAsync();
        }
       private async void ErrorMessage_FailToAddUser()
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Error!\n错误原因：添加失败。\n请重试\n";
            dialog.PrimaryButtonText = "返回";
            dialog.DefaultButton = ContentDialogButton.Primary;
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = this.Content.XamlRoot;
            }
            var result = await dialog.ShowAsync();
        }
    }
}
