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
        public AddEmployeeViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public Command SaveEmployee
        {
            get
            {
                return new Command(async () =>
                {
                    Employee employee = new Employee();
                    employee.Name = Name;
                    employee.Addresss = Address;
                    employee.PhoneNumber = Phone;

                    string url = "http://192.168.0.6:8092/api/Masters/SaveEmployee";
                    HttpClient client = new HttpClient();
                    string jsonData = JsonConvert.SerializeObject(employee);
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
