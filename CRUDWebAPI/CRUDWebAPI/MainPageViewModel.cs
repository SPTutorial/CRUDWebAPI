﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace CRUDWebAPI
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public MainPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public async void GetEmployees()
        {
            using (var client = new HttpClient())
            {
                // send a GET request  
                var uri = "http://192.168.0.5/api/Masters/GetEmployees";
                var result = await client.GetStringAsync(uri);

                //handling the answer  
                var EmployeeList = JsonConvert.DeserializeObject<List<Employee>>(result);

                Employees = new ObservableCollection<Employee>(EmployeeList);
                IsRefreshing = false;
            }
        }
        public Command AddEmployee
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new AddEmployee(null));
                });
            }
        }
        public Command EditEmployee
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new AddEmployee(null));
                });
            }
        }
        public Command RefreshData
        {
            get
            {
                return new Command(() =>
                {
                    GetEmployees();
                });
            }
        }


        bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                if(value!=null)
                {
                    Navigation.PushAsync(new AddEmployee(SelectedEmployee));
                }
                OnPropertyChanged();
            }
        }

        ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
