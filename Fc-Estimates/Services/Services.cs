using Fc_Estimates.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fc_Estimates.Services
{
    /// <summary>
    /// The Interface defining methods for Create Estimate and Read All Estimates  
    /// </summary>
    public interface IDataAccessService
    {
        void GetData(Action<DataItem, Exception> callback);
        ObservableCollection<Estimate> GetEstimates();
        int CreateEstimate(Estimate estimate);
    }

    /// <summary>
    /// Class implementing IDataAccessService interface and implementing
    /// its methods by making call to the Entities using CompanyEntities object
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        REVINTEntities context;
        public DataAccessService()
        {
            context = new REVINTEntities();
        }

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }

        public ObservableCollection<Estimate> GetEstimates()
        {
            ObservableCollection<Estimate> estimates = new ObservableCollection<Estimate>();
            foreach (var item in context.Estimates.OrderByDescending(e => e.medicalProcedure))
            {
                if (item.maximumCost == 0)
                {
                    item.maximumCost = null;
                }
                estimates.Add(item);
            }
            return estimates;
        }

        public int CreateEstimate(Estimate estimate)
        {
            context.Estimates.Add(estimate);
            context.SaveChanges();
            return estimate.estimateId;
        }

    }
}
