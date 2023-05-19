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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = nameInput.Text.Split(' ');

            AddSubscriber(data[0].Trim(), Convert.ToDateTime(data[1].Trim()));
        }

        public void AddSubscriber(string email, DateTime birthDate)
        {
            using(var subs = new UserContext()) 
            {
                var sub = new Subscriber() {Email = email, BirthDate = birthDate };

                subs.Subscribers.Add(sub);
                subs.SaveChanges();

                clickOut.Text = subs.Subscribers.First().Email + " " + subs.Subscribers.Count();
            }
        }
    }
}
