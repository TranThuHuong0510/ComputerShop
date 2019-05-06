using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Models.Models;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private member variables...
        
        private bool disposed = false;
        private DatabaseContext _dataContext;

        private  IRepository<CompanyInformation> _companyInformationRepository;


        public UnitOfWork()
        {
            _dataContext = new DatabaseContext();
        }

        #endregion

        #region Public Repository Creation properties...
        public IRepository<CompanyInformation> CompanyInformationRepository
        {
            get
            {
                if (this._companyInformationRepository == null)
                    this._companyInformationRepository = new Repository<CompanyInformation>(_dataContext);
                return _companyInformationRepository;
            }
        }
        #endregion

        #region  dispose ...
        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _dataContext.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
