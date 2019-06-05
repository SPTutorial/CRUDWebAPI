using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace CRUDWebAPI
{
    class AddEmployeeViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        Employee _employee;
        public AddEmployeeViewModel(INavigation _navigation,Employee employee)
        {
            Navigation = _navigation;
            _employee = employee;
            if (_employee != null)
            {
                Name = _employee.Name;
                Address = _employee.Addresss;
                Phone = _employee.PhoneNumber;
                IsVisibleDeleteBtn = true;
            }
        }

        public Command SaveEmployee
        {
            get
            {
                return new Command(async () =>
                {
                    if(_employee!=null)
                    {
                        _employee.Name = Name;
                        _employee.Addresss = Address;
                        _employee.PhoneNumber = Phone;
                    }
                    else
                    {
                        _employee = new Employee();
                        _employee.Name = Name;
                        _employee.Addresss = Address;
                        _employee.PhoneNumber = Phone;
                    }                    

                    string url = "http://192.168.0.5/api/Masters/SaveEmployee";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(_employee);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string result = await response.Content.ReadAsStringAsync();
                    Response responseData = JsonConvert.DeserializeObject<Response>(result);
                    if (responseData.Status == 1)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        //
                    }
                });
            }
        }
        public Command DeleteEmployee
        {
            get
            {
                return new Command(async () =>
                {
                    string url = $"http://192.168.0.5/api/Masters/DeleteEmployee?EmployeeId={_employee.Id}";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(_employee);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.DeleteAsync(url);
                    string result = await response.Content.ReadAsStringAsync();
                    Response responseData = JsonConvert.DeserializeObject<Response>(result);
                    if (responseData.Status == 1)
                    {
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", responseData.Message, "ok");
                    }
                });
            }
        }
        bool _isVisibleDeleteBtn;
        public bool IsVisibleDeleteBtn
        {
            get
            {
                return _isVisibleDeleteBtn;
            }
            set
            {
                _isVisibleDeleteBtn = value;
                OnPropertyChanged();
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != null)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        string _phone;
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if(value!=null)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
