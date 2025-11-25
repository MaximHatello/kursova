using System;

namespace Library.BLL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string AcademicGroup { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public override string ToString() => $"{Id}: {FirstName} {LastName}";
    }

    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Year { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        public override string ToString() => $"{Id}: {Title} ({Author})";
    }

    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DocumentId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive => ReturnDate == null;

        public override string ToString()
        {
            return $"Позика ID: {Id} | Читач ID: {UserId} | Документ ID: {DocumentId} | Дата повернення: {DueDate:yyyy-MM-dd} | Статус: {(IsActive ? "АКТИВНА" : $"Повернуто {ReturnDate:yyyy-MM-dd}")}";
        }
    }
}