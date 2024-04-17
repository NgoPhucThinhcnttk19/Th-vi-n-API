using System.ComponentModel.DataAnnotations;
namespace Library_API_1.Model
{
    public class Books
    {
        [Key]
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? Dateread { get; set; }
        public int? Rate { get; set; }
        public int Genre {  get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
       public int PublishersID { get; set; }
        public Publishers Publisher { get; set; }
        public List<Book_Author> Book_Authors { get; set; }
    }
}
