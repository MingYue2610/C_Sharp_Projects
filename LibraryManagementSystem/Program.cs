using System;
using System.Collections.Generic; 
using System.Text.RegularExpressions; 

namespace LibraryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Initializing Library Management System...");
                
                // Create sample data for testing (optional)
                var librarySystem = new LibrarySystem();
                SeedSampleData(librarySystem);

                // Initialize the UI with the library system
                var libraryUI = new LibraryUI(librarySystem);
                
                // Start the application by showing the menu
                Console.WriteLine("Initialization complete. Starting application...");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                
                // This will start the main menu loop
                libraryUI.ShowMainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("\nThank you for using the Library Management System.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        // Helper method to create some sample data
        private static void SeedSampleData(LibrarySystem librarySystem)
        {
            try
            {
                // Add sample books
                var book1 = new Book("The Great Gatsby", "F. Scott Fitzgerald", "9780743273565", new DateTime(1925, 4, 10));
                var book2 = new Book("To Kill a Mockingbird", "Harper Lee", "9780446310789", new DateTime(1960, 7, 11));
                var book3 = new Book("1984", "George Orwell", "9780451524935", new DateTime(1949, 6, 8));

                librarySystem.AddBook(book1);
                librarySystem.AddBook(book2);
                librarySystem.AddBook(book3);

                // Add sample members
                var member1 = new StudentMember("John Doe", "john.doe@university.edu");
                var member2 = new FacultyMember("Jane Smith", "jane.smith@university.edu");

                librarySystem.AddMember(member1);
                librarySystem.AddMember(member2);

                Console.WriteLine("Sample data loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading sample data: {ex.Message}");
            }
        }
    }
}
