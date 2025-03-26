/*
configuracao para usar o Gmail com MailKit
instalar o MailKit (dotnet add package mailkit)
 */
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace Desafio_INOA // nome do projeto
{
    public class EmailService
    {
        public void SendEmail(string recipientAddress, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Cassio", "cssrjpv@gmail.com")); // Substitua pelo seu nome e e-mail do Gmail
            message.To.Add(new MailboxAddress("", recipientAddress));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls); // Servidor SMTP do Gmail com STARTTLS
                    client.Authenticate("cssrjpv@gmail.com", "SUA SENHA");
                    // apagar antes de subir pro git!!!
                    client.Send(message);
                    client.Disconnect(true);
                    System.Console.WriteLine("E-mail enviado com sucesso!");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                }
            }
        }
    }
}