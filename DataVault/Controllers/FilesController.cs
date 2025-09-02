using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataVault.Data;
using DataVault.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace DataVault.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FilesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Principal
        

        // POST: Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized("Usuário não logado");

            if (arquivo != null && arquivo.Length > 0)
            {
                var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
                if (!Directory.Exists(pastaUsuario))
                    Directory.CreateDirectory(pastaUsuario);

                var caminhoArquivo = Path.Combine(pastaUsuario, arquivo.FileName);
                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                    await arquivo.CopyToAsync(stream);

                // Salva no banco se não existir
                if (!_context.Files.Any(f => f.IdUsuario == userId && f.NomeArquivo == arquivo.FileName))
                {
                    var novoArquivo = new Files
                    {
                        NomeArquivo = arquivo.FileName,
                        TipoArquivo = Path.GetExtension(arquivo.FileName),
                        TamanhoArquivo = arquivo.Length,
                        IdUsuario = userId.Value,
                        ArquivoFile = true,
                        Favorito = false,
                        Compartilhado = false,
                        Excluido = false
                    };

                    _context.Files.Add(novoArquivo);
                    await _context.SaveChangesAsync();
                }

                return Ok("Upload realizado com sucesso!");
            }

            return BadRequest("Nenhum arquivo selecionado.");
        }

        // POST: Search
        [HttpPost]
        public IActionResult Search(string Procura)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var arquivos = GetArquivosUsuario(userId.Value, Procura);
            ViewBag.NomeCliente = _context.Usuario
                .FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";

            return View("~/Views/Home/Principal.cshtml", arquivos);
        }

        // GET: Download by Name (arquivos da pasta física)
        public IActionResult DownloadByName(string fileName)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
            var caminhoArquivo = Path.Combine(pastaUsuario, fileName);

            if (!System.IO.File.Exists(caminhoArquivo))
                return NotFound("Arquivo não encontrado");

            var contentType = "application/octet-stream";
            return PhysicalFile(caminhoArquivo, contentType, fileName);
        }

        // ==========================
        // Método auxiliar para listar arquivos do banco + pasta
        private List<Files> GetArquivosUsuario(int userId, string filtro = null)
        {
            // Arquivos do banco
            var arquivosDb = _context.Files
                .Where(f => f.IdUsuario == userId)
                .OrderByDescending(f => f.Id)
                .ToList();

            if (!string.IsNullOrEmpty(filtro))
            {
                arquivosDb = arquivosDb
                    .Where(f => f.NomeArquivo.Contains(filtro))
                    .ToList();
            }

            // Arquivos da pasta física
            var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
            if (Directory.Exists(pastaUsuario))
            {
                var arquivosFisicos = Directory.GetFiles(pastaUsuario)
                    .Where(f => string.IsNullOrEmpty(filtro) || Path.GetFileName(f).Contains(filtro))
                    .Select(f => new Files
                    {
                        NomeArquivo = Path.GetFileName(f),
                        TipoArquivo = Path.GetExtension(f),
                        TamanhoArquivo = new FileInfo(f).Length,
                        IdUsuario = userId,
                        ArquivoFile = true
                    });

                // Adiciona apenas os que não estão no banco
                arquivosDb.AddRange(
                    arquivosFisicos.Where(f => !arquivosDb.Any(db => db.NomeArquivo == f.NomeArquivo))
                );
            }

            return arquivosDb;
        }
        public IActionResult Principal()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("PagLogin", "Home");

            var arquivosDb = _context.Files
                .Where(f => f.IdUsuario == userId)
                .OrderByDescending(f => f.Id)
                .ToList();

            // Caminho da pasta física do usuário
            var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());

            if (Directory.Exists(pastaUsuario))
            {
                var arquivosFisicos = Directory.GetFiles(pastaUsuario)
                    .Select(f => new Files
                    {
                        NomeArquivo = Path.GetFileName(f),
                        TipoArquivo = Path.GetExtension(f),
                        TamanhoArquivo = new FileInfo(f).Length,
                        IdUsuario = userId.Value,
                        ArquivoFile = true
                    });

                // Adiciona só arquivos que ainda não estão no banco
                arquivosDb.AddRange(
                    arquivosFisicos.Where(f => !arquivosDb.Any(db => db.NomeArquivo == f.NomeArquivo))
                );
            }

            return View("~/Views/Home/Principal.cshtml", arquivosDb);
        }
    }
}