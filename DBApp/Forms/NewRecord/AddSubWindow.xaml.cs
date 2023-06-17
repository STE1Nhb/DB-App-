using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Represents a form with fields to be filled in, by which you can add data to the subscribers table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class AddSubWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        public AddSubWin(MainWindow thisMainWindow)
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
            if(tbEmail.IsKeyboardFocused) 
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
            if (tbDate.IsKeyboardFocused)
            {
                datePlaceholder.Visibility = Visibility.Collapsed;
            }
            else if(!tbDate.IsKeyboardFocused && string.IsNullOrEmpty(tbDate.Text))
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
        /// Adds ne record to the table.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void AddNewRecord(object sender)
        {
            if (sender == btnOk)
            {
                if ((string.IsNullOrEmpty(tbDate.Text) || string.IsNullOrEmpty(tbEmail.Text)))
                {
                    MessageBox.Show("Please fill all available fields.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if(tbEmail.Text.Length > 50)
                {
                    MessageBox.Show("Please make sure that e-mail length does not exceed 50 characters.",
                       "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (DateTime.TryParse(tbDate.Text.Trim(), out DateTime date) == true && tbEmail.Text.Contains('@'))
                    {
                        using (var subs = new DbAppContext())
                        {
                            var sub = new Subscriber() { Email = tbEmail.Text.Trim(), BirthDate = date };

                            subs.Subscribers.Add(sub);

                            subs.SaveChanges();
                            ThisMainWindow.RefreshDataGrid();
                            ClearFields();
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
            tbEmail.Clear();
            tbDate.Clear();
        }
    }
}
