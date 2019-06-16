using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        private IRepository<Branch> _branchRepository;


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

        public IRepository<Branch> BranchRepository
        {
            get
            {
                if (this._branchRepository == null)
                    this._branchRepository = new Repository<Branch>(_dataContext);
                return _branchRepository;
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

               // Logger.CreateLog(Logger.Levels.ERROR, this, "PdctUnitOfWork Save()", e, errString);
                return false;
            }
            catch (Exception ex)
            {
               // Logger.CreateLog(Logger.Levels.ERROR, this, "PdctUnitOfWork Save()", ex, ex.Message);
                return false;
            }

        }

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
