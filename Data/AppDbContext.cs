using Library_API_1.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library_API_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Books> Books { get; set; }
        public DbSet<Book_Author> Book_Authors { get; set; }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Publishers> Publishers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book_Author>()
                 .HasOne(b => b.Book)
                 .WithMany(ba => ba.Book_Authors)
                 .HasForeignKey(bi => bi.BookId);
            modelBuilder.Entity<Book_Author>()
                .HasOne(a => a.Author)
                 .WithMany(ba => ba.Book_Authors)
                 .HasForeignKey(bi => bi.AuthorsId);

        }
    }
}
