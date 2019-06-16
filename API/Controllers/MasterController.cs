using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common.Object;
using Service;

namespace API.Controllers
{

    [RoutePrefix("api/computer")]
    public class MasterController : ApiController
    {
        private readonly StorerService _pdctBusiness = new StorerService();

        // GET: Master
        [Route("Storer")]
        [HttpGet]
        public async Task<IHttpActionResult> Storer()
        {
            try

            {
                var output = await _pdctBusiness.GetAllStorer();

                var result = new ObjectResult
                {
                    StatusCode = 200,
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
                //return InternalServerErrorResult(this, "PreparePdctArea(PdctHeaderViewModel viewModel)", ex.Message);
            }
        }

    }
}