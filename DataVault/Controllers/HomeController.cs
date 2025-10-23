using DataVault.Data;
using DataVault.Models;
using DataVault.Repositories; // ← Adicione este using
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilesRepository _filesRepository; // ← Nova dependência
        private readonly ApplicationDbContext _context;
        private readonly IPerfilRepository _perfilRepository;

        // ✅ Único construtor com todas as dependências
        public HomeController(ILogger<HomeController> logger, IFilesRepository filesRepository, ApplicationDbContext context, IPerfilRepository perfilRepository)
        {
            _logger = logger;
            _filesRepository = filesRepository;
            _context = context;
            _perfilRepository = perfilRepository;
        }

        public IActionResult Index()
		{
			return View();
		}
        

        public IActionResult PagCadastro()
        {
            return View();
        }

        public IActionResult PagLogin()
        {
            return View();
        }
        public IActionResult EsqueciSenha()
        {
            return View();
        }
        public async Task<IActionResult> Principal()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("PagLogin", "Home");

            // Carrega o perfil do usuário
            var perfil = await _perfilRepository.GetPerfisByUsuarioAsync(userId.Value);
            var modoSite = perfil.FirstOrDefault()?.ModoSite ?? "Claro";
            var lingua = perfil.FirstOrDefault()?.Lingua ?? "pt";

            ViewBag.ModoSite = modoSite; // ← Passa para a view
            ViewBag.Lingua = lingua;

            // Calcula espaço usado
            var espacoUsado = await _filesRepository.GetTotalStorageUsedAsync(userId.Value);
            ViewBag.EspacoUsado = espacoUsado;

            // Carrega arquivos
            var arquivos = _context.Files
                .Where(f => f.IdUsuario == userId && !f.Excluido)
                .ToList();
            ViewBag.Arquivos = arquivos;

            ViewBag.NomeCliente = _context.Usuario
                .FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";

            return View();
        }
        public async Task<IActionResult> Configuracoes()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("PagLogin", "Home");

            // Carrega o modo do usuário
            var perfil = await _perfilRepository.GetPerfisByUsuarioAsync(userId.Value);
            var modoSite = perfil.FirstOrDefault()?.ModoSite ?? "Claro";
            var lingua = perfil.FirstOrDefault()?.Lingua ?? "pt";

            ViewBag.ModoSite = modoSite;
            ViewBag.Lingua = lingua;

            ViewBag.NomeCliente = _context.Usuario
                .FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";

            return View();
        }
       
        public IActionResult Privacy()
		{
			return View();
		}
        public IActionResult Obrigado()
        {
            return View(); // procura /Views/Home/Obrigado.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
