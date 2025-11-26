using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System;
using Library.BLL.Services;
using Library.BLL.Entities;
using Library.BLL.Exceptions;
using Library.Tests.Mocks;

namespace Library.Tests
{
    [TestClass]
    public class LibraryServiceTests
    {
        private LibraryService _service;
        private MockRepository _mock;

        [TestInitialize]
        public void Setup()
        {
            _mock = new MockRepository();
            _service = new LibraryService(_mock);
        }

        // --- USER TESTS ---
        [TestMethod]
        public void AddUser_Valid_ShouldAdd()
        {
            _service.AddUser(new User { Id = 1, FirstName = "Test" });
            Assert.AreEqual(1, _mock.Users.Count);
        }

        [TestMethod]
        public void AddUser_DuplicateId_ShouldThrow()
        {

            _service.AddUser(new User { Id = 1, FirstName = "Original" });


            try
            {
                _service.AddUser(new User { Id = 1, FirstName = "Duplicate" });
                Assert.Fail("Очікувалася помилка DuplicateEntityException, але її не було.");
            }
            catch (DuplicateEntityException)
            {
                
            }
            catch (Exception ex)
            {
                Assert.Fail($"Очікувалася DuplicateEntityException, але випала {ex.GetType().Name}");
            }
        }

        [TestMethod]
        public void RemoveUser_NotFound_ShouldThrow()
        {
            try
            {
                _service.RemoveUser(99);
                Assert.Fail("Очікувалася помилка EntityNotFoundException.");
            }
            catch (EntityNotFoundException) { /* OK */ }
        }

        // --- LOAN TESTS ---
        [TestMethod]
        public void LoanDocument_Valid_ShouldIssue()
        {
            _service.AddUser(new User { Id = 1, FirstName = "U" });
            _service.AddDocument(new Document { Id = 1, Title = "D", IsAvailable = true });

            _service.LoanDocument(1, 1);

            Assert.AreEqual(1, _mock.Loans.Count);
            Assert.IsFalse(_mock.Docs[0].IsAvailable);
        }

        [TestMethod]
        public void LoanDocument_LimitExceeded_ShouldThrow()
        {
            _service.AddUser(new User { Id = 1 });
            for (int i = 1; i <= 5; i++)
                _service.AddDocument(new Document { Id = i, IsAvailable = true });

            for (int i = 1; i <= 4; i++)
                _service.LoanDocument(1, i);

            try
            {
                _service.LoanDocument(1, 5);
                Assert.Fail("Очікувалася помилка LoanLimitExceededException.");
            }
            catch (LoanLimitExceededException) { /* OK */ }
        }

        [TestMethod]
        public void ReturnDocument_Valid_ShouldMakeAvailable()
        {
            _service.AddUser(new User { Id = 1 });
            _service.AddDocument(new Document { Id = 1, IsAvailable = true });
            _service.LoanDocument(1, 1);

            var activeLoans = _service.GetActiveLoans();
            int loanId = activeLoans.First().Id;

            _service.ReturnDocument(loanId);

            Assert.IsTrue(_mock.Docs[0].IsAvailable);
            Assert.IsNotNull(_mock.Loans[0].ReturnDate);
        }
    }
}