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
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentAgents = ВеркоГлазкиSaveEntities.GetContext().Agent.ToList();

            AgentListView.ItemsSource = currentAgents;

            ComboFilter.SelectedIndex = 0;
            ComboSorting.SelectedIndex = 0;

            UpdateAgents();

            // Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void UpdateAgents()
        {
            var currentAgents = ВеркоГлазкиSaveEntities.GetContext().Agent.ToList();

            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || p.Phone.Contains(TBoxSearch.Text)).ToList();
            
            if (ComboSorting.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (ComboSorting.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSorting.SelectedIndex == 3)
            {
                // currentAgents = currentAgents.OrderBy(p => p.Discount).ToList();
            }
            if (ComboSorting.SelectedIndex == 4)
            {
                // currentAgents = currentAgents.OrderByDescending(p => p.Discount).ToList();
            }
            if (ComboSorting.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSorting.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }

            if (ComboFilter.SelectedIndex > 0) // Если выбран не "Все типы"
            {
                var selectedType = ((ComboBoxItem)ComboFilter.SelectedItem).Content.ToString();
                currentAgents = currentAgents.Where(p => p.AgentType.Title == selectedType).ToList();
            }

            AgentListView.ItemsSource = currentAgents.ToList();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }


        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
    }
}
