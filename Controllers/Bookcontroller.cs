using Library_API_1.Data;
using Library_API_1.Model;
using Library_API_1.Models.DOT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Bookcontroller : Controller
    {
        private readonly AppDbContext _dbcontext;
        public Bookcontroller(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// Get: api/book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
        {
            return await _dbcontext.Books.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
            var Books = await _dbcontext.Books.FindAsync(id);
            if (Books == null)
            {
                return NotFound();
            }
            return Books;
        }
        [HttpPost]

        public async Task<ActionResult<Books>> PostBooks(Books books)
        {
            _dbcontext.Books.Add(books);
            await _dbcontext.SaveChangesAsync();
            return CreatedAtAction("GetPost", new { id = books.BookId }, books);
        }
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
        public IActionResult Index()
        {
            return View();
        }
        private bool BooksExists(int id)
		{
			return _dbcontext.Books.Any(e => e.BookId == id);
		}
    }
}