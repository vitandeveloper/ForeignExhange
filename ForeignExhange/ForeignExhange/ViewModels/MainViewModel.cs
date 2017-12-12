
namespace ForeignExhange.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System;

    public class MainViewModel
    {
        #region Propiedades
        public string Amount { get; set; }
        public ObservableCollection<Rate> Rates { get; set; }
        public Rate SourceRate { get; set; }
        public Rate TargetRate { get; set; }
        public bool IsRunning { get; set; }
        public bool IsEnable { get; set; }
        public string Result { get; set; }
        #endregion
        public MainViewModel()
        {

        }

        #region
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
