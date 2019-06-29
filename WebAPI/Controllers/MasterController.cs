using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Common.Object;
using Service;
using Service.ViewModels;
using WebAPI.Controllers;

namespace API.Controllers
{

    [RoutePrefix("api/computer")]
    public class MasterController : BaseAPIController
    {
        private readonly MasterService _masterService = new MasterService();

        #region Product_test
        [Route("Product")]
        [HttpPost]
        public async Task<IHttpActionResult> Product([FromBody] ProductViewModel viewModel)
        {
            try

            {
                if (!ModelState.IsValid)
                {

                    return DataValidationError(GetErrorMessages(ModelState));

                }
                var output = await _masterService.InsertProduct(viewModel);
                //var output = await _masterService.InsertBranch2(viewModel);
                if (output == false) return InternalServerErrorResult();
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Save data successfully",
                    Result = output
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult();
            }
        }

        [Route("Product2")]
        [HttpPost]
        public async Task<IHttpActionResult> Product2([FromBody] ProductViewModel viewModel)
        {
            try

            {
                if (!ModelState.IsValid)
                {

                    return DataValidationError(GetErrorMessages(ModelState));

                }
                var output = await _masterService.InsertProduct2(viewModel);
                //var output = await _masterService.InsertBranch2(viewModel);
                if (output == false) return InternalServerErrorResult();
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Save data successfully",
                    Result = output
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult();
            }
        }
        [Route("Get_Product/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get_Product([FromUri] Guid id)
        {
            try
            {
                var output = await _masterService.GetProduct(id);
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Get data successfully",
                    Result = output
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFoundErrorResult();
            }
        }
        [Route("Get_Product2/{ProductID}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get_Product2([FromUri] string ProductID)
        {
            try
            {
                var output = await _masterService.GetProduct2(ProductID);
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Get data successfully",
                    Result = output
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFoundErrorResult();
            }
        }
        #endregion

        #region BRANCH
        // GET: Master
        [Route("Storer")]
        [HttpGet]
        public async Task<IHttpActionResult> Storer()
        {
            try

            {
                var output = await _masterService.GetAllStorer();

                var result = new ObjectResult
                {
                    StatusCode = 999,
                    Message = "Get data successfully",
                    Result = output
                };

                return Ok(output);
            }
            catch (Exception ex)
            {
                var result = new ObjectResult
                {
                    StatusCode = 200,
                    Message = "Get data successfully",
                    Result = null
                };

                return Ok(result);
            }
        }

        [Route("Branch")]
        [HttpPost]
        public async Task<IHttpActionResult> Branch([FromBody] BranchDetailViewModel viewModel)
        {
            try

            {
                if (!ModelState.IsValid)
                {

                    return DataValidationError(GetErrorMessages(ModelState));

                }
                var output = await _masterService.InsertBranch(viewModel);
                //var output = await _masterService.InsertBranch2(viewModel);
                if (output == false) return InternalServerErrorResult();
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Save data successfully",
                    Result = output
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult();
            }
        }

        [Route("Branch/{BranchId}")]
        [HttpGet]
        public IHttpActionResult BranchDetails_([FromUri] Guid branchId)
        {
            try

            {
                //var output = _masterService.GetBranchDetail(branchId);
                var output = _masterService.GetBranchDetail2(branchId);
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Get data successfully",
                    Result = output
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFoundErrorResult();
            }
        }
        #endregion

        #region PrivateMethod
        private string GetErrorMessages(ModelStateDictionary modelState)
        {
            IEnumerable<ModelError> allErrors = modelState.Values.SelectMany(v => v.Errors);
            var messageErrors = string.Empty;
            foreach (var error in allErrors)
            {
                if (error.Exception != null)
                {
                    messageErrors += error.Exception.Message;
                }
                messageErrors += error.ErrorMessage + "; ";
            };
            return messageErrors;
        }
        #endregion
    }
}