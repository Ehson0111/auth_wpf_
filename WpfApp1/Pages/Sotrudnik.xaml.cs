using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation; // Добавьте эту директиву
using WpfApp1.Models;

namespace WpfApp1.Pages
{
    public partial class Sotrudnik : Page
    {
        private Пр4_Агентсво_недвижимостиEntities db;

        public Sotrudnik(Авторизация user, string role)
        {
            InitializeComponent();
            db = new Пр4_Агентсво_недвижимостиEntities();
            LoadData();
        }

        private void LoadData()
        {
            var employees = db.Сотрудник.Select(c => new
            {
                c.Id_Сотрудник,
                c.Имя,
                c.Фамилия,
                c.Отчество,
                c.Контактные_данные,
                nazvanie = c.dolzhnost.nazvanie
            }).ToList();
            employeesDataGrid.ItemsSource = employees;
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    UpdateEmployeesDataGrid(); // Обновляем данные
        //}

        private void UpdateEmployeesDataGrid()
        {
            var updatedEmployees = db.Сотрудник.Select(c => new
            {
                c.Id_Сотрудник,
                c.Имя,
                c.Фамилия,
                c.Отчество,
                c.Контактные_данные,
                nazvanie = c.dolzhnost.nazvanie
            }).ToList();
            employeesDataGrid.ItemsSource = updatedEmployees;
        }

        private void adduser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployeeForm());
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (employeesDataGrid.SelectedItem != null)
            {
                var selectedEmployee = employeesDataGrid.SelectedItem as dynamic;
                if (selectedEmployee != null)
                {
                    int employeeId = selectedEmployee.Id_Сотрудник;
                    NavigationService.Navigate(new EditEmployeeForm(employeeId));
                }
            }
        }
    }
}