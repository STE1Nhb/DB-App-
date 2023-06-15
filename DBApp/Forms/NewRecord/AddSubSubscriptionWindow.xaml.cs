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

namespace DBApp.Forms.NewRecord
{
    /// <summary>
    /// Represents a form with fields to be filled in, by which you can add data to the subsrcibers_subscriptions table.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class AddSubSubscriptionWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        public AddSubSubscriptionWin(MainWindow thisMainWindow)
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
            if (tbType.IsKeyboardFocused || tbSub.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Makes the placeholder of tbSub TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbSub_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbSub.IsKeyboardFocused)
            {
                subPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbSub.IsKeyboardFocused && string.IsNullOrEmpty(tbSub.Text))
            {
                subPlaceholder.Visibility = Visibility.Visible;
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
            MessageBoxHandler(sender);
        }

        /// <summary>
        /// Adds a record to the table on btnOk click.
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
                if ((string.IsNullOrEmpty(tbSub.Text) || string.IsNullOrEmpty(tbType.Text)))
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
                            if (int.TryParse(tbSub.Text.Trim(), out int sub) == true && int.TryParse(tbType.Text.Trim(), out int type))
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var subSubscription = new SubscriberSubscription() { SubscriberId = sub, SubscriptionId = type };
                                    subs.SubscribersSubscriptions.Add(subSubscription);

                                    try
                                    {
                                        subs.SaveChanges();
                                        ThisMainWindow.RefreshDataGrid();
                                        ClearFields();
                                    }
                                    catch(DbUpdateException)
                                    {
                                        MessageBox.Show("Please make sure that entered Subscriber Id or Subscription Id " +
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

        /// <summary>
        /// Clears the fields of TextBoxes.
        /// </summary>
        private void ClearFields()
        {
            tbSub.Clear();
            tbType.Clear();
        }
    }
}
