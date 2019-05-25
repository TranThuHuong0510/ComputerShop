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

        private  IRepository<Company> _companyRepository;
        private IRepository<Storer> _storerRepository;


        public UnitOfWork()
        {
            _dataContext = new DatabaseContext();
        }

        #endregion

        #region Public Repository Creation properties...
        public IRepository<Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                    this._companyRepository = new Repository<Company>(_dataContext);
                return _companyRepository;
            }
        }

        public IRepository<Storer> StorerRepository
        {
            get
            {
                if (this._storerRepository == null)
                    this._storerRepository = new Repository<Storer>(_dataContext);
                return _storerRepository;
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
