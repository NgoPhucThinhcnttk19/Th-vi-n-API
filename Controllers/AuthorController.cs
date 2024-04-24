using Library_API_1.Data;
using Library_API_1.Models.DOT;
using Microsoft.AspNetCore.Mvc;
using Library_API_1.Models;

namespace Library_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AuthorController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("get-all-authors")]
        public IActionResult GetAllAuthors()
        {
            //var allBooksDomain=_dbContext.Book.ToList();
            var allAuthorsDomain = _dbContext.Authors;
            //Map domain models to DTOs
            var allBooksDTO = allAuthorsDomain.Select(Authors => new AuthorDTO()
            {
                Id = Authors.AuthorsId,
                FullName = Authors.FullName,
                Bookname = Authors.Book_Authors.Select(n => n.Book.Title).ToList(),
            }).ToList();
            return Ok(allAuthorsDomain);

        }
        [HttpGet]
        [Route("get-authors-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            //get book domain model from db
            var authorWithDomain = _dbContext.Authors.Where(n => n.AuthorsId == id);
            if (authorWithDomain == null)
            {
                return NotFound();
            }

            //map domain model to DTO
            var authorWithIdDTO = authorWithDomain.Select(Authors => new AuthorDTO()
            {
                Id = Authors.AuthorsId,
                FullName = Authors.FullName,
                Bookname = Authors.Book_Authors.Select(n => n.Book.Title).ToList(),
            });
            return Ok(authorWithIdDTO);

        }

    }
}