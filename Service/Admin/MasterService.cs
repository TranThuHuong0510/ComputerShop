using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Data.UnitOfWork;
using Models.Models;
using Service.Common;
using Service.ViewModels;

namespace Service
{
    public class MasterService
    {
        #region Declare variables

        private readonly UnitOfWork _unitOfWork;

        public MasterService()
        {
            _unitOfWork = new UnitOfWork();
        }
        #endregion

        #region Public method
        public async Task<dynamic> GetAllStorer()
        {
            try
            {
                var x = new Repository<Storer>();
                //var storers = await _unitOfWork.StorerRepository.GetAll();
                var storers = await x.GetAll();

                return storers.Select(y=> new
                {
                    y.Active,
                    y.Description,
                    y.BranchId,
                    y.EditDate
                });
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return null;
            }

        }

        public async Task<bool> InsertBranch(BranchDetailViewModel viewModel)
        {
            try
            {
                var result = false;

                // insert branch
                var branch = new Branch
                {
                    Id = Guid.NewGuid(),
                    BranchName = viewModel.BranchName,
                    Address = viewModel.Address,
                    Active = true,
                    Phone = viewModel.Phone
                };
                var branchRepo = new Repository<Branch>();
                await branchRepo.Insert(branch);
               // await _unitOfWork.BranchRepository.Insert(branch);

                // insert its storer
                var storers = viewModel.Storers;
                var storerRepo = new Repository<Storer>();
                foreach (var storer in storers)
                {
                    var storerz = new Storer
                    {
                        Id = Guid.NewGuid(),
                        BranchId = branch.Id,
                        Description = storer.StorerName,
                        Active = true
                    };
                    
                    await storerRepo.Insert(storerz);
                }

                // save data
               // result = await _unitOfWork.Save();

                return true;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }

        public async Task<IEnumerable<BranchDetail>> GetBranchDetail(Guid branchId)
        {
            try
            {
                var result = false;
                var repository = new Repository<BranchDetail>();
                SqlParameter[] prams =
                {
                    new SqlParameter { ParameterName = "@branchId", Value = branchId, DbType = DbType.Guid }
                };
                var retval = await repository.Get(Constants.StoreProcedure.GET_BRANCH_DETAIL, prams);

                return retval;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return null;
            }

        }
        #endregion

    }
}
