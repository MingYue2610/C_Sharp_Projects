using System;
using System.Linq;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 

namespace LibraryManagementSystem
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message) { }
    }

    public class Book
    {
        private string _isbn;

        public string BookId { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string ISBN { get; private set; }
        public bool IsAvailable { get; private set; }
        public DateTime PublishDate { get; private set; }

        public Book(string title, string author, string isbn, DateTime publishDate)
        {
            BookId = "B" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            Title = title;
            Author = author;
            ISBN = isbn;
            PublishDate = publishDate;
            IsAvailable = true;
        }

        public void UpdateStatus(bool status) => IsAvailable = status;

        public string GetBookDetails() =>
            $"Title: {Title}\nAuthor: {Author}\nISBN: {ISBN}\nAvailable: {IsAvailable}";
    }

    public abstract class LibraryMember
    {
        public string MemberId { get; private set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public List<BorrowRecord> BorrowHistory { get; private set; }

        protected int MaxBooksAllowed { get; set; }
        protected decimal FineRate { get; set; }

        protected LibraryMember(string name, string email)
        {
            if (string.IsNullOrEmpty(name))
                throw new LibraryException("Name cannot be empty");

            MemberId = "M" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            Name = name;
            Email = email;
            BorrowHistory = new List<BorrowRecord>();
        }

        public virtual bool CanBorrow() =>
            BorrowHistory.Count(r => r.ReturnDate == null) < MaxBooksAllowed;

        public void AddBorrowRecord(BorrowRecord record) => BorrowHistory.Add(record);

        public decimal CalculateFines() =>
            BorrowHistory.Sum(record => record.CalculateFine(FineRate));
    }
    public class Validators
        {
            public static bool ValidateEmail(string email)
            {
                if (string.IsNullOrWhiteSpace(email)) return false;
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            public static bool ValidateISBN(string isbn) => Regex.IsMatch(isbn, @"^\d{13}$");

            public static bool ValidateBookId(string bookId)
            {
                if (string.IsNullOrWhiteSpace(bookId)) return false;
                return Regex.IsMatch(bookId, @"^B[A-Z0-9]{5}$"); // Matches B followed by 5 alphanumeric characters
            }

            public static bool ValidateMemberId(string memberId)
            {
                if (string.IsNullOrWhiteSpace(memberId)) return false;
                return Regex.IsMatch(memberId, @"^M[A-Z0-9]{5}$");  // Matches M followed by 5 alphanumeric characters
            }
        }
    public class StudentMember : LibraryMember
    {
        public StudentMember(string name, string email) : base(name, email)
        {
            MaxBooksAllowed = 3;
            FineRate = 0.50m;
        }
    }

    public class FacultyMember : LibraryMember
    {
        public FacultyMember(string name, string email) : base(name, email)
        {
            MaxBooksAllowed = 5;
            FineRate = 0.25m;
        }

        public override bool CanBorrow() => true;
    }

    public class BorrowRecord
    {
        public Guid RecordId { get; private set; }
        public Book Book { get; private set; }
        public LibraryMember Member { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public BorrowRecord(Book book, LibraryMember member)
        {
            RecordId = Guid.NewGuid();
            Book = book;
            Member = member;
            BorrowDate = DateTime.Now;
            DueDate = BorrowDate.AddDays(14);
        }

        public void ReturnBook()
        {
            if (ReturnDate.HasValue)
                throw new LibraryException("Book already returned");

            ReturnDate = DateTime.Now;
            Book.UpdateStatus(true);
        }

        public decimal CalculateFine(decimal dailyRate)
        {
            if (!ReturnDate.HasValue || ReturnDate <= DueDate)
                return 0;

            int daysLate = (int)(ReturnDate.Value - DueDate).TotalDays;
            return daysLate * dailyRate;
        }
    }

    public class LibrarySystem
    {
        private List<Book> _books;
        private List<LibraryMember> _members;
        private List<BorrowRecord> _activeLoans;

        public LibrarySystem()
        {
            _books = new List<Book>();
            _members = new List<LibraryMember>();
            _activeLoans = new List<BorrowRecord>();
        }

        public void AddBook(Book book) => _books.Add(book);
        public void AddMember(LibraryMember member) => _members.Add(member);

        public List<Book> GetAllBooks() => _books.ToList();
        public List<LibraryMember> GetAllMembers() => _members.ToList();
        public List<BorrowRecord> GetActiveLoans() => _activeLoans.ToList();

        public BorrowRecord LendBook(string bookId, string memberId) // Accept strings
        {
            var book = _books.FirstOrDefault(b => b.BookId == bookId)
                ?? throw new LibraryException("Book not found");

            var member = _members.FirstOrDefault(m => m.MemberId == memberId)
                ?? throw new LibraryException("Member not found");

            if (!book.IsAvailable)
                throw new LibraryException("Book is not available");

            if (!member.CanBorrow())
                throw new LibraryException("Member has reached maximum books allowed");

            var record = new BorrowRecord(book, member);
            book.UpdateStatus(false);
            member.AddBorrowRecord(record);
            _activeLoans.Add(record);

            return record;
        }

        public bool BookExists(string bookId)
        {
            return _books.Any(b => b.BookId == bookId);
        }

        public bool IsBookAvailable(string bookId)
        {
            var book = _books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null) return false;
            
            // Since Book class has IsAvailable property, we can use it directly
            return book.IsAvailable;
        }

        public bool MemberExists(string memberId)
        {
            return _members.Any(m => m.MemberId == memberId);
        }
            public void ReturnBook(Guid recordId)
            {
                var record = _activeLoans.FirstOrDefault(r => r.RecordId == recordId)
                    ?? throw new LibraryException("Borrow record not found");

                record.ReturnBook();
                _activeLoans.Remove(record);
            }

        public List<Book> SearchBooks(string query)
        {
            query = query.ToLower();
            return _books.Where(b =>
                b.Title.ToLower().Contains(query) ||
                b.Author.ToLower().Contains(query) ||
                b.ISBN.Contains(query)
            ).ToList();
        }

        public decimal GetMemberFines(string memberId) // Accept string
        {
            var member = _members.FirstOrDefault(m => m.MemberId == memberId)
                ?? throw new LibraryException("Member not found");

            return member.CalculateFines();
        }
    }
}
