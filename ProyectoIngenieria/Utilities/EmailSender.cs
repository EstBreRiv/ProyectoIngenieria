using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProyectoIngenieria.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Aca se implementa la funcionalidad de enviar correos

            return Task.CompletedTask;
        }
    }
}
