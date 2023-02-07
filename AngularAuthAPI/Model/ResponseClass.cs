using System.Net;

namespace AngularAuthAPI.Model
{
    public class ResponseClass
    {
        public HttpStatusCode statusCode { get; set; } 
        public string? Message { get; set; }
        public dynamic data { get;set ;} 
    }
}
