using Microsoft.AspNetCore.Mvc;
using DataVault.Models;
using DataVault.Data;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;

namespace DataVault.Controllers
{
    public class UsuarioRedefinicaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRedefinicaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exibe a view EsqueciSenha
        public IActionResult EsqueciSenha()
        {
            return View("~/Views/Home/EsqueciSenha.cshtml");
        }

        // POST: Solicitar código
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitarCodigo(UsuarioRedefinicao model) // ⚠️ async!
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                TempData["Erro"] = "Informe o e-mail.";
                return RedirectToAction("EsqueciSenha");
            }

            var usuario = _context.Usuario.FirstOrDefault(u =>
                u.Email.ToLower() == model.Email.ToLower());

            if (usuario == null)
            {
                TempData["Mensagem"] = "Se o e-mail estiver cadastrado, enviamos um código.";
                return RedirectToAction("EsqueciSenha");
            }

            var codigo = new Random().Next(100000, 999999).ToString();

            var gmailEmail = "rafaelbatatagemeos@gmail.com";
            var gmailAppPassword = "triscjkbkfvbvwpu";

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("DataVault", gmailEmail));
                emailMessage.To.Add(new MailboxAddress("", model.Email));
                emailMessage.Subject = "Código de Redefinição de Senha";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Seu código de redefinição de senha é: {codigo}"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(gmailEmail, gmailAppPassword);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                HttpContext.Session.SetString("CodigoRedefinicao", codigo);
                HttpContext.Session.SetString("EmailRedefinicao", model.Email);

                TempData["Mensagem"] = "Código enviado para seu e-mail.";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro MailKit: " + ex.ToString());
                TempData["Erro"] = "Erro ao enviar e-mail. Tente novamente.";
                return RedirectToAction("EsqueciSenha");
            }

            return RedirectToAction("EsqueciSenha");
        }

        // POST: Redefinir senha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedefinirSenha(UsuarioRedefinicao model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Codigo)
                || string.IsNullOrEmpty(model.NovaSenha) || string.IsNullOrEmpty(model.ConfirmarSenha))
            {
                TempData["Erro"] = "Preencha todos os campos.";
                return RedirectToAction("EsqueciSenha");
            }

            var emailSessao = HttpContext.Session.GetString("EmailRedefinicao");
            var codigoSessao = HttpContext.Session.GetString("CodigoRedefinicao");

            if (emailSessao != model.Email || codigoSessao != model.Codigo)
            {
                TempData["Erro"] = "E-mail ou código incorretos.";
                return RedirectToAction("EsqueciSenha");
            }

            if (model.NovaSenha != model.ConfirmarSenha)
            {
                TempData["Erro"] = "As senhas não coincidem.";
                return RedirectToAction("EsqueciSenha");
            }

            // Atualiza a senha do usuário
            var usuario = _context.Usuario.FirstOrDefault(u =>
     u.Email.ToLower() == model.Email.ToLower());
            if (usuario != null)
            {
                usuario.Senha = model.NovaSenha;
                _context.SaveChanges();

                TempData["Mensagem"] = "Senha redefinida com sucesso!";
            }

            // Remove o código da sessão após uso
            HttpContext.Session.Remove("CodigoRedefinicao");
            HttpContext.Session.Remove("EmailRedefinicao");

            return RedirectToAction("PagLogin", "Home");
        }
    }
}