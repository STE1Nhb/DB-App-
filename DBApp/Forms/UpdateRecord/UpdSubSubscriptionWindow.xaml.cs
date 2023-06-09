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

namespace DBApp.Forms.UpdateRecord
{
    /// <summary>
    /// Represents a form with fields to be filled in, by which you can modify the record in subscribers_subscriptions table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class UpdSubSubscriptionWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        private int TargetId { get; set; }
        public UpdSubSubscriptionWin(int targetId, MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            TargetId = targetId;
            InitializeComponent();
        }

        /// <summary>
        /// Makes the placeholder of TextBoxes visible on mouse click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbType.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Makes the placeholder of tbType TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbType_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbType.IsKeyboardFocused)
            {
                typePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbType.IsKeyboardFocused && string.IsNullOrEmpty(tbType.Text))
            {
                typePlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Cancels the changes on btnCancel button click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateRecord(sender);
        }

        /// <summary>
        /// Modifies the table record on btnOk click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            UpdateRecord(sender);
        }

        /// <summary>
        /// Updates the table record.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void UpdateRecord(object sender)
        {
            if (sender == btnOk)
            {
                if (string.IsNullOrEmpty(tbType.Text))
                {
                    MessageBox.Show("Please fill all available fields.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (int.TryParse(tbType.Text.Trim(), out int type))
                    {
                        using (var subs = new DbAppContext())
                        {
                            var subSubscription = subs.SubscribersSubscriptions.SingleOrDefault(s => s.SubscriberId == TargetId);
                            subSubscription.SubscriptionId = type;

                            try
                            {
                                subs.SaveChanges();
                                ThisMainWindow.RefreshDataGrid();
                                this.Close();
                            }
                            catch(DbUpdateException)
                            {
                                MessageBox.Show("Please make sure that entered subscriber Id or subscription Id " +
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
                }
            }
            else if (sender == btnCancel)
            {
                this.Close();
            }
        }
    }
}
