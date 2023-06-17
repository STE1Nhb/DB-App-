using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using DBApp.Forms.NewRecord;

namespace DBApp.Forms
{
    /// <summary>
    /// Represents a form, by which you can get access to all app functionality.
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
        private byte selectedSearchFilter;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads selection items to tableList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>() {
                "Subscribers", "Subscription Types","Subscribers Subscriptions",
            "Subscription Prices", "Purchase Confirmations", "Expiry Dates" };

            tableList.ItemsSource = items;
            tableList.SelectedItem = items[0];
        }

        /// <summary>
        /// Makes the placeholder of tbSearch TextBox visible on mouse click.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbSearch.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        /// <summary>
        /// Changes the data source of the data grid when selected item of th tableList control changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void tableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridDataSourceUpdate();
        }

        /// <summary>
        /// Transmits the data about the table that will be modified to one of the Add forms.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var win = new Window();
            switch ((string)tableList.SelectedItem)
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
                case "Expiry Dates":
                    MessageBox.Show("There is no need to add a record to this table manually, as it fills in automatically when a new Purchase Confirmation record appears.",
                       "Wait a minute", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        /// <summary>
        /// Transmits the data about the table that will be modified to IdReciever form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
                    MessageBox.Show("There is no need to update a record of this table manually, as it updates automatically when a Purchase Confirmation record is updated.",
                        "Wait a minute", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        /// <summary>
        /// Transmits the data about the table that will be modified to IdReciever form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

                case "Expiry Dates":
                    MessageBox.Show("There is no need to delete a record from this table manually, as it deletes automatically when a Purchase Confirmation record is deleted.",
                       "Wait a minute", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        /// <summary>
        /// Updates data source of the data grid.
        /// </summary>
        private void GridDataSourceUpdate()
        {
            string dbCon = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            string cmdString = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter sda;
            DataTable dt;
            bool searchFocused = tbSearch.IsFocused;
            using (SqlConnection con = new SqlConnection(dbCon))
            {
                switch ((string)tableList.SelectedItem)
                {
                    case "Subscribers":

                        if (!searchFocused)
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }
                        try
                        {
                            cmd = new SqlCommand(cmdString, con);
                            sda = new SqlDataAdapter(cmd);
                            dt = new DataTable("Subscribers");
                            sda.Fill(dt);
                            tableContent.ItemsSource = dt.DefaultView;
                            tableContent.Columns[0].Header = "Subscriber Id";
                            tableContent.Columns[1].Header = "Subscriber email";
                            tableContent.Columns[2].Header = "Subscriber birth date";
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("Make sure that you enter the date in the right way.", 
                                "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case "Subscription Types":
                        if (!searchFocused)
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT type_id, subscription_type FROM subscription_types";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }

                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("Subscription types");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscription Id";
                        tableContent.Columns[1].Header = "Subscription title";
                        break;

                    case "Subscribers Subscriptions":
                        if (!searchFocused) 
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT subscriber_id, type_id FROM subscribers_subscriptions";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }

                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("Subscribers Subscriptions");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscriber Id";
                        tableContent.Columns[1].Header = "Subscription Id";
                        break;

                    case "Subscription Prices":
                        if(!searchFocused) 
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT type_id, subscription_price FROM subscription_prices";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }

                        cmd = new SqlCommand(cmdString, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable("subscription_prices");
                        sda.Fill(dt);
                        tableContent.ItemsSource = dt.DefaultView;
                        tableContent.Columns[0].Header = "Subscription Id";
                        tableContent.Columns[1].Header = "Subscription Price ($)";
                        break;

                    case "Purchase Confirmations":
                        if(!searchFocused) 
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT purchase_id, subscriber_id, type_id, subscription_price, CONVERT(varchar(10),purchase_date, 103) FROM purchase_confirmations";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }

                        try
                        {

                            cmd = new SqlCommand(cmdString, con);
                            sda = new SqlDataAdapter(cmd);
                            dt = new DataTable("purchase_confirmations");
                            sda.Fill(dt);
                            tableContent.ItemsSource = dt.DefaultView;
                            tableContent.Columns[0].Header = "Purchase Id";
                            tableContent.Columns[1].Header = "Subscriber Id";
                            tableContent.Columns[2].Header = "Subscription Id";
                            tableContent.Columns[3].Header = "Subscription Price ($)";
                            tableContent.Columns[4].Header = "Purchase Date";
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("Make sure that you enter the date in the right way.",
                               "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case "Expiry Dates":
                        if(!searchFocused)
                        {
                            selectedSearchFilter = 0;
                            tbSearch.Clear();
                            tbSearch_Placeholder.Visibility = Visibility.Visible;
                            cmdString = "SELECT purchase_id, CONVERT(varchar(10),expiry_date, 103) FROM expiry_dates";
                        }
                        else
                        {
                            cmdString = ChangeCmdString();
                        }
                        try
                        {
                            cmd = new SqlCommand(cmdString, con);
                            sda = new SqlDataAdapter(cmd);
                            dt = new DataTable("expiry_dates");
                            sda.Fill(dt);
                            tableContent.ItemsSource = dt.DefaultView;
                            tableContent.Columns[0].Header = "Purchase Id";
                            tableContent.Columns[1].Header = "Expiry Date";
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("Make sure that you enter the date in the right way.",
                               "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;
                }
                ContextMenuButtonsUpdate();
            }
        }

        /// <summary>
        /// Updates context menu of tbSearch control.
        /// </summary>
        private void ContextMenuButtonsUpdate()
        {
            switch (tableList.SelectedItem)
            {
                case "Subscribers":
                    searchFilterOne.Header = "Subscriber ID";
                    searchFilterTwo.Header = "E-Mail";
                    searchFilterThree.Header = "Birth Date";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Visible;
                    searchFilterFour.Visibility = Visibility.Collapsed;
                    searchFilterFive.Visibility = Visibility.Collapsed;
                    break;

                case "Subscription Types":
                    searchFilterOne.Header = "Subscription ID";
                    searchFilterTwo.Header = "Subscription Title";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Collapsed;
                    searchFilterFour.Visibility = Visibility.Collapsed;
                    searchFilterFive.Visibility = Visibility.Collapsed;
                    break;

                case "Subscribers Subscriptions":
                    searchFilterOne.Header = "Subscriber ID";
                    searchFilterTwo.Header = "Subscription ID";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Collapsed;
                    searchFilterFour.Visibility = Visibility.Collapsed;
                    searchFilterFive.Visibility = Visibility.Collapsed;
                    break;

                case "Subscription Prices":
                    searchFilterOne.Header = "Subscription ID";
                    searchFilterTwo.Header = "Subscription Price";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Collapsed;
                    searchFilterFour.Visibility = Visibility.Collapsed;
                    searchFilterFive.Visibility = Visibility.Collapsed;
                    break;

                case "Purchase Confirmations":
                    searchFilterOne.Header = "Purchase ID";
                    searchFilterTwo.Header = "Subscriber ID";
                    searchFilterThree.Header = "Subscription ID";
                    searchFilterFour.Header = "Subscription Price";
                    searchFilterFive.Header = "Purchase Date";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Visible;
                    searchFilterFour.Visibility = Visibility.Visible;
                    searchFilterFive.Visibility = Visibility.Visible;
                    break;

                case "Expiry Dates":
                    searchFilterOne.Header = "Purchase ID";
                    searchFilterTwo.Header = "Expiry Date";

                    searchFilterOne.Visibility = Visibility.Visible;
                    searchFilterTwo.Visibility = Visibility.Visible;
                    searchFilterThree.Visibility = Visibility.Collapsed;
                    searchFilterFour.Visibility = Visibility.Collapsed;
                    searchFilterFive.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Changes the command string to update the data source of the data grid based on the selected search filter.
        /// </summary>
        private string ChangeCmdString() 
        {
            string cmd = string.Empty;
            switch (tableList.SelectedItem)
            {
                case "Subscribers":
                    if(selectedSearchFilter == 1)
                    {
                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                            $"WHERE subscriber_id LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscriber_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if(selectedSearchFilter == 2)
                    {
                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                            $"WHERE subscriber_email LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_email LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscriber_email LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 3)
                    {
                        try
                        {
                           string[] date = new string[3];
                            if (tbSearch.Text.Trim().Length == 10)
                            {
                                if (tbSearch.Text.Trim().Contains('.'))
                                {
                                    date = tbSearch.Text.Split('.');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = subscriber_birth_date";
                                    else
                                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = subscriber_birth_date";

                                }
                                else if (tbSearch.Text.Trim().Contains('/'))
                                {
                                    date = tbSearch.Text.Split('/');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = subscriber_birth_date";
                                    else
                                        cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = subscriber_birth_date";
                                }
                            }
                            else
                            {
                                cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                $"WHERE subscriber_birth_date LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_birth_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR subscriber_birth_date LIKE '%{tbSearch.Text.Trim()}'";
                            }
                        }
                        catch(IndexOutOfRangeException)
                        {
                            cmd = "SELECT subscriber_id, subscriber_email, CONVERT(varchar(10),subscriber_birth_date, 103) FROM subscribers " +
                                $"WHERE subscriber_birth_date LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_birth_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR subscriber_birth_date LIKE '%{tbSearch.Text.Trim()}'";
                        }
                    }
                    break;

                case "Subscription Types":
                    if (selectedSearchFilter == 1)
                    {
                        cmd = "SELECT type_id, subscription_type FROM subscription_types " +
                            $"WHERE type_id LIKE '%{tbSearch.Text.Trim()}%' OR type_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR type_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 2)
                    {
                        cmd = "SELECT type_id, subscription_type FROM subscription_types " +
                            $"WHERE subscription_type LIKE '%{tbSearch.Text.Trim()}%' OR subscription_type LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscription_type LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    break;

                case "Subscribers Subscriptions":
                    if (selectedSearchFilter == 1)
                    {
                        cmd = "SELECT subscriber_id, type_id FROM subscribers_subscriptions " +
                            $"WHERE subscriber_id LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscriber_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 2)
                    {
                        cmd = "SELECT subscriber_id, type_id FROM subscribers_subscriptions " +
                            $"WHERE type_id LIKE '%{tbSearch.Text.Trim()}%' OR type_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR type_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    break;

                case "Subscription Prices":
                    if (selectedSearchFilter == 1)
                    {
                        cmd = "SELECT type_id, subscription_price FROM subscription_prices " +
                            $"WHERE type_id LIKE '%{tbSearch.Text.Trim()}%' OR type_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR type_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 2)
                    {
                        cmd = "SELECT type_id, subscription_price FROM subscription_prices " +
                           $"WHERE subscription_price LIKE '%{tbSearch.Text.Trim()}%' OR subscription_price LIKE '{tbSearch.Text.Trim()}%' " +
                           $"OR subscription_price LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    break;

                case "Purchase Confirmations":
                    if (selectedSearchFilter == 1)
                    {
                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price,CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                            $"WHERE purchase_id LIKE '%{tbSearch.Text.Trim()}%' OR purchase_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR purchase_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 2)
                    {
                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price,CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                            $"WHERE subscriber_id LIKE '%{tbSearch.Text.Trim()}%' OR subscriber_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscriber_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 3)
                    {
                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price,CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                            $"WHERE type_id LIKE '%{tbSearch.Text.Trim()}%' OR type_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR type_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 4)
                    {
                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price,CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                            $"WHERE subscription_price LIKE '%{tbSearch.Text.Trim()}%' OR subscription_price LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR subscription_price LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 5)
                    {
                        try
                        {
                            string[] date = new string[3];
                            if (tbSearch.Text.Trim().Length == 10)
                            {
                                if (tbSearch.Text.Trim().Contains('.'))
                                {
                                    date = tbSearch.Text.Split('.');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price," +
                                        "CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = purchase_date";
                                    else
                                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price," +
                                        "CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = purchase_date";

                                }
                                else if (tbSearch.Text.Trim().Contains('/'))
                                {
                                    date = tbSearch.Text.Split('/');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = purchase_date";
                                    else
                                        cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price," +
                                        "CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = purchase_date";
                                }
                            }
                            else
                            {
                                cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price," +
                                "CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                                $"WHERE purchase_date LIKE '%{tbSearch.Text.Trim()}%' OR purchase_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR purchase_date LIKE '%{tbSearch.Text.Trim()}'";
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            cmd = "SELECT purchase_id, subscriber_id, type_id, subscription_price,CONVERT(varchar(10), purchase_date, 103) FROM purchase_confirmations " +
                                $"WHERE purchase_date LIKE '%{tbSearch.Text.Trim()}%' OR purchase_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR purchase_date LIKE '%{tbSearch.Text.Trim()}'";
                        }
                    }
                    break;

                case "Expiry Dates":
                    if (selectedSearchFilter == 1)
                    {
                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                            $"WHERE purchase_id LIKE '%{tbSearch.Text.Trim()}%' OR purchase_id LIKE '{tbSearch.Text.Trim()}%' " +
                            $"OR purchase_id LIKE '%{tbSearch.Text.Trim()}'";
                    }
                    else if (selectedSearchFilter == 2)
                    {
                        try
                        {
                            string[] date = new string[3];
                            if (tbSearch.Text.Trim().Length == 10)
                            {
                                if (tbSearch.Text.Trim().Contains('.'))
                                {
                                    date = tbSearch.Text.Split('.');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = expiry_date";
                                    else
                                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = expiry_date";

                                }
                                else if (tbSearch.Text.Trim().Contains('/'))
                                {
                                    date = tbSearch.Text.Split('/');
                                    if (date[0].Length == 2)
                                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                        $"WHERE '{date[2]}/{date[1]}/{date[0]}' = expiry_date";
                                    else
                                        cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                        $"WHERE '{date[0]}/{date[1]}/{date[2]}' = expiry_date";
                                }
                            }
                            else
                            {
                                cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                $"WHERE expiry_date LIKE '%{tbSearch.Text.Trim()}%' OR expiry_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR expiry_date LIKE '%{tbSearch.Text.Trim()}'";
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            cmd = "SELECT purchase_id, CONVERT(varchar(10), expiry_date, 103) FROM expiry_dates " +
                                $"WHERE expiry_date LIKE '%{tbSearch.Text.Trim()}%' OR expiry_date LIKE '{tbSearch.Text.Trim()}%' " +
                                $"OR expiry_date LIKE '%{tbSearch.Text.Trim()}'";
                        }
                    }
                    break;
            }
            return cmd;
        }

        /// <summary>
        /// Makes the placeholder of tbSearch TextBox visible/invisible when keyboard focus changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void tbSearch_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tbSearch.IsKeyboardFocused)
            {
                tbSearch_Placeholder.Visibility = Visibility.Collapsed;
            }
            else if (!tbSearch.IsKeyboardFocused && string.IsNullOrEmpty(tbSearch.Text))
            {
                tbSearch_Placeholder.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Updates the data source of the data grid after any value entered in the tbSearch TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if(selectedSearchFilter != 0)
            {
                GridDataSourceUpdate();
            }
            else
            {
                tbSearch.Clear();
                MessageBox.Show("Choose the search filter first!", "Something went wrong", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Changes the search filter for the table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void searchFilterOne_Click(object sender, RoutedEventArgs e)
        {
            selectedSearchFilter = 1;
        }

        /// <summary>
        /// Changes the search filter for the table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void searchFilterTwo_Click(object sender, RoutedEventArgs e)
        {
            selectedSearchFilter = 2;
        }

        /// <summary>
        /// Changes the search filter for the table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void searchFilterThree_Click(object sender, RoutedEventArgs e)
        {
            selectedSearchFilter = 3;
        }

        /// <summary>
        /// Changes the search filter for the table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void searchFilterFour_Click(object sender, RoutedEventArgs e)
        {
            selectedSearchFilter = 4;
        }

        /// <summary>
        /// Changes the search filter for the table.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void searchFilterFive_Click(object sender, RoutedEventArgs e)
        {
            selectedSearchFilter = 5;
        }

        /// <summary>
        /// Data source update of the data grid for other forms.
        /// </summary>
        public void RefreshDataGrid()
        {
            GridDataSourceUpdate();
        }
    }
}
