using System;
using System.Collections.Generic;
using System.Linq;
using Library.BLL.Entities;
using Library.BLL.Exceptions;
using Library.BLL.Interfaces;

namespace Library.BLL.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _repository;
        private List<User> _users;
        private List<Document> _documents;
        private List<Loan> _loans;
        private const int MAX_LOANS = 4;

        public LibraryService(ILibraryRepository repository)
        {
            _repository = repository;
            LoadData();
        }

        private void LoadData()
        {
            _users = _repository.GetUsers();
            _documents = _repository.GetDocuments();
            _loans = _repository.GetLoans();
        }

        private void SaveData()
        {
            _repository.SaveUsers(_users);
            _repository.SaveDocuments(_documents);
            _repository.SaveLoans(_loans);
        }

        // --- USERS ---
        public void AddUser(User user)
        {
            if (_users.Any(u => u.Id == user.Id))
                throw new DuplicateEntityException($"Користувач з ID {user.Id} вже існує.");
            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new ValidationException("Ім'я користувача є обов'язковим.");

            _users.Add(user);
            SaveData();
        }

        public void RemoveUser(int id)
        {
            var user = GetUser(id);
            if (_loans.Any(l => l.UserId == id && l.IsActive))
                throw new ValidationException("Неможливо видалити користувача, який має активні позики.");

            _users.Remove(user);
            SaveData();
        }

        public void UpdateUser(User user)
        {
            var existing = GetUser(user.Id);
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.AcademicGroup = user.AcademicGroup;
            SaveData();
        }

        public User GetUser(int id) =>
            _users.FirstOrDefault(u => u.Id == id) ?? throw new EntityNotFoundException("Читач", id);

        public List<User> GetAllUsers() => new List<User>(_users);

        public List<User> GetUsersSorted(string criterion)
        {
            return criterion.ToLower() switch
            {
                "name" => _users.OrderBy(u => u.FirstName).ToList(),
                "lastname" => _users.OrderBy(u => u.LastName).ToList(),
                "group" => _users.OrderBy(u => u.AcademicGroup).ToList(),
                _ => _users
            };
        }

        public List<User> SearchUsers(string keyword)
        {
            var k = keyword.ToLower();
            return _users.Where(u => u.FirstName.ToLower().Contains(k) || u.LastName.ToLower().Contains(k)).ToList();
        }

        // --- DOCUMENTS ---
        public void AddDocument(Document doc)
        {
            if (_documents.Any(d => d.Id == doc.Id))
                throw new DuplicateEntityException($"Документ з ID {doc.Id} вже існує.");

            _documents.Add(doc);
            SaveData();
        }

        public void RemoveDocument(int id)
        {
            var doc = GetDocument(id);
            if (_loans.Any(l => l.DocumentId == id && l.IsActive))
                throw new ValidationException("Неможливо видалити документ, який наразі видано.");

            _documents.Remove(doc);
            SaveData();
        }

        public void UpdateDocument(Document doc)
        {
            var existing = GetDocument(doc.Id);
            existing.Title = doc.Title;
            existing.Author = doc.Author;
            SaveData();
        }

        public Document GetDocument(int id) =>
            _documents.FirstOrDefault(d => d.Id == id) ?? throw new EntityNotFoundException("Документ", id);

        public List<Document> GetAllDocuments() => new List<Document>(_documents);

        public List<Document> GetDocumentsSorted(string criterion)
        {
            return criterion.ToLower() switch
            {
                "title" => _documents.OrderBy(d => d.Title).ToList(),
                "author" => _documents.OrderBy(d => d.Author).ToList(),
                _ => _documents
            };
        }

        public List<Document> SearchDocuments(string keyword)
        {
            var k = keyword.ToLower();
            return _documents.Where(d => d.Title.ToLower().Contains(k) || d.Author.ToLower().Contains(k)).ToList();
        }

        // --- LOANS ---
        public void LoanDocument(int userId, int docId)
        {
            var user = GetUser(userId);
            var doc = GetDocument(docId);

            if (_loans.Count(l => l.UserId == userId && l.IsActive) >= MAX_LOANS)
                throw new LoanLimitExceededException(userId);

            if (!doc.IsAvailable)
                throw new DocumentNotAvailableException(docId);

            var loan = new Loan
            {
                Id = _loans.Count > 0 ? _loans.Max(l => l.Id) + 1 : 1,
                UserId = userId,
                DocumentId = docId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30)
            };

            _loans.Add(loan);
            doc.IsAvailable = false;
            SaveData();
        }

        public void ReturnDocument(int loanId)
        {
            var loan = _loans.FirstOrDefault(l => l.Id == loanId && l.IsActive);
            if (loan == null) throw new EntityNotFoundException("Активна позика", loanId);

            loan.ReturnDate = DateTime.Now;
            var doc = GetDocument(loan.DocumentId);
            doc.IsAvailable = true;
            SaveData();
        }

        public List<Document> GetUserLoans(int userId)
        {
            GetUser(userId);
            var docIds = _loans.Where(l => l.UserId == userId && l.IsActive).Select(l => l.DocumentId);
            return _documents.Where(d => docIds.Contains(d.Id)).ToList();
        }

        public List<Loan> GetActiveLoans() => _loans.Where(l => l.IsActive).ToList();

        public Loan GetDocumentStatus(int docId) =>
            _loans.FirstOrDefault(l => l.DocumentId == docId && l.IsActive);
    }
}