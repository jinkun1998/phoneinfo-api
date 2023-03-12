namespace PhoneInfo.API.Domain.Bases
{
    public class BaseResponseModel
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public BaseResponseModel(int errorCode, string message, object data)
        {
            ErrorCode = errorCode;
            Message = message;
            Data = data;
        }
    }
}