using dotnet.Data;
using dotnet.Models;

namespace dotnet
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.BookAuthors.Any())
            {
                var bookAuthors = new List<BookAuthor>()
                {
                    new BookAuthor()
                    {
                        Book = new Book()
                        {
                            BookTitle = "Pikachu The Electric Mouse",
                            BookPublicationDate = 1996,
                            BookGenres = new List<BookGenre>()
                            {
                                new BookGenre { Genre = new Genre() { GenreName = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review {
                                    BookTitle="Pikachu",
                                    ReviewText = "Pickahu is the best pokemon, because it is electric",
                                    Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" }
                                },
                                new Review {
                                    BookTitle="Pikachu",
                                    ReviewText = "Pickachu is the best a killing rocks",
                                    Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" }
                                },
                                new Review {
                                    BookTitle="Pikachu",
                                    ReviewText = "Pickchu, pickachu, pikachu",
                                    Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" }
                                },
                            }
                        },
                        Author = new Author()
                        {
                            FirstName = "Jack",
                            LastName = "London",
                            BookAuthors = new List<BookAuthor>()
                        }
                    },
                    new BookAuthor()
                    {
                        Book = new Book()
                        {
                            BookTitle = "Stranger",
                            BookPublicationDate = 1996,
                            BookGenres = new List<BookGenre>()
                            {
                                new BookGenre { Genre = new Genre() { GenreName = "Romance"}}
                            },
                            BookAuthors = new List<BookAuthor>()
                            {
                                new BookAuthor { Author = new Author() { FirstName = "John", LastName = "Doe", BookAuthors = new List<BookAuthor>()}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review {
                                    BookTitle= "Squirtle",
                                    ReviewText = "squirtle is the best pokemon, because it is electric",
                                    Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },

                                new Review {
                                    BookTitle= "Squirtle",
                                    ReviewText = "Squirtle is the best a killing rocks",
                                    Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },

                                new Review {
                                    BookTitle= "Squirtle",
                                    ReviewText = "squirtle, squirtle, squirtle",
                                    Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Author = new Author()
                        {
                            FirstName = "Harry",
                            LastName = "Potter",
                            BookAuthors = new List<BookAuthor>()
                        }
                    },
                        new BookAuthor()
                    {
                        Book = new Book()
                        {
                            BookTitle = "Venasuar",
                            BookPublicationDate = 1903,
                            BookGenres = new List<BookGenre>()
                            {
                                new BookGenre { Genre = new Genre() { GenreName = "Leaf"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { BookTitle="Veasaur",
                                ReviewText = "Venasuar is the best pokemon, because it is electric",
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { BookTitle="Veasaur",
                                ReviewText = "Venasuar is the best a killing rocks",
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { BookTitle="Veasaur",
                                ReviewText = "Venasuar, Venasuar, Venasuar",
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Author = new Author()
                        {
                            FirstName = "Ash",
                            LastName = "Ketchum",
                            BookAuthors = new List<BookAuthor>()
                        }
                    }
                };
                dataContext.BookAuthors.AddRange(bookAuthors);
                dataContext.SaveChanges();
            }
        }
    }
}