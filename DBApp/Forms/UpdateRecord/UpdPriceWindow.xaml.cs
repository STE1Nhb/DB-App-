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

namespace DBApp.Forms.UpdateRecord
{
    /// <summary>
    /// Represents a form with fields to be filled in, by which you can modify the record in subscription_prices table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class UpdPriceWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        private int TargetId { get; set; }
        public UpdPriceWin(int targetId, MainWindow thisMainWindow)
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
            if (tbPrice.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Makes the placeholder of tbPrice TextBox visible/invisible when keyboard focus changes.
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
                if (string.IsNullOrEmpty(tbPrice.Text))
                {
                    MessageBox.Show("Please fill at least one field to update.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    float price;
                    bool canConvert = tbPrice.Text.Contains(".") ? float.TryParse(tbPrice.Text.Replace(".", ","), out price) :
                        float.TryParse(tbPrice.Text.Replace(".", ","), out price);

                    if (canConvert)
                    {
                        using (var subs = new DbAppContext())
                        {
                            var subPrice = subs.SubscriptionPrices.SingleOrDefault(s => s.SubscriptionId == TargetId);
                            subPrice.Price = price;

                            subs.SaveChanges();
                            ThisMainWindow.RefreshDataGrid();
                            this.Close();
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
