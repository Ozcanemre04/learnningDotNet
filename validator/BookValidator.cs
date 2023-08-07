using FluentValidation;
using dotnet2.dto;
namespace dotnet2.validator
{
    public class BookValidator: AbstractValidator<AddBookDto>
    {
        public BookValidator(){

            RuleFor(u =>u.bookName)
            .NotNull()
            .NotEmpty()
            .WithMessage("bookname is required")
            .Length(3,50);

            RuleFor(u =>u.author)
            .NotNull()
            .NotEmpty()
            .WithMessage("author is required")
            .Length(3,50)
            .Matches(@"^[A-Z][a-z]*(?: [A-Z][a-z]*)*$").WithMessage("invalid author: first letter must be uppercase");
            
        }
    }
}