using GameNewsBoard.Application.Exceptions;
using GameNewsBoard.Domain.Enums;

namespace GameNewsBoard.Application.Validators
{
    public static class YearCategoryValidator
    {
        public static void Validate(YearCategory? yearCategory)
        {
            if (yearCategory == null || !Enum.IsDefined(typeof(YearCategory), yearCategory))
                throw new InvalidYearCategoryException("Categoria de ano inválida.");
        }
    }
}
