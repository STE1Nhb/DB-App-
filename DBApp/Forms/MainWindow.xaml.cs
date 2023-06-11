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
using System.Windows.Data;
using System.Windows.Media;
using DBApp.Forms.NewRecord;

namespace DBApp.Forms
{
    public partial class MainWindow : Window
    {
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
            GridDataSourceUpdate();
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
                    win = new AddSubWin(this);
                    win.Show();
                    break;
                case "Subscription Types":
                    win = new AddSubscriptionWin(this);
                    win.Show();
                    break;
                case "Subscribers Subscriptions":
                    win = new AddSubSubscriptionWin(this);
                    win.Show();
                    break;
                case "Subscription Prices":
                    win = new AddPriceWin(this);
                    win.Show();
                    break;
                case "Purchase Confirmations":
                    win = new AddConfirmationWin(this);
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
                "Subscribers", "Subscription Types","Subscribers Subscriptions",
            "Subscription Prices", "Purchase Confirmations", "Expiry Dates" };

            tableList.ItemsSource = items;
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            Window win;
            DbAppContext appContext = new DbAppContext();
            (byte, byte) details;

            switch ((string)tableList.SelectedItem)
            {
                case "Subscribers":
                    details = (1, 1);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Subscription Types":
                    details = (1, 2);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Subscribers Subscriptions":
                    details = (1, 3);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                    //MessageBox.Show("You cannot update this table.\nBut if you made a mistake you can delete a record ;)", 
                    //    "Woops!", MessageBoxButton.OK,MessageBoxImage.Information);
                break;

                case "Subscription Prices":
                    details = (1, 4);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Purchase Confirmations":
                    details = (1, 5);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Expiry Dates":
                    MessageBox.Show("This table is auto-updateable, so let us do this job for you ;)", 
                        "Wait a minute", MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            Window win;
            DbAppContext appContext = new DbAppContext();
            (byte, byte) details;

            switch ((string)tableList.SelectedItem)
            {
                case "Subscribers":
                    details = (2, 1);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Subscription Types":
                    details = (2, 2);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Subscribers Subscriptions":
                    details = (2, 3);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                    break;

                case "Subscription Prices":
                    details = (2, 4);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;

                case "Purchase Confirmations":
                    details = (2, 5);
                    win = new IdRecieverWin(details, this);
                    win.Show();
                break;
            }
        }

        private void GridDataSourceUpdate()
        {
            string dbCon = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            string cmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(dbCon))
            {
                switch ((string)tableList.SelectedItem)
                {
                    case "Subscribers":
                        cmdString = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers";
                        SqlCommand cmd = new SqlCommand(cmdString, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable("Subscribers");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscriber Id";
                        tableContent.Columns[1].Header = "Subscriber email";
                        tableContent.Columns[2].Header = "Subscriber birth date";
                    break;

                    case "Subscription Types":
                        cmdString = "SELECT type_id, subscription_type FROM subscription_types";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("Subscription types");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscription Id";
                        tableContent.Columns[1].Header = "Subscription title";
                    break;

                    case "Subscribers Subscriptions":
                        cmdString = "SELECT subscriber_id, type_id FROM subscribers_subscriptions";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("Subscribers Subscriptions");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscriber Id";
                        tableContent.Columns[1].Header = "Subscription Id";
                    break;

                    case "Subscription Prices":
                        cmdString = "SELECT type_id, subscription_price FROM subscription_prices";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("subscription_prices");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscription Id";
                        tableContent.Columns[1].Header = "Subscription price";
                    break;

                    case "Purchase Confirmations":
                        cmdString = "SELECT purchase_id, subscriber_id, type_id, subscription_price, CONVERT(varchar(10),purchase_date, 103) FROM purchase_confirmations";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("purchase_confirmations");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Purchase Id";
                        tableContent.Columns[1].Header = "Subscriber Id";
                        tableContent.Columns[2].Header = "Subscription Id";
                        tableContent.Columns[3].Header = "Subscription Price";
                        tableContent.Columns[4].Header = "purchase_date";
                    break;

                    case "Expiry Dates":
                        cmdString = "SELECT purchase_id, CONVERT(varchar(10),expiry_date, 103) FROM expiry_dates";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("expiry_dates");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Purchase Id";
                        tableContent.Columns[1].Header = "Expiry Date";
                    break;

                    default:
                        cmdString = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers";
                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("subscribers");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                    break;
                }
            }
        }
        public void RefreshDataGrid()
        {
            GridDataSourceUpdate();
        }
    }
}
