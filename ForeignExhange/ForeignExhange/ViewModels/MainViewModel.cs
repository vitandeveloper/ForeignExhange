
namespace ForeignExhange.ViewModels
{
    using ForeignExhange.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;
    using System.ComponentModel;
    using System.Net.Http;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using Xamarin.Forms;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Eventes
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        ObservableCollection<Rate> _rates;
        #endregion

        #region Propiedades
        public string Amount { get; set; }
        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }
            set
            {
                if(_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rates)));
                }
            }
        }
        public Rate SourceRate { get; set; }
        public Rate TargeteRate { get; set; }
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if(_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if(_isEnabled != value)
                {
                    _isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if(_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
                }
            }
         }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            LoadRates();
        }
        #endregion

        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Result = "Cargando datos...";
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://apiexchangerates.azurewebsites.net");
                var controller = "/api/Rates";
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if(!response.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = Result;
                }

                var rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                Rates = new ObservableCollection<Rate>(rates);

                IsRunning = false;
                IsEnabled = true;
                Result = "Listo para convertir";
            
            }
            catch
            {
                IsRunning = false;
                Result = "Ocurrio algo al conectarnos con el servidor.";
            }
          
        }
        #endregion

        #region Command
        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }
        async void Convert()
        {
           if(string.IsNullOrEmpty(Amount))
           {
                await Application.Current.MainPage.DisplayAlert("Alerta",
                                                                "Ingrece un monto",
                                                                "Ok");
                return;
            }

            decimal amount = 0;
            if(!decimal.TryParse(Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta",
                                                                "Ingrece un monto numerico",
                                                                "Ok");
                return;
            }

            if(SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta",
                                                                "Seleccione una taza origen",
                                                                 "Ok");
                return;
            }

            if (TargeteRate == null)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta",
                                                                "Seleccione una taza destino",
                                                                "Ok");
                return;
            }

            var amountConerted = amount / (decimal)SourceRate.TaxRate *
                                         (decimal)TargeteRate.TaxRate;
            Result = string.Format("{0} {1:C2} = {2} {3:C2}",
                                    SourceRate.Code,
                                    Amount,
                                    TargeteRate.Code,
                                    amountConerted);
        }
        #endregion


    }
}
