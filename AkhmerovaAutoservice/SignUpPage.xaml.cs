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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;
            DataContext = _currentService;
            var _currentClient = Entities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                Entities.GetContext().ClientService.Add(_currentClientService);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            string filteredText = new string(s.Where(c => char.IsDigit(c) || c == ':').ToArray());
            if (s != filteredText)
            {
                TBEnd.Text = "";
                TBStart.Text = TBStart.Text.Substring(TBStart.Text.Length);
                return;
            }
            if (s.Length < 5 || !s.Contains(':'))
                return;
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());
                if (startHour / 60 > 23 || startMin > 59)
                {
                    MessageBox.Show("Введите действительное время");
                    TBStart.Clear();
                    return;
                }
                int sum = startHour + startMin + _currentService.Duration;
                int EndHour = sum / 60;
                if (EndHour >= 24) EndHour -= 24;
                int EndMin = sum % 60;
                s = EndHour.ToString() + ":";
                if (EndMin == 0)
                    s += "00";
                else if (EndMin > 0 && EndMin < 10)
                    s = s + '0' + EndMin;
                else
                    s += EndMin.ToString();
                TBEnd.Text = s;
            }
        }
    }
}
