﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DvorakTrainer.Views;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DvorakTrainer.Controls
{
    public sealed partial class ProgramMenu : UserControl
    {
        public event EventHandler Closed;

        public ProgramMenu()
        {
            this.InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }

        public void Open()
        {
            MenuListBox.SelectedItem = AboutLBI;
            this.Visibility = Visibility.Visible;
            SplitView1.Visibility = Visibility.Visible;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SplitView1.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Collapsed;
            Closed?.Invoke(this, new EventArgs());
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuListBox.SelectedItem == AboutLBI)
            {
                ContentFame.Navigate(typeof(AboutPage));
            }
            else if (MenuListBox.SelectedItem == HelpLBI)
            {
                ContentFame.Navigate(typeof(HelpPage));
            }
        }
    }
}
