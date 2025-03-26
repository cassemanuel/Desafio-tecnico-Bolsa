/*
configuracao para usar o Gmail com MailKit
instalar o MailKit (dotnet add package mailkit)
 */
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Desafio_INOA // nome do projeto
{
    public class EmailService
    {
        public async Task SendEmail( //async pro smtp não travar o código se demorar
            string smtpServer,
            int smtpPort,
            bool useSsl,
            string senderEmail,
            string senderPassword,
            string receiverEmail,
            string subject,
            string body
        )
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Monitor B3", senderEmail));
                message.To.Add(new MailboxAddress("", receiverEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, smtpPort, useSsl);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    System.Console.WriteLine("E-mail enviado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
