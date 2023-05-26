using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
    /// <summary>
    /// Interaction logic for AddConfirmationWindow.xaml
    /// </summary>
    public partial class AddConfirmationWin : Window
    {
        public AddConfirmationWin()
        {
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
        /// Handles the IsKeyboardFocusedChanged event of the tbPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbPrice_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbPrice.IsKeyboardFocused == true)
            {
                pricePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbPrice.IsKeyboardFocused && string.IsNullOrEmpty(tbPrice.Text))
            {
                pricePlaceholder.Visibility = Visibility.Visible;
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
                if (string.IsNullOrEmpty(tbSub.Text) || string.IsNullOrEmpty(tbType.Text) || 
                    string.IsNullOrEmpty(tbPrice.Text) || string.IsNullOrEmpty(tbDate.Text))
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
                            if (int.TryParse(tbSub.Text.Trim(), out int sub) == true && int.TryParse(tbType.Text.Trim(), out int type) &&
                                int.TryParse(tbPrice.Text.Trim(), out int price) && DateTime.TryParse(tbDate.Text.Trim(), out DateTime date))
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var purchase = new PurchaseConfirmation() { SubscriberId = sub, SubscriptionId = type, Price = price, PurchaseDate = date };
                                    subs.PurchaseConfirmations.Add(purchase);
                                    subs.SaveChanges();
                                   

                                    var idString = "SELECT MAX(PurchaseId) FROM PurchaseConfirmations";
                                    var dateString = "DATEADD(month, 1, PurchaseDate) FROM PurchaseConfirmations";
                                    string dbCon = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                                    SqlConnection con = new SqlConnection(dbCon);
                                    var lastId = Convert.ToInt32(new SqlCommand(idString, con));
                                    var futureDate = Convert.ToDateTime(new SqlCommand(dateString, con));
                                    var expDate = new ExpirationDate() {PurchaseId = lastId, Date = futureDate };
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
