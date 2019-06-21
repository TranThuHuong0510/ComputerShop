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

                return storers.Select(y => new
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
                 _unitOfWork.BranchRepository.Insert(branch);

                // insert its storer
                var storers = viewModel.Storers;
                foreach (var storer in storers)
                {
                     _unitOfWork.StorerRepository.Insert(new Storer
                    {
                        Id = Guid.NewGuid(),
                        BranchId = branch.Id,
                        Description = storer.StorerName,
                        Active = true,
                        AddDate = DateTime.Now,
                        EditDate =DateTime.Now
                    });
                }

                // save data
                result = await _unitOfWork.Save();

                return result;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }

        public async Task<bool> InsertBranch2(BranchDetailViewModel viewModel)
        {
            try
            {
                var brachRepository = new Repository<Branch>();
                // insert branch
                var branch = new Branch
                {
                    Id = Guid.NewGuid(),
                    BranchName = viewModel.BranchName,
                    Address = viewModel.Address,
                    Active = true,
                    Phone = viewModel.Phone
                };
                await brachRepository.Insert(branch);

                // insert its storer
                var storerRepository = new Repository<Storer>();
                var storers = viewModel.Storers;
                foreach (var storer in storers)
                {
                    await storerRepository.Insert(new Storer
                    {
                        Id = Guid.NewGuid(),
                        BranchId = branch.Id,
                        Description = storer.StorerName,
                        Active = true,
                        AddDate = DateTime.Now,
                        EditDate = DateTime.Now
                    });
                   
                    
                }

                // save data

                return true;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }

        public async Task<BranchDetailViewModel> GetBranchDetail(Guid branchId)
        {
            try
            {
                var brach = await _unitOfWork.BranchRepository
                    .GetById(branchId);

                var storers = await _unitOfWork.StorerRepository
                    .Get(x => x.BranchId == branchId && x.Active);

                var storerViewModels = storers.Select(x => new StorerViewModel
                {
                    StorerId = x.Id,
                    StorerName = x.Description
                });

                var result = new BranchDetailViewModel
                {
                    BranchId = branchId,
                    Address = brach.Address,
                    BranchName = brach.BranchName,
                    Phone = brach.Phone,
                    Storers = storerViewModels
                };
                return result;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return null;
            }

        }

        public async Task<BranchDetailViewModel> GetBranchDetail2(Guid branchId)
        {
            try
            {
                var repository = new Repository<BranchDetail>();
                SqlParameter[] prams =
           {
                new SqlParameter { ParameterName = "@branchId", Value = branchId, DbType = DbType.Guid }
                
            };
                var result = await repository.Get(Constants.StoreProcedure.GET_BRANCH_DETAIL, prams);

                if (!result.Any()) return new BranchDetailViewModel();

                var output = result.GroupBy(x => new { x.BranchId, x.BranchName, x.Phone, x.Address })
                    .Select(y => new BranchDetailViewModel
                    {
                        BranchId = y.Key.BranchId,
                        BranchName = y.Key.BranchName,
                        Address = y.Key.Address,
                        Phone = y.Key.Phone,
                        Storers = y.Select(z => new StorerViewModel
                        {
                            StorerId = z.StorerId,
                            StorerName = z.StorerName
                        })
                    }).FirstOrDefault();
                return output;
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
