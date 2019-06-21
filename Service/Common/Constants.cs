using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public static class Constants
    {
        public static class Message
        {
            public static string InternalServerError = "Internal Server Error Occurs";
            public static string DataValidationError = "Data Is Not Valid";
            public static string NotFound = "Data is not found";
            public static string Forbidden = "You are not allowed to do this action";
            public static string AuthorizationError = "Your are not authorized";
            //public static string InternalServerError = "Internal Server Error Result";
        }

        public static class StoreProcedure
        {
            public static string GET_BRANCH_DETAIL = "dbo.sp_Master_GetBranchDetail";
        }
    }
}
