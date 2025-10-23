using Microsoft.AspNetCore.Mvc;
using DataVault.Data;
using DataVault.Models;
using System.Linq;

namespace DataVault.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Listar(int usuarioId)
        {
            var pagamentos = _context.Pagamentos
                .Where(p => p.IdUsuario == usuarioId)
                .ToList();

            ViewBag.UsuarioId = usuarioId;
            return View(pagamentos);
        }

       
        public IActionResult Criar(int usuarioId)
        {
            ViewBag.UsuarioId = usuarioId;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Pagamento pagamento)
        {
            if (!ModelState.IsValid)
                return View(pagamento);

            _context.Pagamentos.Add(pagamento);
            _context.SaveChanges();

            return RedirectToAction("Listar", new { usuarioId = pagamento.IdUsuario });
        }

        
        public IActionResult Editar(int id)
        {
            var pagamento = _context.Pagamentos.Find(id);
            if (pagamento == null)
                return NotFound();

            return View(pagamento);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Pagamento pagamento)
        {
            if (!ModelState.IsValid)
                return View(pagamento);

            _context.Pagamentos.Update(pagamento);
            _context.SaveChanges();

            return RedirectToAction("Listar", new { usuarioId = pagamento.IdUsuario });
        }

       
        public IActionResult Excluir(int id)
        {
            var pagamento = _context.Pagamentos.Find(id);
            if (pagamento == null)
                return NotFound();

            _context.Pagamentos.Remove(pagamento);
            _context.SaveChanges();

            return RedirectToAction("Listar", new { usuarioId = pagamento.IdUsuario });
        }
    }
}