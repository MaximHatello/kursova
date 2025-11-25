using System.Collections.Generic;
using Library.BLL.Entities;
using Library.BLL.Interfaces;

namespace Library.Tests.Mocks
{
    public class MockRepository : ILibraryRepository
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Document> Docs { get; set; } = new List<Document>();
        public List<Loan> Loans { get; set; } = new List<Loan>();

        public List<User> GetUsers() => Users;
        public void SaveUsers(List<User> users) => Users = users;

        public List<Document> GetDocuments() => Docs;
        public void SaveDocuments(List<Document> docs) => Docs = docs;

        public List<Loan> GetLoans() => Loans;
        public void SaveLoans(List<Loan> loans) => Loans = loans;
    }
}