using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UpdConfirmsWindow.xaml
    /// </summary>
    public partial class UpdConfirmationWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        private int TargetId { get; set; }
        public UpdConfirmationWin(int targetId, MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            TargetId = targetId;
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
        /// Handles the IsKeyboardFocusedChanged event of the tbDate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbDate_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbDate.IsKeyboardFocused == true)
            {
                datePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbDate.IsKeyboardFocused && string.IsNullOrEmpty(tbDate.Text))
            {
                datePlaceholder.Visibility = Visibility.Visible;
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
                if (string.IsNullOrEmpty(tbSub.Text) && string.IsNullOrEmpty(tbType.Text) &&
                    string.IsNullOrEmpty(tbDate.Text))
                {
                    MessageBox.Show("Please fill at least one field to update.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    message = MessageBox.Show("All entered data will replace the existing!", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            int sub = 0;
                            int type = 0;
                            DateTime date = DateTime.Now;

                            if ( (String.IsNullOrEmpty(tbSub.Text) || int.TryParse(tbSub.Text.Trim(), out sub) == true) && 
                                (String.IsNullOrEmpty(tbType.Text) || int.TryParse(tbType.Text.Trim(), out type) == true) &&
                                (String.IsNullOrEmpty(tbDate.Text) || DateTime.TryParse(tbDate.Text.Trim(), out date) == true) )
                            {
                                using (var subs = new DbAppContext())
                                {
                                    try
                                    {
                                        bool contains;
                                        contains = subs.SubscribersSubscriptions.AsEnumerable().Any(row => sub == row.SubscriberId
                                        && type == row.SubscriptionId);
                                        if (contains)
                                        {
                                            var confirm = subs.PurchaseConfirmations.SingleOrDefault(s => s.PurchaseId == TargetId);
                                            var price = (float)
                                                (
                                                    subs.SubscriptionPrices
                                                    .Where(id => type == 0 ? id.SubscriptionId == confirm.SubscriptionId : id.SubscriptionId == type)
                                                    .Select(p => p.Price)
                                                    .First()
                                                );

                                            //var purchase = new PurchaseConfirmation() { SubscriberId = sub, SubscriptionId = type, Price = price, PurchaseDate = date };
                                            //subs.PurchaseConfirmations.Add(purchase);

                                            //subs.SaveChanges();
                                            //ThisMainWindow.RefreshDataGrid();
                                            //ClearFields();

                                            confirm.SubscriberId = String.IsNullOrEmpty(tbSub.Text) ? confirm.SubscriberId : sub;
                                            confirm.SubscriptionId = String.IsNullOrEmpty(tbType.Text) ? confirm.SubscriptionId : type;
                                            confirm.PurchaseDate = String.IsNullOrEmpty(tbDate.Text) ? confirm.PurchaseDate : date;
                                            confirm.Price = price;

                                            //try
                                            //{
                                                subs.SaveChanges();
                                                ExpirationDateCount();
                                                ThisMainWindow.RefreshDataGrid();
                                                this.Close();
                                            //}
                                            //catch (DbUpdateException)
                                            //{
                                            //    MessageBox.Show("Please make sure that subscription type of this subscriber matches up " +
                                            //        "with subscription type in Subscribers Subscriptions table.", "Something went wrong",
                                            //        MessageBoxButton.OK, MessageBoxImage.Error);
                                            //}
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please make sure that entered Subscriber/Subscription Id is existing one or Subscription type of this Subscriber matches up " +
                                                    "with the Subscription type in Subscribers Subscriptions table.", "Something went wrong",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                    }
                                    catch (InvalidOperationException)
                                    {
                                        MessageBox.Show("Please make sure that entered Subscription Id has a price.", "Something went wrong",
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
        private void ExpirationDateCount()
        {
            if (!String.IsNullOrEmpty(tbDate.Text))
            {
                using (var subs = new DbAppContext())
                {
                        var expDate = subs.ExpirationDates.SingleOrDefault(s => s.PurchaseId == TargetId);

                        var futureDate = Convert.ToDateTime
                            (
                                subs.PurchaseConfirmations
                                .Where(id => id.PurchaseId == TargetId)
                                .Select(date => date.PurchaseDate)
                                .First()
                            )
                            .AddMonths(1);

                        expDate.ExpiryDate = futureDate;
                        subs.SaveChanges();
                }
            }
        }
    }
}
