using Microsoft.AspNetCore.Http;

namespace GameNewsBoard.Application.IServices.Auth;

public interface ICookieService
{
    void SetJwtCookie(HttpResponse response, string token, TimeSpan expiration);
    void ClearJwtCookie(HttpResponse response);
}
