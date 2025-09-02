using Microsoft.AspNetCore.Mvc;
using DataVault.Data;
using DataVault.Models;
using System.Linq;

namespace DataVault.Controllers
{
    public class PerfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfilController(ApplicationDbContext context)
        {
            _context = context;
        }

   
        public IActionResult Detalhes(int usuarioId)
        {
            var perfil = _context.Perfis.FirstOrDefault(p => p.IdUsuario == usuarioId);
            if (perfil == null)
            {
                
                perfil = new Perfil
                {
                    IdUsuario = usuarioId,
                    ModoSite = "Claro",
                    Lingua = "PT-BR",
                    HistoricoMensagens = string.Empty
                };
                _context.Perfis.Add(perfil);
                _context.SaveChanges();
            }

            return View(perfil);
        }

        
        public IActionResult Editar(int usuarioId)
        {
            var perfil = _context.Perfis.FirstOrDefault(p => p.IdUsuario == usuarioId);
            if (perfil == null)
                return NotFound();

            return View(perfil);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Perfil perfil)
        {
            if (!ModelState.IsValid)
                return View(perfil);

            var perfilExistente = _context.Perfis.FirstOrDefault(p => p.Id == perfil.Id);
            if (perfilExistente == null)
                return NotFound();

            perfilExistente.ModoSite = perfil.ModoSite;
            perfilExistente.Lingua = perfil.Lingua;
            perfilExistente.HistoricoMensagens = perfil.HistoricoMensagens;

            _context.Perfis.Update(perfilExistente);
            _context.SaveChanges();

            TempData["Mensagem"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Detalhes", new { usuarioId = perfil.IdUsuario });
        }
    }
}
