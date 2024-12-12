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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

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

            string cleanedSearchText = TBoxSearch.Text.Replace("(", "")
                                           .Replace(")", "")
                                           .Replace("-", "")
                                           .Replace(" ", "")
                                           .Replace(".", "")
                                           .Replace("+", "");

            currentAgents = currentAgents.Where(p =>
                p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) ||
                p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) ||
                p.Phone.Replace("(", "")
                        .Replace(")", "")
                        .Replace("-", "")
                        .Replace(" ", "")
                        .Replace(".", "")
                        .Replace("+", "").Contains(cleanedSearchText)
            ).ToList();

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

            AgentListView.ItemsSource = currentAgents;

            TableList = currentAgents;

            ChangePage(0, 0);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean ifUpdate = true;

            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            ifUpdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            ifUpdate = false;
                        }
                        break;
                }
            }

                if (ifUpdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    TBCount.Text = min.ToString();
                    TBAllRecords.Text = " из " + CountRecords.ToString();

                    AgentListView.ItemsSource = CurrentPageList;
                    AgentListView.Items.Refresh();
                }
            
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

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAgentButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }
    }
}
