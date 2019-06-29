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
                var result = await x.Get(Constants.StoreProcedure.TEST);

                //var storers = await _unitOfWork.StorerRepository.GetAll();
                //var storers = await x.GetAll();

                //return storers.Select(y => new
                //{
                //    y.Active,
                //    y.Description,
                //    y.BranchId,
                //    y.EditDate
                //});
                return result;
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

                // save data=>true/false
                result = await _unitOfWork.Save();

                return result;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }
        //
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
                    new SqlParameter { ParameterName = "@branchId", Value = branchId, DbType = DbType.Guid}
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
        #region Insert Product
        public async Task<bool> InsertProduct(ProductViewModel viewModel)
        {
            try
            {
                var result = false;

                // insert product
                var product = new Product
                {
                    ID = Guid.NewGuid(),
                    ProductID = viewModel.ProductID,
                    DescriptionVN = viewModel.DescriptionVN,
                    DescriptionEN = viewModel.DescriptionEN,
                    ProductGroupID = viewModel.ProductGroupID
                };
                _unitOfWork.ProductRepository.Insert(product);

                // save data=>true/false
                result = await _unitOfWork.Save();

                return result;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }
        public async Task<bool> InsertProduct2(ProductViewModel viewModel)
        {
            try
            {
                var productRepository = new Repository<Product>();
                // insert branch
                var product_ = new Product
                {
                    ID = Guid.NewGuid(),
                    ProductID = viewModel.ProductID,
                    DescriptionVN = viewModel.DescriptionVN,
                    DescriptionEN = viewModel.DescriptionEN,
                    ProductGroupID = viewModel.ProductGroupID
                };
                await productRepository.Insert(product_);

                // save data

                return true;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return false;
            }

        }
        #endregion
        #region Get_Product
        public async Task<ProductViewModel> GetProduct(Guid id)
        {
            try
            {
                var product_ = await _unitOfWork.ProductRepository
                    .GetById(id);

                var result = new ProductViewModel
                {
                    ProductID = product_.ProductID,
                    ProductGroupID = product_.ProductGroupID,
                    DescriptionVN = product_.DescriptionVN,
                    DescriptionEN = product_.DescriptionEN
                };
                return result;
            }
            catch (Exception ex)
            {
                //Logger.CreateLog(Logger.Levels.ERROR, this, "GetAllArea(bool? isActive)", ex, "isActive=" + isActive + ", " + ex.Message);
                return null;
            }

        }
        public async Task<ProductViewModel> GetProduct2(string productid)
        {
            try
            {
                var repository = new Repository<ProductViewModel>();

                SqlParameter[] prams =
                {
                    new SqlParameter { ParameterName = "@ProductID", Value = productid, DbType = DbType.String}
                };

                var result = await repository.Get(Constants.StoreProcedure.GET_PRODUCT, prams);

                if (!result.Any()) return new ProductViewModel();

                var output = result.GroupBy(x => new { x.ProductID, x.ProductGroupID, x.DescriptionVN, x.DescriptionEN })
                    .Select(y => new ProductViewModel
                    {
                        ProductID = y.Key.ProductID,
                        ProductGroupID = y.Key.ProductGroupID,
                        DescriptionVN = y.Key.DescriptionVN,
                        DescriptionEN = y.Key.DescriptionEN
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
