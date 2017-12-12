
namespace ForeignExhange.ViewModels
{
    using ForeignExhange.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;
    using System.ComponentModel;

    public class MainViewModel : INotifyPropertyChanged
    {
        #region Eventes
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        bool _isRunning;
        string _result;
        #endregion

        #region Propiedades
        public string Amount { get; set; }
        public ObservableCollection<Rate> Rates { get; set; }
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
        public bool IsEnabled { get; set; }
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
        void LoadRates()
        {
            IsRunning = true;
            Result = "Cargando datos...";
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
        void Convert()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
