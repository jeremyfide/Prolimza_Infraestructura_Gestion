using BenjaminCamacho.Services;
using System.Text.Encodings.Web;

namespace BenjaminCamacho.Extensions
{
    namespace Microsoft.AspNetCore.Mvc
    {
        public static class EmailSenderExtensions
        {
            public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
            {
                return emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
            }
        }

    }
}
