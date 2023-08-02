using dotnet2.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet2.dto;
using Mapster;
using dotnet2.models;
using dotnet2.services;

namespace dotnet2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BooksController : ControllerBase
    {
         private readonly IBookService _bookService;

         public BooksController(IBookService bookService){
            _bookService = bookService;
         }


         [HttpGet]
         public async Task<ActionResult<IEnumerable<BookDto>>> GetAll(){
            var books = await _bookService.GetAllBooks();
            
            return Ok(books);
         }

         [HttpGet]
         [Route("{id:Guid}")]
         public async Task<ActionResult<BookDto>> GetById([FromRoute] Guid id){
            var book = await _bookService.GetBookById(id);
            if(book == null){
                return NotFound();
            }
            return Ok(book);
         }


         [HttpDelete("{id:Guid}")]
         public async Task<ActionResult<string>> Delete(Guid id){
            var book = await _bookService.Delete(id);
            if(book == null){
                return NotFound();
            }
            return Ok(book);
         }

         [HttpPost]
         public async Task<ActionResult<BookDto>> Create([FromBody] AddBookDto addBookDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
             var book = await _bookService.CreateBook(addBookDto);
             return CreatedAtAction(nameof(GetById), new {id = book.Id},book);
            
         }
         [HttpPut]
         [Route("{id:Guid}")]
         public async Task<ActionResult<BookDto>> UpdateBook([FromRoute] Guid id,[FromBody] UpdateBookDto updateBookDto){
             var book = await _bookService.UpdateBook(id,updateBookDto);
             if(book == null){
                return NotFound();
             }
             return Ok(book);
         }
    }

}