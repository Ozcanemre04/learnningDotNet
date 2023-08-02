
using dotnet2.dto;

namespace dotnet2.services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooks();
        Task<BookDto> GetBookById(Guid id);

        Task<BookDto> CreateBook(AddBookDto addBookDto);

        Task<String> Delete(Guid id);

        Task<BookDto> UpdateBook(Guid id ,UpdateBookDto updateBookDto);
    }
}