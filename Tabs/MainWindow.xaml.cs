using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;



namespace Tabs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Item> items = new List<Item>();
        History history = new History();
        public MainWindow()
        {
            InitializeComponent();
           
            Categories_CB.ItemsSource = Enum.GetValues(typeof(Categories));

            RefreshHistoryDataGrid();

            SetCategoryCount();

            SetBalance();
        }
        
        private void RefreshHistoryDataGrid()
        {
            
            history.Items_DataGrid.ItemsSource = items = SQLiteDataAccess.GetItems();
        }

        private void SetBalance()
        {
            var x = SQLiteDataAccess.GetBalance().FirstOrDefault();

            if (x != null)
            {
                Balance_Label.Content = $"Balance:{x.Balance} TND";
            }
            else
            {
                Balance_Label.Content = $"Balance:0 TND";
            }
        }
        private void SetCategoryCount()
        {
            //declating container vars
            float foodcount = 0;
            float clothescount = 0;
            float utilitiescount = 0;
            float misccount = 0;

            //setting vars
            foreach (var item in SQLiteDataAccess.GetItems())
            {
                if (item.Category == Categories.Food) { foodcount += item.ItemPrice; }
                if (item.Category == Categories.clothes) { clothescount += item.ItemPrice; }
                if (item.Category == Categories.Utilities) { utilitiescount += item.ItemPrice; }
                if (item.Category == Categories.Misc) { misccount += item.ItemPrice; }
            }

            //sending to forms
            FoodCount_Label.Content = $"Food:{foodcount} TND";
            UtilitiesCount_Label.Content = $"Utilities:{utilitiescount} TND";
            ClothesCount_Label.Content = $"Clothes:{clothescount} TND";
            MiscCount_Label.Content = $"Misc:{misccount} TND";
        }
        private void Salary_Btn_Click(object sender, RoutedEventArgs e)
        {
            float input;

            float.TryParse(Salary_TextBox.Text, out input); 

            if (input==0)
            {
                MessageBox.Show("Entrer un entier");
            }
            else
            {
                //adjusting balance
                var finances = SQLiteDataAccess.GetBalance().FirstOrDefault();

                float x;
                if (finances.Balance != 0)
                {
                    x = float.Parse(Salary_TextBox.Text) + finances.Balance;

                    Balance_Label.Content = $"Balance:{x} TND";
                }
                else
                {
                    x = float.Parse(Salary_TextBox.Text);
                    Balance_Label.Content = $"Balance:0 TND";
                }

                finances.Balance = x;
                SQLiteDataAccess.LoadSalary(finances);
                
                //setting values for new item
                var item = new Item();
                item.Date = DateTime.Now;
                item.ItemName = "Salaire";
                item.ItemPrice = input;
                item.Category = Categories.Misc;

                //saving to db
                SQLiteDataAccess.SaveItem(item);
            }
            Salary_TextBox.Text = "";
            RefreshHistoryDataGrid();
        }

        private void Pay_Btn_Click(object sender, RoutedEventArgs e)
        {
            var itemname = ItemName_TextBox.Text;
            float price;
            float.TryParse(ItemPrice_TextBox.Text,out price);
            if(itemname=="" || price == 0 || Categories_CB.SelectedItem==null)
            {
                MessageBox.Show("Entrer un prix ou nom non vide svp");
            }
            else
            {
                var item = new Item();
                item.Date = DateTime.Now;
                item.ItemName = itemname;
                item.ItemPrice = price;

                item.Category = (Categories)Enum.Parse(typeof(Categories), Categories_CB.SelectedValue.ToString());

                SQLiteDataAccess.SaveItem(item);
                var finances = SQLiteDataAccess.GetBalance().FirstOrDefault();
                finances.Balance -= price;
                SQLiteDataAccess.LoadSalary(finances);
                Balance_Label.Content = $"Balance:{finances.Balance} TND";
            }
            ItemName_TextBox.Text = "";
            ItemPrice_TextBox.Text = "";
            RefreshHistoryDataGrid();
        }

        private void DisplayList_Btn_Click(object sender, RoutedEventArgs e)
        {
            history.Visibility = Visibility.Visible;
        }

        private void MainCLosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            history.Close();
            Application.Current.Shutdown();
        }

        private void Salary_Btn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
