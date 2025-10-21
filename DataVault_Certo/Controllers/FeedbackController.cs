using Microsoft.AspNetCore.Mvc;
using DataVault.Data;
using DataVault.Models;
using System.Linq;
using MimeKit;

namespace DataVault.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Listar(int usuarioId)
        {
            var feedbacks = _context.Feedbacks
                .Where(f => f.IdUsuario == usuarioId)
                .OrderByDescending(f => f.DataCriacao)
                .ToList();

            return View(feedbacks);
        }


        [HttpGet]
        public IActionResult Criar()
        {
            return RedirectToAction("Obrigado", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Feedback feedback)
        {
            

            feedback.DataCriacao = DateTime.Now;
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            // ✉️ Opcional: envie um e-mail de confirmação ou notificação
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("DataVault", "rafaelbatatagemeos@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("", "rafaelbatatagemeos@gmail.com")); // você recebe o feedback
                emailMessage.Subject = "Novo Feedback Recebido";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Mensagem: {feedback.Mensagem}\nNível: {feedback.SelecionarNivel}\nUsuário ID: {feedback.IdUsuario}"
                };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("rafaelbatatagemeos@gmail.com", "triscjkbkfvbvwpu");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log opcional
                System.Diagnostics.Debug.WriteLine("Erro ao enviar notificação de feedback: " + ex.Message);
            }

            TempData["Mensagem"] = "Feedback enviado com sucesso!";
            return RedirectToAction("Obrigado", "Home"); // ✅ Redireciona para sua própria view
        }


        public IActionResult Editar(int id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback == null)
                return NotFound();

            return View(feedback);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Feedback feedback)
        {
            if (!ModelState.IsValid)
                return View(feedback);

            var feedbackExistente = _context.Feedbacks.FirstOrDefault(f => f.Id == feedback.Id);
            if (feedbackExistente == null)
                return NotFound();

            feedbackExistente.Mensagem = feedback.Mensagem;
            feedbackExistente.SelecionarNivel = feedback.SelecionarNivel;

            _context.Feedbacks.Update(feedbackExistente);
            _context.SaveChanges();

            TempData["Mensagem"] = "Feedback atualizado com sucesso!";
            return RedirectToAction("Listar", new { usuarioId = feedback.IdUsuario });
        }

      
        public IActionResult Remover(int id)
        {
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == id);
            if (feedback == null)
                return NotFound();

            _context.Feedbacks.Remove(feedback);
            _context.SaveChanges();

            TempData["Mensagem"] = "Feedback removido com sucesso!";
            return RedirectToAction("Listar", new { usuarioId = feedback.IdUsuario });
        }
    }
}