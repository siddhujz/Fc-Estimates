using Fc_Estimates.MessageInfrastructure;
using Fc_Estimates.Model;
using Fc_Estimates.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fc_Estimates.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IDataAccessService _serviceProxy;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        //IDataAccessService _serviceProxy;

        ObservableCollection<Estimate> _Estimates;

        public ObservableCollection<Estimate> Estimates
        {
            get { return _Estimates; }
            set
            {
                _Estimates = value;
                RaisePropertyChanged("Estimates");
            }
        }


        /// <summary>
        /// The declaration of the Estimate object for Save and Messanger purpose
        /// </summary>
        Estimate _Estimate;

        public Estimate Estimate
        {
            get { return _Estimate; }
            set
            {
                _Estimate = value;
                RaisePropertyChanged("Estimate");
            }
        }

        /// <summary>
        /// The declaration used in case of search Estimate
        /// </summary>
        string _MedicalProcedure;

        public string MedicalProcedure
        {
            get { return _MedicalProcedure; }
            set
            {
                _MedicalProcedure = value;
                RaisePropertyChanged("MedicalProcedure");
            }
        }

        #region Command Object Declarations
        public RelayCommand ReadAllCommand { get; set; }
        public RelayCommand<Estimate> SaveCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand<Estimate> SendEstimateCommand { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// The class gets an instance of The DataAccessService
        /// </summary>
        public MainViewModel(IDataAccessService dataAccessService)
        {
            _serviceProxy = dataAccessService;
            _serviceProxy.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            Estimates = new ObservableCollection<Estimate>();
            Estimate = new Estimate();
            ReadAllCommand = new RelayCommand(GetEstimates);
            SaveCommand = new RelayCommand<Estimate>(SaveEstimate);
            WelcomeTitle = "Welcome To MVVM";
            SearchCommand = new RelayCommand(SearchEstimate);
            SendEstimateCommand = new RelayCommand<Estimate>(SendEstimate);
            ReceiveEstimate();
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}

        /// <summary>
        /// Method to Read All Estimates
        /// </summary>
        /// 
        void GetEstimates()
        {
            Estimates.Clear();
            foreach (var item in _serviceProxy.GetEstimates())
            {
                Estimates.Add(item);
            }
        }

        /// <summary>
        /// Method to Save Estimates. Once the Estimate is added in the database
        /// it will be added in the Estimates observable collection and Property Changed will be raised
        /// </summary>
        /// <param name="estimate"></param>
        void SaveEstimate(Estimate estimate)
        {
            Estimate.estimateId = _serviceProxy.CreateEstimate(estimate);
            if (Estimate.estimateId != 0)
            {
                Estimates.Add(Estimate);
                RaisePropertyChanged("Estimate");
            }
        }

        /// <summary>
        /// The method to search Estimate based upon the MedicalProcedure
        /// </summary>
        void SearchEstimate()
        {
            Estimates.Clear();
            var Res = from e in _serviceProxy.GetEstimates()
                      where e.medicalProcedure.ToLower().StartsWith(MedicalProcedure.ToLower())
                      select e;
            foreach (var item in Res)
            {
                Estimates.Add(item);
            }
        }

        /// <summary>
        /// The method to send the selected Estimate from the DataGrid on UI
        /// to the View Model
        /// </summary>
        /// <param name="estimate"></param>
        void SendEstimate(Estimate estimate)
        {
            if (estimate != null)
            {
                Messenger.Default.Send<MessageCommunicator>(new MessageCommunicator()
                {
                    Emp = estimate
                });
            }
        }

        /// <summary>
        /// The Method used to Receive the send Estimate from the DataGrid UI
        /// and assigning it the the Estimate Notifiable property so that
        /// it will be displayed on the other view
        /// </summary>
        void ReceiveEstimate()
        {
            if (Estimate != null)
            {
                Messenger.Default.Register<MessageCommunicator>(this, (estimate) =>
                {
                    this.Estimate = estimate.Emp;
                });
            }
        }

    }
}