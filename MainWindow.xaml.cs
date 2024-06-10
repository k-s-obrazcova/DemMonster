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
using System.Security.Cryptography;
using System.Security.Policy;
using Microsoft.Win32;
using System.IO;

namespace WpfApp27
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Session1_01Entities db =  new Session1_01Entities();
        public MainWindow()
        {
            InitializeComponent();
            Title = "Пользователи";
            List_Reload();
        }

        public void List_Reload()
        {
            var UsersList = db.Users.ToList();
  //          UsersList = UsersList.Where(x => x.FirstName == "Karim").Distinct().ToList();
            ListCheck.SelectedValuePath = "ID";
            ListCheck.ItemsSource = UsersList;
            ListCheck.SelectionMode = SelectionMode.Single;

            var OfficeList = db.Offices.ToList();
            Office.ItemsSource = OfficeList;
            Office.SelectedIndex = 0;
            Office.DisplayMemberPath = "Title";
            Office.SelectedValuePath = "ID";
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("INFO", "User Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("WARNING", "User warn", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Console.WriteLine("YES");
                    break;
                case MessageBoxResult.No:
                    Console.WriteLine("NO");
                    break;
            }


        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
            string hash = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Password.Text))).Replace("-", "");
            Users user = new Users { 
            Email = Email.Text,
            Password = hash,
            LastName = Last_Name.Text,
            FirstName = First_Name.Text,
            OfficeID = (int)Office.SelectedValue,
            Birthdate = Birthday.SelectedDate.Value,
            RoleID = (bool)Role.IsChecked ? 1 : 2,
            Active = true,
            };
            db.Users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch 
            {
            
            }
            List_Reload();

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ListCheck.SelectedItem != null)
            {
                Users selectedItem = (Users)ListCheck.SelectedItem;
                selectedItem.LastName = Last_Name.Text;
                selectedItem.FirstName = First_Name.Text;
                selectedItem.Email = Email.Text;
                db.SaveChanges();
                List_Reload();
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListCheck.SelectedItem != null)
            {
            Users selectedItem = (Users)ListCheck.SelectedItem;
            db.Users.Remove(selectedItem);
            db.SaveChanges();
            List_Reload();
            }

        }

        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выбор фоточки";
            openFileDialog.Filter = "All supported gapfics|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Profile.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                File.Delete(Directory.GetCurrentDirectory()+"/Photo/"+openFileDialog.SafeFileName);
                File.Copy(openFileDialog.FileName, Directory.GetCurrentDirectory() + "/Photo/" + openFileDialog.SafeFileName);
            }
        }

        private void ListCheck_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListCheck.SelectedItem != null)
            {
                Users selectedItem = (Users)ListCheck.SelectedItem;
                Email.Text = selectedItem.Email;
                Last_Name.Text = selectedItem.LastName;
                First_Name.Text = selectedItem.FirstName;
                Office.SelectedItem = selectedItem.Offices;
                Password.Text = selectedItem.Password;
                Birthday.SelectedDate = selectedItem.Birthdate;
                if (selectedItem.RoleID == 1)
                {
                    Role.IsChecked = true;
                }
                else
                {
                    Role.IsChecked = false;
                }
            }
  
        }
        private void Information_Click(object sender, RoutedEventArgs e)
        { 
            Button button = sender as Button;
            if (button != null)
            {
                Grid grid = button.Parent as Grid;
                if (grid != null)
                {
                    Users user = grid.DataContext as Users;
                    if (user != null)
                    {
                        Window1 window = new Window1(user);
                        window.Show();
                        this.Close();
                    }
                }
            }
        
        
        }
        }
}
