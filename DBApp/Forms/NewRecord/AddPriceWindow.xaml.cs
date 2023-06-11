using System;
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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace DBApp.Forms.NewRecord
{
    public partial class AddPriceWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        public AddPriceWin(MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            InitializeComponent();
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
        /// Handles the IsKeyboardFocused event of the tbPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbPrice_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
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
        /// Messages the user of an attempt to perform an action
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void MessageBoxHandler(object sender)
        {
            MessageBoxResult message;
            if (sender == btnOk)
            {
                if ((string.IsNullOrEmpty(tbType.Text) || string.IsNullOrEmpty(tbPrice.Text)))
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
                            float price;
                            bool canConvert = tbPrice.Text.Contains(".") ? float.TryParse(tbPrice.Text.Replace(".", ","), out price) :
                                float.TryParse(tbPrice.Text.Replace(".", ","), out price);

                            if (int.TryParse(tbType.Text.Trim(), out int type) == true && canConvert && price > 0)
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var subPrice = new SubscriptionPrice() { SubscriptionId = type, Price = price };
                                    subs.SubscriptionPrices.Add(subPrice);

                                    try
                                    {
                                        subs.SaveChanges();
                                        ThisMainWindow.RefreshDataGrid();
                                        ClearFields();
                                    }
                                    catch(DbUpdateException)
                                    {
                                        MessageBox.Show("Please make sure that entered Subscription Id " +
                                             "is existing one.", "Something went wrong",
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
            tbType.Clear();
            tbPrice.Clear();
        }
    }
}
