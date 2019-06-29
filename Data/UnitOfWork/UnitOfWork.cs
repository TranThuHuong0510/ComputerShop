using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.Core;
using Data;
using Data.Repository;
using Models.Models;

namespace Data.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private DatabaseContext _dataContext;

        #endregion

        public UnitOfWork()
        {
            _dataContext = new DatabaseContext();
        }

        #region Public Repository Creation properties...
        private GenericRepository<Company> _companyRepository;
        private GenericRepository<Storer> _storerRepository;
        private GenericRepository<Branch> _branchRepository;
        private GenericRepository<Product> _productRepository;


        #endregion

        #region Public member methods...
        public GenericRepository<Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                    this._companyRepository = new GenericRepository<Company>(_dataContext);
                return _companyRepository;
            }
        }

        public GenericRepository<Branch> BranchRepository
        {
            get
            {
                if (this._branchRepository == null)
                    this._branchRepository = new GenericRepository<Branch>(_dataContext);
                return _branchRepository;
            }
        }


        public GenericRepository<Storer> StorerRepository
        {
            get
            {
                if (this._storerRepository == null)
                    this._storerRepository = new GenericRepository<Storer>(_dataContext);
                return _storerRepository;
            }
        }
        public GenericRepository<Product> ProductRepository
        {
            get {
                if (this._productRepository == null)
                    this._productRepository = new GenericRepository<Product>(_dataContext);
                return _productRepository;
            }
        }
        /// <summary>
        /// Save method.
        /// </summary>
        public async Task<bool> Save()
        {
            try
            {
                return _dataContext.SaveChanges() > 0;
            }
            catch (DbEntityValidationException e)
            {
                var errString = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    errString +=
                        string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State) + Environment.NewLine;
                    foreach (var ve in eve.ValidationErrors)
                    {
                        errString +=
                            string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage) +
                            Environment.NewLine;
                    }
                }
                //System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                Logger.CreateLog(Logger.Levels.ERROR, this, "UnitOfWork Save()", e, errString);
                return false;
            }
            catch (Exception ex)
            {
                Logger.CreateLog(Logger.Levels.ERROR, this, "UnitOfWork Save()", ex, ex.Message);
                return false;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

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