
namespace ForeignExhange.ViewModels
{
    using ForeignExhange.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.ComponentModel;
    using Xamarin.Forms;
    using ForeignExhange.Helpers;
    using ForeignExhange.Services;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Services
        ApiService apiServices;
        DataService dataService;
        DialogService dialogService;
        #endregion

        #region Eventes
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        bool _isRunning;
        bool _isEnabled;
        string _result;
        string _status;
        string _colorAlert;
        Rate _sourceRate;
        Rate _targetRate;
        List<Rate> rates;
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

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if(_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public string ColorAlert
        {
            get
            {
                return _colorAlert;
            }
            set
            {
                if (_colorAlert != value)
                {
                    _colorAlert = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorAlert)));
                }
            }
        }


        #endregion

        #region Constructors
        public MainViewModel()
        {
            apiServices = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            LoadRates();
        }
        #endregion

        #region Methods
        async void LoadRates()
        {
            IsRunning = true;
            Status = Lenguages.Loading;
            ColorAlert = Colors.ColorMessage;

            var connection = await apiServices.CheckConnection();
            if (!connection.IsSucces)
            {
                _isRunning = true;
                _colorAlert = Colors.ColorDangerInter;
                _status = connection.Message;
                LoadLocalData();
            }
            else
            {
                await LoadDataFromAPI();
            }

            if (rates.Count ==0)
            {
                IsEnabled = false;
                IsRunning = false;
                Status = Lenguages.Erro_internet_db;
                ColorAlert = Colors.ColorDanger;
                return;
            }
            else
            {
                Rates = new ObservableCollection<Rate>(rates);
                IsRunning = false;
                IsEnabled = true;
                Result = Lenguages.Ready; 
            }

        }

        private void LoadLocalData()
        {
            rates = dataService.Get<Rate>(false);
            Status = Lenguages.Rate_loaded_localdata;
            ColorAlert = Colors.ColorFine;

        }

        async Task LoadDataFromAPI()
        {
            var response = await apiServices.GetList<Rate>("https://apiexchangerates.azurewebsites.net", "api/Rates");
            if (!response.IsSucces)
            {
                LoadLocalData();
                return;
            }

            Status = Lenguages.Rate_loaded_internet;
            ColorAlert = Colors.ColorGod;
            //borra la db anterios (si la hay) y guarda una nueva con los datos actuales
            rates = (List<Rate>)response.Result;
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);

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
                await dialogService.ShowMessage(Lenguages.Error,Lenguages.AmountPlaceHolder);
                return;
            }

            decimal amount = 0;
            if(!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage(Lenguages.Error, Lenguages.AmountNumericValidation);
                return;
            }

           if(SourceRate == null)
           {
                  await dialogService.ShowMessage(Lenguages.Error,Lenguages.SourceRateTitle);
                  return;
           }

           if (TargeteRate == null)
           {
                await dialogService.ShowMessage(Lenguages.Error,Lenguages.TargetRateTitle);
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
