namespace PhoneInfo.API.Domain.Jwt.Response
{
    public class JwtResponseModel
    {
        public string ResponseMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}