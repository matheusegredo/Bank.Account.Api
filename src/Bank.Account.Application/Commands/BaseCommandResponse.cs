namespace Bank.Application.Commands
{
    public class BaseCommandResponse
    {
        public BaseCommandResponse(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
