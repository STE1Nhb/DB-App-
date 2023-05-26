using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DBApp.Forms.NewRecord;

namespace DBApp.Forms
{
    public partial class MainWindow : Window
    {
        DbAppContext subs = new DbAppContext();
        List<ComboBoxItem> itemsToAdd = new List<ComboBoxItem>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the tableList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void tableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string dbCon = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            string cmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(dbCon))
            {
                switch((string)tableList.SelectedItem)
                {
                    case "Subscribers":
                        cmdString = "SELECT SubscriberId, Email, CONVERT(varchar(10),BirthDate, 103)FROM Subscribers";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable("Subscribers");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    case "SubscriptionTypes":
                        cmdString = "SELECT SubscriptionId, Type FROM SubscriptionTypes";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("SubscriptionTypes");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    case "SubscribersSubscriptions":
                        cmdString = "SELECT SubscriberId, SubscriptionId FROM SubscriberSubscriptions";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("SubscriberSubscriptions");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    case "SubscriptionPrices":
                        cmdString = "SELECT SubscriptionId, Price FROM SubscriptionPrices";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("SubscriptionPrices");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    case "PurchaseConfirmations":
                        cmdString = "SELECT PurchaseId, SubscriberId, SubscriptionId, PurchaseDate FROM PurchaseConfirmations";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("PurchaseConformations");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    case "ExpirationDates":
                        cmdString = "SELECT PurchaseId, Date FROM ExpirationDates";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("ExpirationDates");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                    default:
                        cmdString = "SELECT SubscriberId, Email, BirthDate FROM Subscribers";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("Subscribers");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window();
            switch((string)tableList.SelectedItem) 
            {
                case "Subscribers":
                    win = new AddSubWin();
                    win.Show();
                    break;
                case "SubscriptionTypes":
                    win = new AddSubscriptionWin();
                    win.Show();
                    break;
                case "SubscribersSubscriptions":
                    win = new AddSubSubscriptionWin();
                    win.Show();
                    break;
                case "SubscriptionPrices":
                    win = new AddPriceWin();
                    win.Show();
                    break;
                case "PurchaseConfirmations":
                    win = new AddConfirmationWin();
                    win.Show();
                    break;
            }
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>() { 
                "Subscribers", "SubscriptionTypes","SubscribersSubscriptions",
            "SubscriptionPrices", "PurchaseConfirmations", "ExpirationDates" };

            tableList.ItemsSource = items;
        }
    }
}
