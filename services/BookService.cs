
using Microsoft.EntityFrameworkCore;
using dotnet2.dto;
using dotnet2.models;
using dotnet2.data;
using Mapster;
using AutoMapper;
//without mapster
// commented codes are autoMapper method;
  namespace dotnet2.services;

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper ;

        public BookService(ApplicationDbContext dbContext,IMapper mapper){
            _dbContext = dbContext;
            _mapper = mapper;
         }

     

        public async Task<string> Delete(Guid id)
        {
            var book  = await _dbContext.books.FindAsync(id);
            if(book == null){
                 throw new Exception("book is not found");
            } 
            _dbContext.Remove(book);
            await _dbContext.SaveChangesAsync();
            var deletedMessage = "book deleted";
            return deletedMessage;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            var books = await _dbContext.books.ToListAsync();
             var bookDtos = books.Select(book => new BookDto
        {
            Id = book.Id,
            bookName = book.bookName,
            author = book.author
        });
        // var bookDtos = books.Select(book=>_mapper.Map<BookDto>(book));
            return bookDtos;
        }

        public async Task<BookDto> GetBookById(Guid id)
        {
            var book = await _dbContext.books.FirstOrDefaultAsync(x => x.Id == id);
            if (book is not null) {
               var bookDto = new BookDto 
                 {
                   Id = book.Id,
                   bookName = book.bookName,
                   author = book.author
                 };
            // var bookDto = _mapper.Map<BookDto>(book);
                return bookDto;
            }
                 throw new Exception("book is not found");
        }
    
        public async Task<BookDto> CreateBook(AddBookDto addBookDto)
        {
            var book  = new Books{
                bookName=addBookDto.bookName,
                author=addBookDto.author
            };
            // var book = _mapper.Map<Books>(addBookDto);

            _dbContext.books.Add(book);
            await _dbContext.SaveChangesAsync();
            var bookDto = new BookDto{
                Id= book.Id,
                bookName= book.bookName,
                author=book.author
            };
            // var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }

        public async Task<BookDto> UpdateBook(Guid id ,UpdateBookDto updateBookDto)
        {   
            var book = await _dbContext.books.FindAsync(id);
            if(book == null){
                throw new Exception("book is not found");
            }
            book.bookName= updateBookDto.bookName;
            book.author= updateBookDto.author;
            // _mapper.Map(updateBookDto, book);
            await _dbContext.SaveChangesAsync();
            var bookDto = new BookDto
        {
            Id = book.Id,
            bookName = book.bookName,
            author = book.author,
           
        };
            // var bookDto = _mapper.Map<BookDto>(book);
            return bookDto;
        }
    
}

//with mapster

//  public class BookService : IBookService
//     {
//         private readonly ApplicationDbContext _dbContext;

//         public BookService(ApplicationDbContext dbContext){
//             _dbContext = dbContext;
//          }

     

//         public async Task<string> Delete(Guid id)
//         {
//             var book  = await _dbContext.books.FindAsync(id);
//             if(book == null){
//                  throw new Exception("book is not found");
//             } 
//             _dbContext.Remove(book);
//             await _dbContext.SaveChangesAsync();
//             var deletedMessage = "book deleted";
//             return deletedMessage;
//         }

//         public async Task<IEnumerable<BookDto>> GetAllBooks()
//         {
//             var books = await _dbContext.books.ToListAsync();
//              var bookDtos = books.Adapt<IEnumerable<BookDto>>();
//             return bookDtos;
//         }

//         public async Task<BookDto> GetBookById(Guid id)
//         {
//             var book = await _dbContext.books.FirstOrDefaultAsync(x => x.Id == id);
//             if (book is not null) {
               
//                 return book.Adapt<BookDto>();
//             }
//                  throw new Exception("book is not found");
//         }
    
//         public async Task<BookDto> CreateBook(AddBookDto addBookDto)
//         {
//             var book  = addBookDto.Adapt<Books>();
//             _dbContext.books.Add(book);
//             await _dbContext.SaveChangesAsync();
//             var bookDto = book.Adapt<BookDto>();
//             return bookDto;
//         }

//         public async Task<BookDto> UpdateBook(Guid id ,UpdateBookDto updateBookDto)
//         {   
//             var book = await _dbContext.books.FindAsync(id);
//             if(book == null){
//                 throw new Exception("book is not found");
//             }
//             var updatedBook = updateBookDto.Adapt(book);
//             await _dbContext.SaveChangesAsync();
//             var bookDto = book.Adapt<BookDto>();
//             return bookDto;
//         }
    
// }