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

                return Ok(result);
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
        public async Task<IHttpActionResult> BranchDetails([FromUri] Guid branchId)
        {
            try

            {
                var branch = await _masterService.GetBranchDetail(branchId);
                if (branch == null) return NotFound();
                var result = new ObjectResult
                {
                    StatusCode = 201,
                    Message = "Get data successfully",
                    Result = branch
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