using System;
using System.Collections.Generic;
using NLibrary;
// ReSharper disable All

namespace NLibrary
{
    public class Book
    {
        public string author;
        public string title;
        public int pages_count;

        public Book(string author, string title, int pages_count, Library lib)
        {
            this.author = author;
            this.title = title;
            this.pages_count = pages_count;
            
            lib.books.Add(this);
            lib.nonexist[this.title] = false;
            Console.WriteLine("В библиотеку добавлена книга "+this.title);
        }
    }

    public class Library
    {
        private static Library library;
        public List<Book> books = new List<Book>();
        public Dictionary<string, bool> nonexist = new Dictionary<string, bool>();

        private  Library() { }

        // реализация синглтона
        public static Library CreateLibrary()
        {
            if(library == null)
                return library = new Library();
            return library;
        }
        //
        
        public void GetBook(Book book, Student student)
        {
            var st_bilet = student.GetBilet();
            Console.WriteLine(st_bilet.student_name + " хочет взять книгу "+book.title);
            if (!BookExist(book))
            {
                Console.WriteLine("Книги нет в наличии");
                return;
            }
            
            Console.WriteLine("\t"+st_bilet.student_name + " забрал книгу");
            student.AddToBooksList(book);
            this.nonexist[book.title] = true;
        }

        public void ReturnBook(Book book, Student student)
        {
            var st_bilet = student.GetBilet();
            Console.WriteLine(st_bilet.student_name + " вернул книгу "+book.title);
            student.RemoveFromBooksList(book);
            this.nonexist[book.title] = false;
        }

        public bool BookExist(Book book)
        {
            if (this.books.IndexOf(book) == -1 || this.nonexist[book.title] == true)
                return false;
            return true;
        }

    }

    public class StudentBilet
    {
        public string student_name;

        public StudentBilet(string name)
        {
            this.student_name = name;
        }
    }
    
    public class Student
    {
        private string name;
        private StudentBilet st_bilet;
        private List<Book> student_books = new List<Book>();

        public Student(string name)
        {
            this.name = name;
            this.st_bilet = new StudentBilet(this.name);
        }

        public StudentBilet GetBilet()
        {
            return this.st_bilet;
        }

        public void AddToBooksList(Book book)
        {
            this.student_books.Add(book);
        }

        public void RemoveFromBooksList(Book book)
        {
            this.student_books.Remove(book);
        }
        
        public void GetBooksList()
        { 
            Console.WriteLine("Список книг пользователя "+this.name+" : ");
            foreach (var book in student_books)
                Console.WriteLine("  "+book.title);
        }
    }
}

namespace MainNamespace
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("\n\t\tWelcome to library\n");
            Library lib = Library.CreateLibrary();
           
            Book b1 = new Book("Bharkava", "Грокаем алгоритмы", 400, lib);
            Book b2 = new Book("Shildt", "С++ Базовый курс", 600, lib);
            Book b3 = new Book("Tanenbaum", "Архитектура компьютера", 800, lib);
            
            Console.WriteLine("\n");
            Student st1 = new Student("Artem");
            Student st2 = new Student("Tolyan");

            lib.GetBook(b1, st1);
            lib.GetBook(b3, st1);
            lib.GetBook(b1, st2);

            st1.GetBooksList();
            st2.GetBooksList();
            
            lib.GetBook(b2, st2);
            st2.GetBooksList();
            
            lib.ReturnBook(b1, st1);
            lib.GetBook(b1, st2);
            
            st1.GetBooksList();
            st2.GetBooksList();
        }
    }
    
}
