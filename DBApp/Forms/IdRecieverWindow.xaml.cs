﻿using System;
using System.Collections.Generic;
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
using DBApp.Forms.UpdateRecord;


namespace DBApp.Forms
{
    /// <summary>
    /// Represents a form with field to be filled in, which transmits the Id of the modified record.
    /// </summary>
    public partial class IdRecieverWin : Window
    {
        private MainWindow thisMainWindow;
        private (byte, byte) ChangeDetails { get; set; } // details of the change type and the target for change
        public IdRecieverWin((byte, byte) details, MainWindow thisMainWindow)
        {
            this.thisMainWindow = thisMainWindow;
            ChangeDetails = details;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ChangeDetails.Item2 == 1 || ChangeDetails.Item2 == 3)
                tblockRecord.Text = "Subscriber ID";
            else if (ChangeDetails.Item2 == 2 || ChangeDetails.Item2 == 4)
                tblockRecord.Text = "Subscription ID";
            else
                tblockRecord.Text = "Purchase ID";
        }

        /// <summary>
        /// Makes the placeholder of TextBoxes visible on mouse click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbId.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Makes the placeholder of tbId TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbId_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbId.IsKeyboardFocused)
            {
                idPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbId.IsKeyboardFocused && string.IsNullOrEmpty(tbId.Text))
            {
                idPlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Cancels the changes on btnCancel button click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrDelete(sender);
        }

        /// <summary>
        /// Transmits the Id of the modified target to one of the Update forms.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            UpdateOrDelete(sender);
        }


        /// <summary>
        /// Transmits data to the update window or deletes the record based on the ChangeDetails variable value. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void UpdateOrDelete(object sender)
        {
            if (sender == btnOk)
            {
                using (DbAppContext subs = new DbAppContext())
                {
                    bool contains;

                    if (string.IsNullOrEmpty(tbId.Text) || !int.TryParse(tbId.Text, out int id) || id < 1)
                    {
                        MessageBox.Show("Please make sure that you enter existing ID",
                            "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        Window win;
                        if (ChangeDetails.Item1 == 1)
                        {
                            try
                            {
                                switch (ChangeDetails.Item2)
                                {
                                    case 1:
                                        contains = subs.Subscribers.AsEnumerable().Any(row => id == row.SubscriberId);
                                        if (contains)
                                        {
                                            win = new UpdSubWin(id, thisMainWindow);
                                            win.Show();
                                            tbId.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that you enter existing ID",
                                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                        break;

                                    case 2:
                                        contains = subs.SubscriptionTypes.AsEnumerable().Any(row => id == row.SubscriptionId);
                                        if (contains)
                                        {
                                            win = new UpdSubTypeWin(id, thisMainWindow);
                                            win.Show();
                                            tbId.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that you enter existing ID",
                                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                        break;

                                    case 3:
                                        contains = subs.SubscribersSubscriptions.AsEnumerable().Any(row => id == row.SubscriberId);
                                        if (contains)
                                        {
                                            win = new UpdSubSubscriptionWin(id, thisMainWindow);
                                            win.Show();
                                            tbId.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that you enter existing ID",
                                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                        break;

                                    case 4:
                                        contains = subs.SubscriptionPrices.AsEnumerable().Any(row => id == row.SubscriptionId);
                                        if (contains)
                                        {
                                            win = new UpdPriceWin(id, thisMainWindow);
                                            win.Show();
                                            tbId.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that you enter existing ID",
                                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                        break;

                                    case 5:
                                        contains = subs.PurchaseConfirmations.AsEnumerable().Any(row => id == row.PurchaseId);
                                        if (contains)
                                        {
                                            win = new UpdConfirmationWin(id, thisMainWindow);
                                            win.Show();
                                            tbId.Clear();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that you enter existing ID",
                                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }    
                                        break;
                                }
                            }
                            catch (ArgumentNullException)
                            {
                                MessageBox.Show("Please make sure that you enter existing ID",
                                    "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else if (ChangeDetails.Item1 == 2)
                        {
                            try
                            {
                                switch (ChangeDetails.Item2)
                                {
                                    case 1:
                                        var subToDelete = subs.Subscribers.
                                            SingleOrDefault(s => s.SubscriberId == id);
                                        subs.Subscribers.Remove(subToDelete);
                                        subs.SaveChanges();
                                        thisMainWindow.RefreshDataGrid();
                                        tbId.Clear();
                                        break;

                                    case 2:
                                        var typeToDelete = subs.SubscriptionTypes.
                                            SingleOrDefault(s => s.SubscriptionId == id);
                                        subs.SubscriptionTypes.Remove(typeToDelete);
                                        subs.SaveChanges();
                                        thisMainWindow.RefreshDataGrid();
                                        tbId.Clear();
                                        break;

                                    case 3:
                                        var subSubscriptionToDelete = subs.SubscribersSubscriptions.
                                            SingleOrDefault(s => s.SubscriberId == id);
                                        subs.SubscribersSubscriptions.Remove(subSubscriptionToDelete);
                                        subs.SaveChanges();
                                        thisMainWindow.RefreshDataGrid();
                                        tbId.Clear();
                                        break;

                                    case 4:
                                        var typePriceToDelete = subs.SubscriptionPrices.
                                            SingleOrDefault(s => s.SubscriptionId == id);
                                        subs.SubscriptionPrices.Remove(typePriceToDelete);
                                        subs.SaveChanges();
                                        thisMainWindow.RefreshDataGrid();
                                        tbId.Clear();
                                        break;

                                    case 5:
                                        var purchaseToDelete = subs.PurchaseConfirmations.
                                            SingleOrDefault(s => s.PurchaseId == id);
                                        subs.PurchaseConfirmations.Remove(purchaseToDelete);
                                        subs.SaveChanges();
                                        thisMainWindow.RefreshDataGrid();
                                        tbId.Clear();
                                        break;
                                }
                                this.Close();
                            }
                            catch (ArgumentNullException)
                            {
                                MessageBox.Show("Please make sure that you enter existing ID",
                                    "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
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
