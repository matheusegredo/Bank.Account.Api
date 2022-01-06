namespace Bank.CrossCutting.Exceptions
{
    public class ForbidenRequestException : Exception
    {
        public ForbidenRequestException(string message) : base(message)
        {
        }
    }
}
