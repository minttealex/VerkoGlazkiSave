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

namespace VerkoGlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для SalesHistoryPage.xaml
    /// </summary>
    public partial class SalesHistoryPage : Page
    {
        private Agent _currentAgent = new Agent();

        public SalesHistoryPage(Agent selectedAgent)
        {
            InitializeComponent();

            if (selectedAgent != null)
            {
                _currentAgent = selectedAgent;
            }


            DataContext = _currentAgent;
            LoadSalesHistory();
            LoadProducts();
        }

        private void LoadSalesHistory()
        {
            var currentSales = ВеркоГлазкиSaveEntities.GetContext()
                .ProductSale
                .Where(s => s.AgentID == _currentAgent.ID) 
                .Select(s => new
                {
                    ProductName = s.Product.Title,
                    Quantity = s.ProductCount,   
                    SaleDate = s.SaleDate     
                })
                .ToList();

            SalesListView.ItemsSource = currentSales.Select(s => new
            {
                s.ProductName,
                s.Quantity,
                SaleDateFormatted = s.SaleDate.ToString("dd.MM.yyyy")
            }).ToList();
        }


        private void LoadProducts()
        {
            var products = ВеркоГлазкиSaveEntities.GetContext().Product.ToList();
            ProductComboBox.ItemsSource = products;
        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if a product is selected
            if (ProductComboBox.SelectedItem is Product selectedProduct)
            {
                if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
                {
                    var newSale = new ProductSale
                    {
                        ProductID = selectedProduct.ID,
                        AgentID = _currentAgent.ID,
                        SaleDate = DateTime.Now,
                        ProductCount = quantity
                    };

                    ВеркоГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
                    ВеркоГлазкиSaveEntities.GetContext().SaveChanges();

                    LoadSalesHistory();
                    MessageBox.Show("Продажа добавлена.");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество больше 0.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для продажи.");
            }
        }


        private void DeleteSaleButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
