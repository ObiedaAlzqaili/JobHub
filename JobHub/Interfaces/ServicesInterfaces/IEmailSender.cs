namespace JobHub.Interfaces.ServicesInterfaces
{
    public interface IEmailSender
    {
        public  Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
