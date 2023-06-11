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
    /// Interaction logic for UpdSubType.xaml
    /// </summary>
    public partial class UpdSubTypeWin : Window
    {
        private MainWindow ThisMainWindow { get; set; }
        private int TargetId { get; set; }
        public UpdSubTypeWin(int targetId, MainWindow thisMainWindow)
        {
            ThisMainWindow = thisMainWindow;
            TargetId = targetId;
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
                if (string.IsNullOrEmpty(tbType.Text))
                {
                    MessageBox.Show("Please fill at least one field to update.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if(tbType.Text.Length > 30)
                {
                    MessageBox.Show("Please make sure that title length does not exceed 30 characters.",
                       "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    message = MessageBox.Show("All entered data will replace the existing!", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            using (var subs = new DbAppContext())
                            {
                                var type = subs.SubscriptionTypes.SingleOrDefault(s => s.SubscriptionId == TargetId);
                                type.Type = tbType.Text;

                                subs.SaveChanges();
                                ThisMainWindow.RefreshDataGrid();
                                this.Close();
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
    }
}
