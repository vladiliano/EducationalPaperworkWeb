using EducationalPaperworkWeb.Domain.Domain.Enums.In_Program_Enums;

namespace EducationalPaperworkWeb.Domain.Domain.Models.ResponseEntities
{
    public class BaseResponse<T> : IBaseResponse<T>
    {
        public string Description { get; set; }
        public OperationStatusCode StatusCode { get; set; }
        public T Data { get; set; }
    }

    public interface IBaseResponse<T>
    {
        string Description { get; }
        OperationStatusCode StatusCode { get; }
        T Data { get; }
    }
}
