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

        // ✅ Único construtor com todas as dependências
        public HomeController(ILogger<HomeController> logger, IFilesRepository filesRepository)
        {
            _logger = logger;
            _filesRepository = filesRepository;
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
            if (userId == null || userId == 0)
            {
                return RedirectToAction("PagLogin");
            }

            var arquivos = await _filesRepository.GetFilesByUserIdAsync(userId.Value);
            ViewBag.Arquivos = arquivos;
            return View();
        }
        public IActionResult Configuracoes()
        {
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
