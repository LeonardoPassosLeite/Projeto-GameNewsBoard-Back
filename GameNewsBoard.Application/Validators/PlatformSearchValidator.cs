using GameNewsBoard.Application.Exceptions.Domain;

namespace GameNewsBoard.Application.Validators
{
    public static class PlatformSearchValidator
    {
        public static void Validate(Platform? platform, string? searchTerm)
        {
            if (platform == null && string.IsNullOrWhiteSpace(searchTerm))
                throw new InvalidPlatformException("Pelo menos um filtro (plataforma ou termo de busca) deve ser fornecido.");
        }
    }
}
