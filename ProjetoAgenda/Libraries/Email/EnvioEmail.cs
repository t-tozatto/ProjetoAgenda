using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProjetoAgenda.Libraries.Email
{
    public static class EnvioEmail
    {
        //id - numeros aleatórios(5) - data(yyyy-MM-dd)
        /// <summary>
        /// Retorna a nova senha do usuário
        /// </summary>
        /// <param name="enviarPara"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public static string EnviarEmail(string enviarPara, string usuario, int idUsuario)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[10];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = chars[random.Next(chars.Length)];

            string codigo = new String(stringChars);

            using (SmtpClient smtpClient = new SmtpClient(ConfiguracaoEmail.SmtpHost, Convert.ToInt32(ConfiguracaoEmail.SmtpPorta)))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(ConfiguracaoEmail.Usuario, ConfiguracaoEmail.Senha);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(enviarPara);
                    mailMessage.From = new MailAddress("thais.c.tozatto@gmail.com");
                    mailMessage.Subject = string.Concat("Recuperação de senha");
                    mailMessage.Body = string.Concat("<!DOCTYPE html><html><head><style>.button {  border: none;  color: white;  padding: 15px 32px;  text-align: center;  text-decoration: none;  display: inline-block;  font-size: 16px;  margin: 4px 2px;  cursor: pointer;}.button1 {background-color: #008CBA;} </style></head><body><p><a>Ol&aacute;,</a></p><p><a>Uma redefini&ccedil;&atilde;o de senha foi solicitada para sua conta.</a></p><p><a>Seu usu&aacute;rio &eacute;:<strong> ", usuario, "</strong></a></p><p><a>Sua nova senha: <strong>", codigo, "</strong></a></p>");
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.Priority = MailPriority.High;

                    smtpClient.Send(mailMessage);
                }
            }
            return codigo;
        }
    }
}
