﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
    public partial class AddSubSubscriptionWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        public AddSubSubscriptionWin(MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the IsKeyboardFocused event of the tbSub control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbSub_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbSub.IsKeyboardFocused == true)
            {
                subPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbSub.IsKeyboardFocused && string.IsNullOrEmpty(tbSub.Text))
            {
                subPlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the IsKeyboardFocused event of the tbType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbType_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbType.IsKeyboardFocused == true)
            {
                typePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbType.IsKeyboardFocused && string.IsNullOrEmpty(tbType.Text))
            {
                typePlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }

        /// <summary>
        /// Handles the Click event of the btnOk control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }

        /// <summary>
        /// Messages the user about an attempt to perform an action.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void MessageBoxHandler(object sender)
        {
            MessageBoxResult message;
            if (sender == btnOk)
            {
                if ((string.IsNullOrEmpty(tbSub.Text) || string.IsNullOrEmpty(tbType.Text)))
                {
                    MessageBox.Show("Please fill all available fields.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    message = MessageBox.Show("Сarefully check the data you entered before adding it to the table.", 
                        "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            if (int.TryParse(tbSub.Text.Trim(), out int sub) == true && int.TryParse(tbType.Text.Trim(), out int type))
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var subSubscription = new SubscriberSubscription() { SubscriberId = sub, SubscriptionId = type };
                                    subs.SubscribersSubscriptions.Add(subSubscription);

                                    try
                                    {
                                        subs.SaveChanges();
                                        ThisMainWindow.RefreshDataGrid();
                                        ClearFields();
                                    }
                                    catch(DbUpdateException)
                                    {
                                        MessageBox.Show("Please make sure that entered Subscriber Id or Subscription Id " +
                                            "are existing.", "Something went wrong",
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
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
                message = MessageBox.Show("All changes will be cancelled!", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        this.Close();
                    break;
                }
            }
        }
        private void ClearFields()
        {
            tbSub.Clear();
            tbType.Clear();
        }
    }
}
