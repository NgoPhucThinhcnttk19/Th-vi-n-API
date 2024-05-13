using Library_API_1.Model;
using Library_API_1.Models.DOT;

namespace Library_API_1.Repositories
{     public interface IAuthorRepository
        {
            List<AuthorDTO> GellAllAuthors();
            AuthorNoIdDTO GetAuthorById(int id);
            AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO);
            AuthorNoIdDTO UpdateAuthorById(int id, AuthorNoIdDTO authorNoIdDTO);
            Authors? DeleteAuthorById(int id);
        }
    }
