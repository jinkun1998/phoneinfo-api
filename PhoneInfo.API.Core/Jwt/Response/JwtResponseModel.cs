namespace PhoneInfo.API.Core.Jwt.Response
{
    public class JwtResponseModel
    {
        public string ResponseMessage { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
