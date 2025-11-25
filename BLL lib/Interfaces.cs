using System.Collections.Generic;
using Library.BLL.Entities;

namespace Library.BLL.Interfaces
{
    public interface ILibraryRepository
    {
        List<User> GetUsers();
        void SaveUsers(List<User> users);

        List<Document> GetDocuments();
        void SaveDocuments(List<Document> documents);

        List<Loan> GetLoans();
        void SaveLoans(List<Loan> loans);
    }

    public interface ILibraryService
    {
        // Users
        void AddUser(User user);
        void RemoveUser(int id);
        void UpdateUser(User user);
        User GetUser(int id);
        List<User> GetAllUsers();
        List<User> GetUsersSorted(string criterion); // Name, LastName, Group
        List<User> SearchUsers(string keyword);

        // Documents
        void AddDocument(Document doc);
        void RemoveDocument(int id);
        void UpdateDocument(Document doc);
        Document GetDocument(int id);
        List<Document> GetAllDocuments();
        List<Document> GetDocumentsSorted(string criterion); // Title, Author
        List<Document> SearchDocuments(string keyword);

        // Loans
        void LoanDocument(int userId, int docId);
        void ReturnDocument(int loanId);
        List<Document> GetUserLoans(int userId);
        List<Loan> GetActiveLoans();
        Loan GetDocumentStatus(int docId);


    }
}