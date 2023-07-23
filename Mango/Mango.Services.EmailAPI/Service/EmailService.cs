using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _options;

        public EmailService(DbContextOptions<AppDbContext> options)
        {
            _options = options;
        }

        public async Task EmailCartAndLog(CartDTO cartDTO)
        {
            // Template de email
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDTO.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul/>");
            foreach(var item in cartDTO.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Quantity);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEMail(message.ToString(), cartDTO.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string userEmail)
        {
            string message = "User Registration Successful. </br> Email: " + userEmail;
            await LogAndEMail(message, userEmail);
        }

        private async Task<bool> LogAndEMail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_options);

                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
