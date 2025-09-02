using Microsoft.AspNetCore.Mvc;
using DataVault.Models;
using DataVault.Data;
using System.Net.Mail;
using System.Net;

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
        public IActionResult SolicitarCodigo(UsuarioRedefinicao model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                TempData["Erro"] = "Informe o e-mail.";
                return RedirectToAction("EsqueciSenha");
            }

            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == model.Email);
            if (usuario == null)
            {
                TempData["Erro"] = "E-mail não cadastrado.";
                return RedirectToAction("EsqueciSenha");
            }

            // Gera código aleatório
            var codigo = new Random().Next(100000, 999999).ToString();

            // Envia o código por e-mail
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("seuemail@dominio.com");
                mail.To.Add(model.Email);
                mail.Subject = "Código de Redefinição de Senha";
                mail.Body = $"Seu código de redefinição de senha é: {codigo}";

                using (var smtp = new SmtpClient("smtp.dominio.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("seuemail@dominio.com", "suasenha");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                TempData["Mensagem"] = "Código enviado para seu e-mail.";
            }
            catch
            {
                TempData["Erro"] = "Erro ao enviar o e-mail. Tente novamente.";
            }

            // Armazena temporariamente o código na sessão para validar na redefinição
            HttpContext.Session.SetString("CodigoRedefinicao", codigo);
            HttpContext.Session.SetString("EmailRedefinicao", model.Email);

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
            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == model.Email);
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