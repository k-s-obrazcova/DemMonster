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

namespace WpfApp27
{
    /// <summary>
    /// Логика взаимодействия для All_Users.xaml
    /// </summary>
    public partial class All_Users : Window
    {
        public Session1_01Entities db = new Session1_01Entities();
        public All_Users()
        {
            InitializeComponent();
            Data_Reload();
        }

        public void Data_Reload()
        {
            var UsersList = db.Users.ToList();
            DataUsers.SelectedValuePath = "ID";
            DataUsers.ItemsSource = UsersList;
            DataUsers.SelectionMode = DataGridSelectionMode.Single;

            var OfficeList = db.Offices.ToList();
            Parametr.ItemsSource = OfficeList;
            Parametr.SelectedIndex = 0;
            Parametr.DisplayMemberPath = "Title";
            Parametr.SelectedValuePath = "ID";

            Functional functional = new Functional();

            All.Text = functional.CountUsers().ToString();
            Cairo.Text = UsersList.Where(a => a.Offices.Title == "Cairo").ToList().Count.ToString();
            double AverageCount = 0;
            foreach(var user in UsersList)
            {
                AverageCount += user.ID;
            }
            if(AverageCount != 0)
            {

                AverageCount /= UsersList.Count;
            }
            Average.Text = AverageCount.ToString();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var UsersList = db.Users.Where(a => a.FirstName.Contains(Search.Text)||a.LastName.Contains(Search.Text)).ToList();
            DataUsers.SelectedValuePath = "ID";
            DataUsers.ItemsSource = UsersList;
            DataUsers.SelectionMode = DataGridSelectionMode.Single;
        }

        private void Parametr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Parametr.ActualHeight != 0)
            {
                int OfficeID = (int)Parametr.SelectedValue;
                var UsersList = db.Users.Where(a => a.Offices.ID == OfficeID).ToList();
                DataUsers.SelectedValuePath = "ID";
                DataUsers.ItemsSource = UsersList;
                DataUsers.SelectionMode = DataGridSelectionMode.Single;
            }

        }
    }
}
