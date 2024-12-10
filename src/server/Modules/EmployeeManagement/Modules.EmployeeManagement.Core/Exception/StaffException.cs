using System.Net;
using Shared.Core.Exceptions;

namespace Modules.EmployeeManagement.Core.Exception
{
    public class StaffException : CustomException
    {
        public StaffException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message,default, statusCode)
        {
        }
    }
}