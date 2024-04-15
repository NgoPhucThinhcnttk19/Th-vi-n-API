using System.ComponentModel.DataAnnotations;

namespace Library_API_1.Model
{
    public class Publishers
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Books> Books { get; set; }
    }
}
