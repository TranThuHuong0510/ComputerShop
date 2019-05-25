using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Data.UnitOfWork;
using Models.Models;

namespace Service
{
    public class StorerService
    {
        #region Declare variables

        private readonly UnitOfWork _unitOfWork;

        public StorerService()
        {
            _unitOfWork = new UnitOfWork();
        }
        #endregion
        public async Task<IEnumerable<Storer>> GetAllStorer()
        {
            try
            {
             //   var x = new Repository<Storer>();
                var storers = await _unitOfWork.StorerRepository.GetAll();
                //var storers = await x.GetAll();

                return storers;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return null;
            }

        }
    }
}
