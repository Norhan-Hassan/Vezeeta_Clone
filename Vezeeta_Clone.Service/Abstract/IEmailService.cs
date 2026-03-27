namespace Vezeeta_Clone.Service.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string email, string Message, string? reason);
    }
}
