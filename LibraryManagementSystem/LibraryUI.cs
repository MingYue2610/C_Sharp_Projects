using System;
using System.Linq;
using System.Collections.Generic; 
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace LibraryManagementSystem
{
    public class LibraryUI
    {
        private readonly LibrarySystem _librarySystem;

        public LibraryUI(LibrarySystem librarySystem)
        {
            _librarySystem = librarySystem;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Library Management System ===");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Add Member");
                Console.WriteLine("3. Lend Book");
                Console.WriteLine("4. Return Book");
                Console.WriteLine("5. Search Books");
                Console.WriteLine("6. View All Books");
                Console.WriteLine("7. View Active Loans");
                Console.WriteLine("8. Check Member Fines");
                Console.WriteLine("9. Exit");
                Console.Write("\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1": AddBook(); break;
                    case "2": AddMember(); break;
                    case "3": LendBook(); break;
                    case "4": ReturnBook(); break;
                    case "5": SearchBooks(); break;
                    case "6": ViewAllBooks(); break;
                    case "7": ViewActiveLoans(); break;
                    case "8": CheckMemberFines(); break;
                    case "9": return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void AddBook()
        {
            bool isInputValid = false;
            string title, author, isbn, dateInput;    // Data type is declare here so its easier to read below
            DateTime publishDate;

            while (!isInputValid) // Keep looping until the user has input the correct option from the available
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Add New Book ===");
                    Console.WriteLine("Type 'R' at any time to return to main menu\n"); //Advising the user they can return to main menu at any time
                    Console.Write("Title: ");

                    title = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(title))
                        throw new LibraryException("Title cannot be empty"); 
                    if (IsReturnCommand(title)) return; // Helper method to return to main menu
                    if (!IsAlphanumeric(title)) // Checks to ensure only numbers and letters are accepted
                        throw new LibraryException("Title must contain only alphanumeric characters.");
                    
                    Console.Write("Author: ");
                    author = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(author))
                        throw new LibraryException("Author cannot be empty");
                    if (IsReturnCommand(author)) return;
                    if (!IsAlphabetic(author))
                        throw new LibraryException("Author name must contain only alphabetic characters.");
                    
                    Console.Write("ISBN (13 digits): ");
                    isbn = Console.ReadLine();
                    if (IsReturnCommand(isbn)) return;
                    if (isbn.Length != 13 || !long.TryParse(isbn, out _))
                        throw new LibraryException("ISBN must be exactly 13 digits.");
                    if (!Validators.ValidateISBN(isbn))
                        throw new LibraryException("Invalid ISBN format");
                    
                    Console.Write("Publish Date (dd-MM-yyyy): ");
                    dateInput = Console.ReadLine();
                    if (IsReturnCommand(dateInput)) return;
                    if (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate))
                        throw new LibraryException("Invalid date format");

                    var book = new Book(title, author, isbn, publishDate);
                    _librarySystem.AddBook(book);

                    Console.WriteLine("\nBook added successfully!");
                    isInputValid = true;
                }
                catch (LibraryException ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("\nPress any key to try again...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private bool IsAlphanumeric(string input)
        {
            return input.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c));
        }

        private bool IsAlphabetic(string input)
        {
            return input.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        private bool IsReturnCommand(string input)
        {
            return input?.ToUpper() == "R";
        }
        private void AddMember()
        {
            bool isInputValid = false;
            string name, email, type;
            while (!isInputValid) 
            {
                try
            {
                Console.Clear();
                Console.WriteLine("=== Add New Member ===");
                Console.WriteLine("Type 'R' at any time to return to main menu\n");
                
                Console.Write("Name: ");
                name = Console.ReadLine();
                if (IsReturnCommand(name)) return;
                if (!IsAlphabetic(name))
                        throw new LibraryException("Your name must contain only alphabetic characters.");
                
                Console.Write("Email: ");
                email = Console.ReadLine();
                if (IsReturnCommand(email)) return;
                if (!Validators.ValidateEmail(email))
                    throw new LibraryException("Invalid email format");

                Console.Write("Type (1 for Student, 2 for Faculty): ");
                type = Console.ReadLine();

                LibraryMember member = type switch
                {
                    "1" => new StudentMember(name, email),
                    "2" => new FacultyMember(name, email),
                    _ => throw new LibraryException("Invalid member type")
                };

                _librarySystem.AddMember(member);
                Console.WriteLine("\nMember added successfully!");
                isInputValid = true;
            }
            catch (LibraryException ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            }
        }

        private void LendBook()
        {
            bool isInputValid = false;
            string bookId, memberId;
            while (!isInputValid) 
            {
                try
            {
                Console.Clear();
                Console.WriteLine("=== Lend Book ===");

                ViewAllBooks();
                Console.Write("\nEnter Book ID (or 'R' to return to main menu): ");
                
                bookId = Console.ReadLine()?.ToUpper(); // Changed from Guid to string
                if (IsReturnCommand(bookId)) return; // Allowing the user to go back to main menu
                
                // Advising user of the correct format
                if (!Validators.ValidateBookId(bookId))
                    throw new LibraryException("Invalid Book ID format. Must be 'B' followed by 5 characters (e.g., B00001)");
                
                // Add book existence check
                if (!_librarySystem.BookExists(bookId))
                    throw new LibraryException("Book not found in the library system.");
                
                // Add book availability check
                if (!_librarySystem.IsBookAvailable(bookId))
                    throw new LibraryException("This book is currently not available for lending.");
                
                ViewAllMembers();
                Console.Write("\nEnter Member ID: ");
                memberId = Console.ReadLine()?.ToUpper(); // Changed from Guid to string
                if (IsReturnCommand(memberId)) return;
                if (!Validators.ValidateMemberId(memberId))
                    throw new LibraryException("Invalid Member ID format. Must be 'M' followed by 5 alphanumeric characters (e.g., M00001)");
                
                // Add member existence check
                if (!_librarySystem.MemberExists(memberId))
                    throw new LibraryException("Member not found in the library system.");

                var record = _librarySystem.LendBook(bookId, memberId); // Pass strings
                Console.WriteLine("\nBook lent successfully!");
                Console.WriteLine($"Due Date: {record.DueDate:dd-MM-yyyy}");
                isInputValid = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            }
        }

        private void ReturnBook()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Return Book ===");
                
                ViewActiveLoans();
                Console.Write("\nEnter Record ID: ");
                if (!Guid.TryParse(Console.ReadLine(), out Guid recordId))
                    throw new LibraryException("Invalid Record ID format");

                _librarySystem.ReturnBook(recordId);
                Console.WriteLine("\nBook returned successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void SearchBooks()
        {
            Console.Clear();
            Console.WriteLine("=== Search Books ===");

            string selectedBook = AutofillBookTitle();
            
            if (!string.IsNullOrEmpty(selectedBook))
            {
                Console.WriteLine($"\nYou selected: {selectedBook}");
            }
            else
            {
                Console.WriteLine("\nNo book selected.");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private string AutofillBookTitle()
        {
            var books = _librarySystem.GetAllBooks().Select(b => b.Title).ToList();
            Console.WriteLine("Search by book title.");

            string input = string.Empty;
            int suggestionIndex = 0;
            List<string> suggestions = new List<string>();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter) // Select title
                {
                    Console.WriteLine();
                    return suggestions.Count > 0 ? suggestions[suggestionIndex] : input;
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0) // Remove last character
                {
                    input = input.Substring(0, input.Length - 1);
                    suggestionIndex = 0;
                }
                else if (key.Key == ConsoleKey.UpArrow) // Navigate up suggestions
                {
                    if (suggestions.Count > 0)
                    {
                        suggestionIndex = (suggestionIndex - 1 + suggestions.Count) % suggestions.Count;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow) // Navigate down suggestions
                {
                    if (suggestions.Count > 0)
                    {
                        suggestionIndex = (suggestionIndex + 1) % suggestions.Count;
                    }
                }
                else if (!char.IsControl(key.KeyChar)) // Add character
                {
                    input += key.KeyChar;
                    suggestionIndex = 0;
                }

                // Clear previous input line and suggestions
                Console.SetCursorPosition(0, 2);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 3);
                Console.Write(new string(' ', Console.WindowWidth * 5)); // Clearing multiple lines

                // Show current input
                Console.SetCursorPosition(0, 2);
                Console.Write($"Enter here: {input}");

                // Update suggestions list
                suggestions = books.Where(b => b.StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToList();

                if (suggestions.Count > 0)
                {
                    Console.SetCursorPosition(0, 3);
                    for (int i = 0; i < suggestions.Count; i++)
                    {
                        if (i == suggestionIndex) // Highlight selected suggestion
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine(suggestions[i]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(suggestions[i]);
                        }
                    }
                }
            }
        }


        private void ViewAllBooks()
        {
            var books = _librarySystem.GetAllBooks();
            DisplayBooks(books);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewAllMembers()
        {
            var members = _librarySystem.GetAllMembers();
            foreach (var member in members)
            {
                Console.WriteLine($"ID: {member.MemberId}");
                Console.WriteLine($"Name: {member.Name}");
                Console.WriteLine($"Email: {member.Email}");
                Console.WriteLine($"Type: {member.GetType().Name}");
                Console.WriteLine();
            }
        }

        private void ViewActiveLoans()
        {
            var loans = _librarySystem.GetActiveLoans();
            foreach (var loan in loans)
            {
                Console.WriteLine($"Record ID: {loan.RecordId}");
                Console.WriteLine($"Book: {loan.Book.Title}");
                Console.WriteLine($"Member: {loan.Member.Name}");
                Console.WriteLine($"Due Date: {loan.DueDate:yyyy-MM-dd}");
                Console.WriteLine();
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void CheckMemberFines()
        {
            bool isInputValid = false;
            string memberId;
            decimal fines;
            while (!isInputValid) 
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=== Check Member Fines ===");
                    
                    ViewAllMembers();
                    Console.Write("\nEnter Member ID: ");
                    memberId = Console.ReadLine(); // Changed from Guid to string
                    if (IsReturnCommand(memberId)) return;


                    fines = _librarySystem.GetMemberFines(memberId); // Pass string
                    Console.WriteLine($"\nTotal Fines: ${fines:F2}");
                    isInputValid = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                }
                
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void DisplayBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.BookId}");
                Console.WriteLine($"Title: {book.Title}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"ISBN: {book.ISBN}");
                Console.WriteLine($"Available: {book.IsAvailable}");
                Console.WriteLine();
            }
        }
    }
}
