﻿using Library_API_1.Data;
using Library_API_1.Model;
using Library_API_1.Models.DOT;

namespace Library_API_1.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
            private readonly AppDbContext _dbContext;
            public SQLBookRepository(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }
        public List<BookWithAuthorAndPublisherDTO> GetAllBooks(string? filterOn = null, string?
 filterQuery = null,
  string? sortBy = null, bool isAscending = true, int pageNumber = 1, int
 pageSize = 1000)
        {
            var allBooks = _dbContext.Books.Select(Books => new
           BookWithAuthorAndPublisherDTO()
            {
                Id = Books.BookId,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.Dateread.Value : null,
                Rate = Books.IsRead ? Books.Rate.Value : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publisher.Name,
                AuthorNames = Books.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false &&
           string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending ? allBooks.OrderBy(x => x.Title) :
                   allBooks.OrderByDescending(x => x.Title);
                }
            }
            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return allBooks.Skip(skipResults).Take(pageSize).ToList();
        }
        public BookWithAuthorAndPublisherDTO GetBookById(int id)
            {
                var bookWithDomain = _dbContext.Books.Where(n => n.BookId == id);
                //Map Domain Model to DTOs
                var bookWithIdDTO = bookWithDomain.Select(book => new
               BookWithAuthorAndPublisherDTO()
                {
                    Id = book.BookId,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.Dateread,
                    Rate = book.Rate,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    PublisherName = book.Publisher.Name,
                    AuthorNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).FirstOrDefault();
                return bookWithIdDTO;
            }
            public AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO)
            {
                //map DTO to Domain Model
                var bookDomainModel = new Books
                {
                    Title = addBookRequestDTO.Title,
                    Description = addBookRequestDTO.Description,
                    IsRead = addBookRequestDTO.IsRead,
                    Dateread = addBookRequestDTO.DateRead,
                    Rate = addBookRequestDTO.Rate,
                    Genre = addBookRequestDTO.Genre,
                    CoverUrl = addBookRequestDTO.CoverUrl,
                    DateAdded = addBookRequestDTO.DateAdded,
                    PublishersID = addBookRequestDTO.PublishersID
                };
                //Use Domain Model to add Book 
                _dbContext.Books.Add(bookDomainModel);
                _dbContext.SaveChanges();
                foreach (var id in addBookRequestDTO.AuthorId)
                {
                    var _book_author = new Book_Author()
                    {
                        BookId = bookDomainModel.BookId,
                        AuthorsId = id
                    };
                    _dbContext.Book_Authors.Add(_book_author);
                    _dbContext.SaveChanges();
                }
                return addBookRequestDTO;
            }
            public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO bookDTO)
            {
                var bookDomain = _dbContext.Books.FirstOrDefault(n => n.BookId == id);
                if (bookDomain != null)
                {
                    bookDomain.Title = bookDTO.Title;
                    bookDomain.Description = bookDTO.Description;
                    bookDomain.IsRead = bookDTO.IsRead;
                    bookDomain.Dateread = bookDTO.DateRead;
                    bookDomain.Rate = bookDTO.Rate;
                    bookDomain.Genre = bookDTO.Genre;
                    bookDomain.CoverUrl = bookDTO.CoverUrl;
                    bookDomain.DateAdded = bookDTO.DateAdded;
                    bookDomain.PublishersID = bookDTO.PublishersID;
                    _dbContext.SaveChanges();
                }
                var authorDomain = _dbContext.Book_Authors.Where(a => a.BookId == id).ToList();
                if (authorDomain != null)
                {
                    _dbContext.Book_Authors.RemoveRange(authorDomain);
                    _dbContext.SaveChanges();
                }
                foreach (var authorid in bookDTO.AuthorId)
                {
                    var _book_author = new Book_Author()
                    {
                        BookId = id,
                        AuthorsId = authorid
                    };
                    _dbContext.Book_Authors.Add(_book_author);
                    _dbContext.SaveChanges();
                }
                return bookDTO;
            }
            public Books? DeleteBookById(int id)
            {
                var bookDomain = _dbContext.Books.FirstOrDefault(n => n.BookId == id);
                if (bookDomain != null)
                {
                    _dbContext.Books.Remove(bookDomain);
                    _dbContext.SaveChanges();
                }
                return bookDomain;
            }
        }
    }
