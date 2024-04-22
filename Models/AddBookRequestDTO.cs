﻿namespace Library_API_1.Models
{
    public class AddBookRequestDTO
    {
        public List<int> AuthorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public int? Rate { get; set; }
        public int Genre { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public int PublishersID { get; set; }

    }
}
