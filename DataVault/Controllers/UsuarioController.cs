using Microsoft.AspNetCore.Mvc;
using DataVault.Models;
using DataVault.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DataVault.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cadastro
        public IActionResult Cadastro()
        {
            return View("PagCadastro"); // Se a view estiver na Home
        }

        // POST: Cadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro(Usuario usuario, string ConfirmarSenha)
        {
            if (!ModelState.IsValid)
            {
                return View("PagCadastro", usuario);
            }

            if (usuario.Senha != ConfirmarSenha)
            {
                TempData["Erro"] = "As senhas não coincidem.";
                return View("PagCadastro", usuario);
            }

            if (_context.Usuario.Any(u => u.Email == usuario.Email))
            {
                TempData["Erro"] = "Este e-mail já está cadastrado.";
                return View("PagCadastro", usuario);
            }

            _context.Usuario.Add(usuario);
            _context.SaveChanges();

            TempData["Mensagem"] = "Cadastro realizado com sucesso!";
            return RedirectToAction("PagLogin", "Home");
        }

        // GET: Login
        public IActionResult Login()
        {
            return View("PagLogin"); // Se a view estiver na Home
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string senha)
        {
            var user = _context.Usuario
                .FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (user != null)
            {
                // Cria sessão com ID e NomeCliente
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("NomeCliente", user.nome_cliente);

                return RedirectToAction("Principal", "Home"); // Redireciona para Home/Principal
            }

            TempData["Erro"] = "E-mail ou senha inválidos!";
            return RedirectToAction("PagLogin", "Home");
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("PagLogin", "Home");
        }
    }
}