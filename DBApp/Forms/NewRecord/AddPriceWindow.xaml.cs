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
    /// <summary>
    /// Represents a form with fields to be filled in, by which you can add data to the subscription_prices table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class AddPriceWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        public AddPriceWin(MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// Makes the placeholder of TextBoxes visible on mouse click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbType.IsKeyboardFocused || tbPrice.IsKeyboardFocused)
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
        /// Makes the placeholder of tbType TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbPrice_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbPrice.IsKeyboardFocused)
            {
                pricePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbPrice.IsKeyboardFocused && string.IsNullOrEmpty(tbPrice.Text))
            {
                pricePlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Cancels the changes on btnCancel button click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecord(sender);
        }

        /// <summary>
        /// Adds a record to the table on btnOk click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            AddNewRecord(sender);
        }

        /// <summary>
        /// Adds new record to the table.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void AddNewRecord(object sender)
        {
            if (sender == btnOk)
            {
                if ((string.IsNullOrEmpty(tbType.Text) || string.IsNullOrEmpty(tbPrice.Text)))
                {
                    MessageBox.Show("Please fill all available fields.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
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
                }
            }
            else if (sender == btnCancel)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Clears the fields of TextBoxes.
        /// </summary>
        private void ClearFields()
        {
            tbType.Clear();
            tbPrice.Clear();
        }
    }
}
