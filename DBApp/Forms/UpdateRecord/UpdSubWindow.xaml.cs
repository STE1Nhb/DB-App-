using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for UpdSubWindow.xaml
    /// </summary>
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

        private void tbEmail_IsKeyboardFocused(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbEmail.IsKeyboardFocused == true)
            {
                emailPlaceholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbEmail.IsKeyboardFocused && string.IsNullOrEmpty(tbEmail.Text))
            {
                emailPlaceholder.Visibility = Visibility.Visible;
            }
        }

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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxHandler(sender);
        }

        private void MessageBoxHandler(object sender)
        {
            MessageBoxResult message;
            if (sender == btnOk)
            {
                if (string.IsNullOrEmpty(tbDate.Text) && string.IsNullOrEmpty(tbEmail.Text))
                {
                    MessageBox.Show("Please fill at least one field to update.",
                        "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    message = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (message)
                    {
                        case MessageBoxResult.Yes:
                            DateTime date = DateTime.Now;

                            if ( (String.IsNullOrEmpty(tbEmail.Text) || (tbEmail.Text.Contains("@") && tbEmail.Text.Length < 50) ) && 
                                (String.IsNullOrEmpty(tbDate.Text) || DateTime.TryParse(tbDate.Text.Trim(), out date)) )
                            {
                                using (var subs = new DbAppContext())
                                {
                                    var sub = subs.Subscribers.SingleOrDefault(s => s.SubscriberId == TargetId);
                                    sub.Email = String.IsNullOrEmpty(tbEmail.Text) ? sub.Email : tbEmail.Text.Trim();
                                    sub.BirthDate = String.IsNullOrEmpty(tbDate.Text) ? sub.BirthDate : date;

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
