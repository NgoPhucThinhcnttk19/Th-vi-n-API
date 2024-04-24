using Library_API_1.Data;
using Library_API_1.Model;
using Library_API_1.Models.DOT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Bookcontroller : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        public Bookcontroller(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// Get: api/book
        [HttpGet]
        [Route("id")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            // Get Book Domain model form Db
            var bookwithDomain = _dbcontext.Books.Where(n => n.BookId == id);
            if (bookwithDomain == null)
            {
                return NotFound();
            }
            //             Domain model from DTOS
            var bookwithIdDTO = bookwithDomain.Select(Books => new BookDTO()
            {
                Id = Books.BookId,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.Dateread,
                Rate = Books.Rate,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList(),
            });
            return Ok(bookwithIdDTO);  
        }
        [HttpPost("add-book")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            //   DTO to Domain Model 
            var bookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead= addBookRequestDTO.IsRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl= addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublishersID = addBookRequestDTO.PublishersID,
            };
            //Use Domain Model to create book
            _dbcontext.Books.Add(bookDomainModel);
            _dbcontext.SaveChanges();
            foreach (var id in addBookRequestDTO.AuthorId)
            {
                var _book_author = new Book_Author()
                {
                    BookId = bookDomainModel.BookId,
                    AuthorsId = id
                };
                _dbcontext.Book_Authors.Add(_book_author);
                _dbcontext.SaveChanges();
            }
            return Ok();
        }
        [HttpPut("id")]
        public async Task<IActionResult> PutBooks(int id, Books books)
        {
            if (id != books.BookId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(books).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BooksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteBooks(int id)
        {
            var Books = await _dbcontext.Books.FindAsync(id);
            if (Books == null) 
            {
                return NotFound();
            }
            _dbcontext.Books.Remove(Books);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("get-all-books")]
        public IActionResult GetAll()
        {
            var allBooksDomain = _dbcontext.Books;
            var allBooksDTO = allBooksDomain.Select(Books => new BookDTO()
            {
                Id = Books.BookId,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.Dateread.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList(),
            }).ToList();
            return Ok(allBooksDomain);
        }
        
        private bool BooksExists(int id)
		{
			return _dbcontext.Books.Any(e => e.BookId == id);
		}
    }
}