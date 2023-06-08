using System;
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
    /// Interaction logic for IdRecieverWindow.xaml
    /// </summary>
    public partial class IdRecieverWin : Window
    {
        private MainWindow thisMainWindow;
        private (byte, byte, int) ChangeDetails { get; set; } // details of the change type and the target for change
        public IdRecieverWin((byte, byte, int) details, MainWindow thisMainWindow)
        {
            this.thisMainWindow = thisMainWindow;
            ChangeDetails = details;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the IsKeyboardFocused event of the tbType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbId_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbId.IsKeyboardFocused == true)
            {
                idPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbId.IsKeyboardFocused && string.IsNullOrEmpty(tbId.Text))
            {
                idPlaceholder.Visibility = Visibility.Visible;
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
                if (string.IsNullOrEmpty(tbId.Text) || !int.TryParse(tbId.Text, out int id) || id < 1 || id > ChangeDetails.Item3 )
                {
                    MessageBox.Show("Please make sure that you entered existing ID",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Window win;
                    message = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            if (ChangeDetails.Item1 == 1)
                            {
                                switch (ChangeDetails.Item2)
                                {
                                    case 1:
                                        win = new UpdSubWin(id, thisMainWindow);
                                        win.Show();
                                        break;

                                    case 2:
                                        win = new UpdSubTypeWin(id, thisMainWindow);
                                        win.Show();
                                        break;

                                    case 3:
                                        win = new UpdSubSubscriptionWin(id, thisMainWindow);
                                        win.Show();
                                        break;

                                    case 4:
                                        win = new UpdPriceWin(id, thisMainWindow);
                                        win.Show();
                                        break;

                                    case 5:
                                        win = new UpdConfirmationWin(id, thisMainWindow);
                                        win.Show();
                                        break;
                                }
                                this.Close();
                            }
                            else if (ChangeDetails.Item1 == 2)
                            {
                                using (DbAppContext subs = new DbAppContext())
                                {
                                    switch (ChangeDetails.Item2)
                                    {
                                        case 1:
                                            var subToDelete = subs.Subscribers.
                                                SingleOrDefault(s => s.SubscriberId == id);
                                            subs.Subscribers.Remove(subToDelete);
                                            subs.SaveChanges();
                                            thisMainWindow.RefreshDataGrid();
                                            
                                        break;

                                        case 2:
                                            var typeToDelete = subs.SubscriptionTypes.
                                                SingleOrDefault(s => s.SubscriptionId == id);
                                            subs.SubscriptionTypes.Remove(typeToDelete);
                                            subs.SaveChanges();
                                            thisMainWindow.RefreshDataGrid();
                                            break;

                                        case 3:
                                            var subSubscriptionToDelete = subs.SubscribersSubscriptions.
                                                SingleOrDefault(s => s.SubscriberId == id);
                                            subs.SubscribersSubscriptions.Remove(subSubscriptionToDelete);
                                            subs.SaveChanges();
                                            thisMainWindow.RefreshDataGrid();
                                            break;

                                        case 4:
                                            var typePriceToDelete = subs.SubscriptionPrices.
                                                SingleOrDefault(s => s.SubscriptionId == id);
                                            subs.SubscriptionPrices.Remove(typePriceToDelete);
                                            subs.SaveChanges();
                                            thisMainWindow.RefreshDataGrid();
                                            break;

                                        case 5:
                                            var purchaseToDelete = subs.PurchaseConfirmations.
                                                SingleOrDefault(s => s.PurchaseId == id);
                                            subs.PurchaseConfirmations.Remove(purchaseToDelete);
                                            subs.SaveChanges();
                                            thisMainWindow.RefreshDataGrid();
                                            break;
                                    }
                                    this.Close();
                                }
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
