﻿
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
    using ForeignExhange.Helpers;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Eventes
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        Rate _sourceRate;
        Rate _targetRate;
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
        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }
            set
            {
                if(_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceRate)));
                }
            }
        }
        public Rate TargeteRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargeteRate)));
                }
            }
        }
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
            Result = Lenguages.Loading;
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
                Result = Lenguages.Ready;
            
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
                await Application.Current.MainPage.DisplayAlert(Lenguages.Error,
                                                                Lenguages.AmountPlaceHolder,
                                                                Lenguages.Accept);
                return;
            }

            decimal amount = 0;
            if(!decimal.TryParse(Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert(Lenguages.Error,
                                                                Lenguages.AmountNumericValidation,
                                                                Lenguages.Accept);
                return;
            }

           if(SourceRate == null)
           {
                  await Application.Current.MainPage.DisplayAlert(Lenguages.Error,
                                                                  Lenguages.SourceRateTitle,
                                                                   Lenguages.Accept);
                  return;
           }

           if (TargeteRate == null)
           {
                await Application.Current.MainPage.DisplayAlert(Lenguages.Error,
                                                                Lenguages.TargetRateTitle,
                                                                Lenguages.Accept);
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


        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        void Switch()
        {
            var aux = TargeteRate;
            TargeteRate = SourceRate;
            SourceRate = aux;
            Convert();
        }

        #endregion


    }
}
