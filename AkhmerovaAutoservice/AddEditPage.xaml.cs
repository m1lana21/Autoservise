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

namespace AkhmerovaAutoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        bool IsEditing = false;
        private string oldName;
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
            {
                _currentService = SelectedService;
                IsEditing = true;
            }
            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost == 0) 
                errors.AppendLine("Укажите стоимость услуги");
            string s = _currentService.Discount.ToString();

            if (string.IsNullOrWhiteSpace(s)) 
                errors.AppendLine("Укажите скидку");

            string s1 = _currentService.Duration.ToString();

            if (string.IsNullOrWhiteSpace(s1))
                errors.AppendLine("Укажите длительность услуги");

            if (_currentService.Duration > 240 || _currentService.Duration < 0)
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0 минут");
            if (_currentService.Discount < 0 || _currentService.Discount > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allServices = Entities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();
            if (!IsEditing)
            {
                if (allServices.Count != 0)
                {
                    MessageBox.Show("Уже существует такая услуга");
                    return;
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentService.ID == 0)
            {
                Entities.GetContext().Service.Add(_currentService);
            }
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}
