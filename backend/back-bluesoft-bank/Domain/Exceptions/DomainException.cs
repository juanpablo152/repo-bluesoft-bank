namespace back_bluesoft_bank.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class InsufficientFundsException : DomainException
{
    public InsufficientFundsException(string message) : base(message) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}

public class ConcurrencyException : DomainException
{
    public ConcurrencyException(string message) : base(message) { }
}
