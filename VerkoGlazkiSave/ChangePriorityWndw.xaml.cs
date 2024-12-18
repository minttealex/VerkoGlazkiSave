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

namespace VerkoGlazkiSave
{
    /// <summary>
    /// Логика взаимодействия для ChangePriorityWndw.xaml
    /// </summary>
    public partial class ChangePriorityWndw : Window
    {
        public ChangePriorityWndw(int maxPriority)
        {
            InitializeComponent();
            TBPriority.Text = maxPriority.ToString();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void SavePriorityBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBPriority.Text))
            {
                this.DialogResult = false;
                this.Close();
                return;
            }
            if (int.TryParse(TBPriority.Text, out int newPriority))
            {
                if (newPriority < 0)
                {
                    MessageBox.Show("Приоритет не может быть отрицательным");
                    return;
                }

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите корректное числовое значение для приоритета");
            }
        }
    }
}
