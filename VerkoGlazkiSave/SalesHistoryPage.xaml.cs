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
        private List<Product> _allProducts;

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
                .ToList();

            SalesListView.ItemsSource = currentSales.Select(s => new
            {
                ProductSale = s,
                ProductName = s.Product.Title,
                Quantity = s.ProductCount,
                SaleDateFormatted = s.SaleDate.ToString("dd.MM.yyyy")
            }).ToList();
        }

        private void LoadProducts()
        {
            _allProducts = ВеркоГлазкиSaveEntities.GetContext().Product.ToList();
            ProductComboBox.ItemsSource = _allProducts;
        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is Product selectedProduct)
            {
                if (int.TryParse(QuantityTextBox.Text, out int quantity) && quantity > 0)
                {
                    if (SaleDatePicker.SelectedDate.HasValue)
                    {
                        var saleDate = SaleDatePicker.SelectedDate.Value;

                        var newSale = new ProductSale
                        {
                            ProductID = selectedProduct.ID,
                            AgentID = _currentAgent.ID,
                            SaleDate = saleDate,
                            ProductCount = quantity
                        };

                        ВеркоГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
                        ВеркоГлазкиSaveEntities.GetContext().SaveChanges();

                        LoadSalesHistory();
                        MessageBox.Show("Продажа добавлена.");
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите дату продажи.");
                    }
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
            if (SalesListView.SelectedItem is var selectedSale && selectedSale != null)
            {
                var saleToDelete = (selectedSale as dynamic).ProductSale;

                if (saleToDelete != null)
                {
                    ВеркоГлазкиSaveEntities.GetContext().ProductSale.Remove(saleToDelete);
                    ВеркоГлазкиSaveEntities.GetContext().SaveChanges();

                    LoadSalesHistory();
                    MessageBox.Show("Продажа удалена.");
                }
                else
                {
                    MessageBox.Show("Не удалось найти продажу для удаления.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продажу для удаления.");
            }
        }


        private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EditTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();
            var filteredProducts = _allProducts.Where(p => p.Title.ToLower().Contains(searchText)).ToList();
            ProductComboBox.ItemsSource = filteredProducts;

            if (filteredProducts.Count > 0)
            {
                ProductComboBox.IsDropDownOpen = true;
            }
        }

        private void ProductComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox editTextBox = ProductComboBox.Template.FindName("PART_EditableTextBox", ProductComboBox) as TextBox;

            if (editTextBox != null)
            {
                editTextBox.TextChanged += EditTextBox_TextChanged;
            }
        }
    }

}
