using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Validators;

namespace WpfApp1.Pages
{
    public partial class EditEmployeeForm : Page
    {
        private Пр4_Агентсво_недвижимостиEntities db;
        private int _employeeId;
        private bool _isNewEmployee; //  для определения режима (добавление или редактирование)sdvls,dxv

        public EditEmployeeForm()
        {
            InitializeComponent();
            _isNewEmployee = true; // Режим добавления
            db = new Пр4_Агентсво_недвижимостиEntities();

            DataContext = new Сотрудник
            {
                Авторизация = new Авторизация()
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

            DataContext = employee;

            // Загрузка данных для ComboBox
            cbDolzhnost.ItemsSource = db.dolzhnost.ToList();
            cbpol.ItemsSource = db.pol.ToList();
        }

        //private void BtnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (_isNewEmployee)
        //        {
        //            // Добавление нового сотрудника
        //            var newEmployee = DataContext as Сотрудник;
        //            if (newEmployee == null)
        //            {
        //                MessageBox.Show("Ошибка при создании нового сотрудника!");
        //                return;
        //            }

        //            // Хэширование пароля
        //            HashPassword hash = new HashPassword();
        //            newEmployee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

        //            // Добавление в базу данных
        //            db.Сотрудник.Add(newEmployee);
        //            db.SaveChanges();

        //            MessageBox.Show("Новый сотрудник успешно добавлен!");
        //        }
        //        else
        //        {
        //            // Редактирование существующего сотрудника
        //            var employee = db.Сотрудник.Find(_employeeId);
        //            if (employee == null)
        //            {
        //                MessageBox.Show("Сотрудник не найден!");
        //                return;
        //            }

        //            // Обновление данных сотрудника
        //            employee.Имя = txtFirstName.Text;
        //            employee.Фамилия = txtLastName.Text;
        //            employee.Отчество = txtMiddleName.Text;
        //            employee.Контактные_данные = txtContactData.Text;
        //            employee.Зарплата = Convert.ToInt32(txtSalary.Text);
        //            employee.Дата_рождение = dpBirthday.SelectedDate.Value;
        //            employee.id_dolzhnost = (int)cbDolzhnost.SelectedValue;
        //            employee.id_pol = (int)cbpol.SelectedValue;

        //            // Обновление пароля
        //            HashPassword hash = new HashPassword();
        //            employee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

        //            // Сохранение изменений
        //            db.SaveChanges();
        //            MessageBox.Show("Изменения сохранены!");
        //        }

        //        // Переход на предыдущую страницу
        //        NavigationService.GoBack();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка: {ex.Message}");
        //    }
        //}

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var employee = DataContext as Сотрудник;
                if (employee == null)
                {
                    MessageBox.Show("Ошибка при создании или редактировании сотрудника!");
                    return;
                }

                // Валидация данных сотрудника
                var validator = new EmployeeValidator();
                var validationResults = validator.Validate(employee);
                //string[] d = new string[100];

                string error = "Обязательно:";
                if (validationResults.Any())
                {
                    foreach (var result in validationResults)
                    {
                        //strings.Append(result.ToString());
                        //MessageBox.Show(result.ErrorMessage);
                        error = error + "\n"+ result.ToString();
                    }
                    if (error!= "Обязательно:")
                    {
                        MessageBox.Show(error);
                    }

                    return;
                }

                if (_isNewEmployee)
                {
                    // Хэширование пароля
                    HashPassword hash = new HashPassword();
                    employee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

                    // Добавление нового сотрудника
                    db.Сотрудник.Add(employee);
                    db.SaveChanges();

                    MessageBox.Show("Новый сотрудник успешно добавлен!");
                }
                else
                {
                    // Редактирование существующего сотрудника
                    var existingEmployee = db.Сотрудник.Find(_employeeId);
                    if (existingEmployee == null)
                    {
                        MessageBox.Show("Сотрудник не найден!");
                        return;
                    }

                    // Обновление данных сотрудника
                    existingEmployee.Имя = txtFirstName.Text;
                    existingEmployee.Фамилия = txtLastName.Text;
                    existingEmployee.Отчество = txtMiddleName.Text;
                    existingEmployee.Контактные_данные = txtContactData.Text;
                    existingEmployee.Зарплата = Convert.ToInt32(txtSalary.Text);
                    existingEmployee.Дата_рождение = dpBirthday.SelectedDate.Value;
                    existingEmployee.id_dolzhnost = (int)cbDolzhnost.SelectedValue;
                    existingEmployee.id_pol = (int)cbpol.SelectedValue;

                    // Обновление пароля
                    HashPassword hash = new HashPassword();
                    existingEmployee.Авторизация.password = hash.HashPassword1(pbPassword.Text);

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
            // Логика для кнопки "Добавить" (если нужно)djf 
        }

    }
}