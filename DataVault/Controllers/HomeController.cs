using DataVault.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DataVault.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
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
        public IActionResult Principal()
        {
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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
