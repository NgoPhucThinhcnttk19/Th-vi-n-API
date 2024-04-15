using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_API_1.Model
{
    public class Book_Author
    {
        [Key]
        public int Id { get; set; }
        //foreignkey Khóa ngoại(book)
        public int BookId { get; set; }
        public Books Book { get; set; }
        //foreignkey Khóa ngoại (Author)
        public int AuthorsId { get; set; }
        public Authors Author {  get; set; }
    }
}
