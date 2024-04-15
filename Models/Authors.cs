using System.ComponentModel.DataAnnotations;

namespace Library_API_1.Model
{
    public class Authors
    {
        [Key]
        public int AuthorsId { get; set; }
        public string? FullName { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
    }
}
