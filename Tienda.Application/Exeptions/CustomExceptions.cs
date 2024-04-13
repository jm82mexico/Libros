using FluentValidation.Results;

namespace Tienda.Application.Exeptions
{
    public class DomainException : Exception
    {
        public DomainException(string? message) : base(message)
        {
        }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string? message) : base(message)
        {
        }
    }

    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) is not found")
        {
        }
    }

    public class ValidationExcepction : ApplicationException
    {

        public ValidationExcepction()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationExcepction(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyname = failureGroup.Key;
                var propertyErrors = failureGroup.ToArray();

                Errors.Add(propertyname, propertyErrors);
            }
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}