﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DBApp.Forms.NewRecord
{
    public partial class AddSubWin : Window
    {
        public AddSubWin()
        {
            InitializeComponent();
        }

        private void tbEmail_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(tbEmail.IsKeyboardFocused == true) 
            {
                emailPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbEmail.IsKeyboardFocused && string.IsNullOrEmpty(tbEmail.Text))
            {
                emailPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void tbDate_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbDate.IsKeyboardFocused == true)
            {
                datePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if(!tbDate.IsKeyboardFocused && string.IsNullOrEmpty(tbDate.Text))
            {
                datePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }

        private void MessageBoxHandler(object sender)
        {
            MessageBoxResult message;
            if (sender == btnOk)
            {
                if ((string.IsNullOrEmpty(tbDate.Text) || string.IsNullOrEmpty(tbEmail.Text)))
                {
                    MessageBox.Show("Please fill all available fields.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    message = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            if (DateTime.TryParse(tbDate.Text.Trim(), out DateTime date) == true && tbEmail.Text.Contains('@'))
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var sub = new Subscriber() { Email = tbEmail.Text.Trim(), BirthDate = date };

                                    subs.Subscribers.Add(sub);
                                    subs.SaveChanges();
                                }
                            }

                            else
                            {
                                MessageBox.Show("Please make sure that all fields are filled out in the right way.", 
                                    "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                    }
                }
            }
            else if (sender == btnCancel)
            {
                message = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        this.Close();
                        break;
                }
            }
        }
    }
}