using Microsoft.AspNetCore.Http;

namespace GameNewsBoard.Application.DTOs.Requests;

public class UploadImageRequest
{
    public IFormFile Image { get; set; } = null!;
}