using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class EditEmployeeForm : Page
    {
        private Пр4_Агентсво_недвижимостиEntities db;
        private int _employeeId;
        private bool _isNewEmployee; // Флаг для определения режима (добавление или редактирование)sdvls,d

        public EditEmployeeForm()
        {
            InitializeComponent();
            _isNewEmployee = true; // Режим добавления
            db = new Пр4_Агентсво_недвижимостиEntities();

            // Инициализация нового сотрудника
            DataContext = new Сотрудник
            {
                Авторизация = new Авторизация() // Создаем новую запись авторизации
            };

            // Загрузка данных для ComboBox
            cbDolzhnost.ItemsSource = db.dolzhnost.ToList();
            cbpol.ItemsSource = db.pol.ToList();
        }

        public EditEmployeeForm(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            db = new Пр4_Агентсво_недвижимостиEntities();

            // Загрузка данных сотрудника
            var employee = db.Сотрудник.Find(employeeId);
            if (employee == null)
            {
                MessageBox.Show("Сотрудник не найден!");
                return;
            }

            // Установка DataContext
            DataContext = employee;

            // Загрузка данных для ComboBox
            cbDolzhnost.ItemsSource = db.dolzhnost.ToList();
            cbpol.ItemsSource = db.pol.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_isNewEmployee)
                {
                    // Добавление нового сотрудника
                    var newEmployee = DataContext as Сотрудник;
                    if (newEmployee == null)
                    {
                        MessageBox.Show("Ошибка при создании нового сотрудника!");
                        return;
                    }

                    // Хэширование пароля
                    HashPassword hash = new HashPassword();
                    newEmployee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

                    // Добавление в базу данных
                    db.Сотрудник.Add(newEmployee);
                    db.SaveChanges();

                    MessageBox.Show("Новый сотрудник успешно добавлен!");
                }
                else
                {
                    // Редактирование существующего сотрудника
                    var employee = db.Сотрудник.Find(_employeeId);
                    if (employee == null)
                    {
                        MessageBox.Show("Сотрудник не найден!");
                        return;
                    }

                    // Обновление данных сотрудника
                    employee.Имя = txtFirstName.Text;
                    employee.Фамилия = txtLastName.Text;
                    employee.Отчество = txtMiddleName.Text;
                    employee.Контактные_данные = txtContactData.Text;
                    employee.Зарплата = Convert.ToInt32(txtSalary.Text);
                    employee.Дата_рождение = dpBirthday.SelectedDate.Value;
                    employee.id_dolzhnost = (int)cbDolzhnost.SelectedValue;
                    employee.id_pol = (int)cbpol.SelectedValue;

                    // Обновление пароля
                    HashPassword hash = new HashPassword();
                    employee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

                    // Сохранение изменений
                    db.SaveChanges();
                    MessageBox.Show("Изменения сохранены!");
                }

                // Переход на предыдущую страницу
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void CLEAR_Click(object sender, RoutedEventArgs e)
        {
            // Очистка полей
            txtFirstName.Clear();
            txtLastName.Clear();
            txtMiddleName.Clear();
            txtContactData.Clear();
            txtSalary.Clear();
            dpBirthday.SelectedDate = null;
            cbDolzhnost.SelectedIndex = -1;
            cbpol.SelectedIndex = -1;
            pbPassword.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Логика для кнопки "Добавить" (если нужно)
        }
    }
}