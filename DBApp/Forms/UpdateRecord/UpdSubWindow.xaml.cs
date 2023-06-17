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
    /// Represents a form with fields to be filled in, by which you can modify the record in subscribers table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class UpdSubWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        private int TargetId { get; set; }
        public UpdSubWin(int targetId, MainWindow thisMainWindow)
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
            if (tbEmail.IsKeyboardFocused || tbDate.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Makes the placeholder of tbEmail TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbEmail_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbEmail.IsKeyboardFocused)
            {
                emailPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbEmail.IsKeyboardFocused && string.IsNullOrEmpty(tbEmail.Text))
            {
                emailPlaceholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Makes the placeholder of tbDate TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbDate_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
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
                if (string.IsNullOrEmpty(tbDate.Text) && string.IsNullOrEmpty(tbEmail.Text))
                {
                    MessageBox.Show("Please fill at least one field to update.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if(tbEmail.Text.Length > 50)
                {
                    MessageBox.Show("Please make sure that e-mail length does not exceed 50 characters.",
                       "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    DateTime date = DateTime.Now;

                    if ( (String.IsNullOrEmpty(tbEmail.Text) || (tbEmail.Text.Contains("@") && tbEmail.Text.Length < 50) ) && 
                        (String.IsNullOrEmpty(tbDate.Text) || DateTime.TryParse(tbDate.Text.Trim(), out date)) )
                    {
                        using (var subs = new DbAppContext())
                        {
                            var sub = subs.Subscribers.SingleOrDefault(s => s.SubscriberId == TargetId);
                            sub.Email = String.IsNullOrEmpty(tbEmail.Text) ? sub.Email : tbEmail.Text.Trim();
                            sub.BirthDate = String.IsNullOrEmpty(tbDate.Text) ? sub.BirthDate : date;

                            try
                            {
                                subs.SaveChanges();
                                ThisMainWindow.RefreshDataGrid();
                                this.Close();
                            }
                            catch(DbUpdateException) 
                            {
                                MessageBox.Show("Please make sure that all fields are filled out in the right way.",
                                    "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
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
