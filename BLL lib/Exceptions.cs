using System;

namespace Library.BLL.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException(string message) : base(message) { }
    }

    public class EntityNotFoundException : LibraryException
    {
        public EntityNotFoundException(string entity, int id)
            : base($"Сутність '{entity}' з ID {id} не знайдена") { }
    }

    public class LoanLimitExceededException : LibraryException
    {
        public LoanLimitExceededException(int userId)
            : base($"Користувач {userId} досягнув ліміту видачі (4 документи)") { }
    }

    public class DocumentNotAvailableException : LibraryException
    {
        public DocumentNotAvailableException(int docId)
            : base($"Документ {docId} наразі недоступний для видачі") { }
    }

    public class DuplicateEntityException : LibraryException
    {
        public DuplicateEntityException(string message) : base(message) { }
    }

    public class ValidationException : LibraryException
    {
        public ValidationException(string message) : base(message) { }
    }
}