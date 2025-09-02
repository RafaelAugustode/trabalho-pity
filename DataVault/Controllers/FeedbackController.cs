using Microsoft.AspNetCore.Mvc;
using DataVault.Data;
using DataVault.Models;
using System.Linq;

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

       
        public IActionResult Criar(int usuarioId)
        {
            var feedback = new Feedback
            {
                IdUsuario = usuarioId
            };
            return View(feedback);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            feedback.DataCriacao = DateTime.Now;

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            TempData["Mensagem"] = "Feedback enviado com sucesso!";
            return RedirectToAction("Listar", new { usuarioId = feedback.IdUsuario });
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