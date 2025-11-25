using System.Collections.Generic;
using Library.BLL.Entities;
using Library.BLL.Interfaces;
using Library.DAL;           

namespace Library.DAL.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly IStorageService<User> _userStorage;
        private readonly IStorageService<Document> _docStorage;
        private readonly IStorageService<Loan> _loanStorage;

        // Конструктор за замовчуванням (композиція) - щоб працювало в ConsoleApp
        public LibraryRepository()
        {
            _userStorage = new FileStorageService<User>("users.json");
            _docStorage = new FileStorageService<Document>("documents.json");
            _loanStorage = new FileStorageService<Loan>("loans.json");
        }

        // Конструктор для ін'єкції (для розширюваності/тестів)
        public LibraryRepository(
            IStorageService<User> u,
            IStorageService<Document> d,
            IStorageService<Loan> l)
        {
            _userStorage = u;
            _docStorage = d;
            _loanStorage = l;
        }

        public List<User> GetUsers() => _userStorage.Load();
        public void SaveUsers(List<User> users) => _userStorage.Save(users);

        public List<Document> GetDocuments() => _docStorage.Load();
        public void SaveDocuments(List<Document> docs) => _docStorage.Save(docs);

        public List<Loan> GetLoans() => _loanStorage.Load();
        public void SaveLoans(List<Loan> loans) => _loanStorage.Save(loans);
    }
}