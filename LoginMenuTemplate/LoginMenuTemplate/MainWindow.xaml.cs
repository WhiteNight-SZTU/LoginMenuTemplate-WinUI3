// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using LoginMenuTemplate.Pages;
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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LoginMenuTemplate
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            current = this;
            backgroundImage = ApplicationBackgroundImage;
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            
        }
        private void LoadPages(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
            string pageName = "LoginMenuTemplate.Pages." + ((string)selectedItem.Tag);
            Type pageType = Type.GetType(pageName);
            if (pageType == null)
            {
                //Should never happened.
            }
            else
            {
                pagesFrame.Navigate(pageType);
                currentFrame = pagesFrame;
                currentPagesFrame = pagesFrame;
            }
       
        }
        
      
        public static MainWindow current;
        public static Microsoft.UI.Xaml.Controls.Frame currentFrame;
        public static Microsoft.UI.Xaml.Media.ImageBrush backgroundImage;
        public static Frame currentPagesFrame;
        
    }
}

