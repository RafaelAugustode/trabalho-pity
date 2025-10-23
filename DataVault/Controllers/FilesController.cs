using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DataVault.Data;
using DataVault.Models;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using DataVault.Models.Composite;
using Microsoft.EntityFrameworkCore;
using DataVault.Models.Decorator;
using DataVault.Models.Strategy;
using DataVault.Repositories;


namespace DataVault.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly DataVault.Services.IAService _iaService;
        private readonly IFilesRepository _filesRepository;

        public FilesController(ApplicationDbContext context, IWebHostEnvironment env, DataVault.Services.IAService iaService, IFilesRepository filesRepository)
        {
            _context = context;
            _env = env;
            _iaService = iaService;
            _filesRepository = filesRepository;
        }
        private async Task<long> CalcularEspacoUsado()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            return userId.HasValue ? await _filesRepository.GetTotalStorageUsedAsync(userId.Value) : 0;
        }
        
        // GET: Principal (filtro por tipo)
        public async Task<IActionResult> Filter(string tipo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("PagLogin", "Home");

            var arquivos = GetArquivosUsuario(userId.Value);
            var arquivosFiltrados = FiltrarPorTipo(arquivos, tipo);

            ViewBag.Arquivos = arquivosFiltrados;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhFiltro = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }
        private IEnumerable<Files> FiltrarPorTipo(IEnumerable<Files> arquivos, string tipo)
        {
            // Filtra só arquivos (não pastas) e não excluídos
            var arquivosValidos = arquivos.Where(f => !f.IsPasta && !f.Excluido);

            return tipo?.ToLowerInvariant() switch
            {
                "pdf" => arquivosValidos.Where(f =>
                    f.TipoArquivo.Equals(".pdf", StringComparison.OrdinalIgnoreCase)),

                "excel" => arquivosValidos.Where(f =>
                    f.TipoArquivo.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                    f.TipoArquivo.Equals(".xls", StringComparison.OrdinalIgnoreCase)),

                "jpg" => arquivosValidos.Where(f =>
                    f.TipoArquivo.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    f.TipoArquivo.Equals(".jpeg", StringComparison.OrdinalIgnoreCase)),

                "png" => arquivosValidos.Where(f =>
                    f.TipoArquivo.Equals(".png", StringComparison.OrdinalIgnoreCase)),

                "word" => arquivosValidos.Where(f =>
                    f.TipoArquivo.Equals(".docx", StringComparison.OrdinalIgnoreCase) ||
                    f.TipoArquivo.Equals(".doc", StringComparison.OrdinalIgnoreCase)),

                "outros" => arquivosValidos.Where(f =>
                    !new[] { ".pdf", ".xlsx", ".xls", ".jpg", ".jpeg", ".png", ".docx", ".doc" }
                        .Contains(f.TipoArquivo.ToLowerInvariant())),

                _ => arquivos.Where(f => !f.Excluido) // "Recentes" ou vazio → todos
            };
        }

        // POST: Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized("Usuário não logado");

            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Nenhum arquivo selecionado.");

            // --- Salva o arquivo fisicamente ---
            var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
            Directory.CreateDirectory(pastaUsuario);
            var caminhoArquivo = Path.Combine(pastaUsuario, arquivo.FileName);

            using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                await arquivo.CopyToAsync(stream);

            // --- Lê o conteúdo com Strategy ---
            ILeitorArquivoStrategy strategy = Path.GetExtension(arquivo.FileName).ToLower() switch
            {
                 ".pdf" => new LeitorPdf(),
                ".txt" => new LeitorTxtStrategy(),
                ".docx" => new LeitorDocxStrategy(),
                ".pptx" => new LeitorPptxStrategy(),
                ".xml" => new LeitorXmlStrategy(),
                ".sql" => new LeitorSqlStrategy(),
                ".jpg" => new LeitorImagemStrategy(),
                ".jpeg" => new LeitorImagemStrategy(),
                ".png" => new LeitorImagemStrategy(),
                ".word" => new LeitorDocxStrategy(), // trata .word como .docx
                _ => null
            };

            string texto = "";
            if (strategy != null)
            {
                try
                {
                    var leitor = new LeitorArquivoContext(strategy);
                    texto = leitor.Ler(caminhoArquivo);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro ao ler arquivo: {ex.Message}");
                }
            }

            // --- Usa IA para obter tema ---
            string tema = await _iaService.ExtrairTema(texto);

            bool conteudoSensivel = false;
            if (!string.IsNullOrWhiteSpace(texto))
            {
                string promptSensivel = $@"
            Este texto contém informações confidenciais como CPF, RG, senhas, dados bancários, 
            números de cartão de crédito, e-mails pessoais ou privacidade sensível?
            Responda APENAS 'Sim' ou 'Não'.
            Texto: {texto.Substring(0, Math.Min(500, texto.Length))}
        ";

                string respostaSensivel = await _iaService.EnviarPrompt(promptSensivel);
                conteudoSensivel = respostaSensivel.Contains("Sim", StringComparison.OrdinalIgnoreCase);
            }


            // ✅ --- NOVO: Usa IA para obter tags ---
            string tags = "";
            if (!string.IsNullOrWhiteSpace(texto))
            {
                string promptTags = $@"
        Extraia até 5 palavras-chave em português que representem os conceitos principais do texto.
        Retorne APENAS as palavras separadas por vírgulas, sem números, sem explicações, sem aspas.
        Texto: {texto.Substring(0, Math.Min(600, texto.Length))}
    ";

                string tagsBrutas = await _iaService.EnviarPrompt(promptTags);

                var tagsLista = tagsBrutas
                    .Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Take(5)
                    .Select(t => t.Replace(" ", "")) // Remove espaços internos (ex: "Segunda Guerra" → "SegundaGuerra")
                    .ToList();

                tags = string.Join(",", tagsLista);
            }

            // --- Verifica ou cria pasta ---
            var pastaExistente = _context.Files
                .FirstOrDefault(f => f.IsPasta && f.NomeArquivo == tema && f.IdUsuario == userId);

            int pastaId;
            if (pastaExistente != null)
            {
                pastaId = pastaExistente.Id;
                TempData["Mensagem"] = $"Arquivo '{arquivo.FileName}' organizado em: '{tema}'.";
            }
            else
            {
                var novaPasta = new Files
                {
                    NomeArquivo = tema,
                    IsPasta = true,
                    IdUsuario = userId.Value,
                    Excluido = false
                };
                _context.Files.Add(novaPasta);
                await _context.SaveChangesAsync();
                pastaId = novaPasta.Id;
                TempData["Mensagem"] = $"Nova pasta '{tema}' criada!";
            }

            // --- Registra o arquivo no banco ---
            if (!_context.Files.Any(f => f.IdUsuario == userId && f.NomeArquivo == arquivo.FileName))
            {
                var registro = new Files
                {
                    NomeArquivo = arquivo.FileName,
                    TipoArquivo = Path.GetExtension(arquivo.FileName),
                    TamanhoArquivo = arquivo.Length,
                    IdUsuario = userId.Value,
                    ArquivoFile = true,
                    IsPasta = false,
                    PastaPaiId = pastaId,
                    Excluido = false,
                    ConteudoSensivel = conteudoSensivel,
                    Tags = tags // ← AQUI! Salva as tags no banco
                };
                _context.Files.Add(registro);
                await _context.SaveChangesAsync();
            }

            return Content("Upload concluído!");
        }
        // Gleison Pode parecer estranho mas este método é para aparecer as pastas quando clicar no menu pastas
        public async Task<IActionResult> Pastas()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("PagLogin", "Home");

            var pastas = _context.Files
                .Where(f => f.IdUsuario == userId && f.IsPasta && !f.Excluido)
                .ToList();

            ViewBag.Arquivos = pastas;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhPastas = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ObterResumo(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Json(new { erro = "Não autenticado" });

            var arquivo = _context.Files.FirstOrDefault(f => f.Id == id && f.IdUsuario == userId);
            if (arquivo == null || arquivo.IsPasta)
                return Json(new { erro = "Arquivo não encontrado" });

            var caminho = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString(), arquivo.NomeArquivo);
            if (!System.IO.File.Exists(caminho))
                return Json(new { erro = "Arquivo físico não encontrado" });

            ILeitorArquivoStrategy strategy = Path.GetExtension(arquivo.NomeArquivo).ToLower() switch
            {
                ".pdf" => new LeitorPdf(),
                ".txt" => new LeitorTxtStrategy(),
                ".docx" => new LeitorDocxStrategy(),
                ".pptx" => new LeitorPptxStrategy(),
                ".xml" => new LeitorXmlStrategy(),
                ".sql" => new LeitorSqlStrategy(),
                ".jpg" => new LeitorImagemStrategy(),
                ".jpeg" => new LeitorImagemStrategy(),
                ".png" => new LeitorImagemStrategy(),
                ".word" => new LeitorDocxStrategy(), // trata .word como .docx
                _ => null
            };

            if (strategy == null)
                return Json(new { erro = "Formato não suportado" });

            try
            {
                var leitor = new LeitorArquivoContext(strategy);
                string conteudo = leitor.Ler(caminho);
                if (string.IsNullOrWhiteSpace(conteudo))
                    return Json(new { erro = "Conteúdo vazio" });

                var trecho = conteudo.Length > 1200 ? conteudo.Substring(0, 1200) : conteudo;
                string prompt = $@"
            Resuma o seguinte texto em uma frase curta, clara e objetiva em português.
            Não use aspas, não diga 'O texto fala sobre...', apenas o resumo direto.
            Texto: {trecho}
        ";

                string resumo = await _iaService.EnviarPrompt(prompt);
                resumo = resumo.Replace("\"", "").Replace("\n", " ").Trim();

                return Json(new { resumo = resumo });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no resumo: {ex.Message}");
                return Json(new { erro = "Erro ao gerar resumo" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterArquivosDaPasta(int pastaId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var arquivos = await _context.Files
                .Where(f => f.IdUsuario == userId && f.PastaPaiId == pastaId && !f.Excluido && !f.IsPasta)
                .Select(f => new
                {
                    id = f.Id, 
                    nomeArquivo = f.NomeArquivo,
                    tipoArquivo = f.TipoArquivo,
                    tags = f.Tags,
                    conteudoSensivel = f.ConteudoSensivel,
                     favorito = f.Favorito,        
                    compartilhado = f.Compartilhado
                })
                .ToListAsync();

            return Json(arquivos);
        }

        // POST: Search
        [HttpPost]
        public async Task<IActionResult> Search(string Procura)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("PagLogin", "Home");

            var arquivos = GetArquivosUsuario(userId.Value, Procura);

            ViewBag.Arquivos = arquivos;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhPesquisa = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
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
            var query = _context.Files
                .Where(f => f.IdUsuario == userId && !f.Excluido); // só arquivos não excluídos

            if (!string.IsNullOrEmpty(filtro))
            {
                var filtroLower = filtro.ToLower();
                query = query.Where(f => f.NomeArquivo.ToLower().Contains(filtroLower));
            }

            return query.ToList();
        }
        // GET: Principal
        public async Task<IActionResult> Principal()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("PagLogin", "Home");

            var arquivos = GetArquivosUsuario(userId.Value);
            ViewBag.Arquivos = arquivos;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }
        public IActionResult GetTree()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized();

            var userPath = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
            var builder = new FilesTreeBuilder(userPath);
            var tree = builder.BuildTree();

            return View("~/Views/Home/Principal.cshtml", tree);
        }
        public IActionResult GetTreeFiltered(string tipo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            // Monta a árvore de arquivos (Composite)
            var rootFolder = BuildUserFolderTree(userId.Value);

            // Aplica o decorator para filtrar pelo tipo
            IFileFilter filter = new FileFilterBase(rootFolder);
            if (!string.IsNullOrEmpty(tipo))
            {
                filter = new FileTypeFilter(filter, tipo);
            }

            var arquivosFiltrados = filter.GetFiles();

            // Opcional: você pode transformar para FolderComposite se quiser exibir na view como árvore
            var filteredFolder = new FolderComposite("Uploads filtrados");
            foreach (var f in arquivosFiltrados)
            {
                filteredFolder.Add(f);
            }

            return View("Principal", filteredFolder);
        }

        private FolderComposite BuildUserFolderTree(int userId)
        {
            var root = new FolderComposite("Uploads");

            // Pega arquivos do banco
            var arquivos = _context.Files
                .Where(f => f.IdUsuario == userId)
                .ToList();

            foreach (var f in arquivos)
            {
                if (f.IsPasta) continue; // pulando pastas, só arquivos neste exemplo
                root.Add(new FileLeaf(f.NomeArquivo, f.TipoArquivo, f.TamanhoArquivo));
            }

            return root;
        }
        public IActionResult LerArquivo(int id)
        {
            var arquivo = _context.Files.FirstOrDefault(f => f.Id == id);
            if (arquivo == null) return NotFound();

            var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", arquivo.IdUsuario.ToString());
            var caminhoArquivo = Path.Combine(pastaUsuario, arquivo.NomeArquivo);

            ILeitorArquivoStrategy strategy = arquivo.TipoArquivo.ToLower() switch
            {
                ".pdf" => new LeitorPdf(),
                ".txt" => new LeitorTxtStrategy(),
                _ => null
            };

            if (strategy == null) return BadRequest("Tipo de arquivo não suportado.");

            var context = new LeitorArquivoContext(strategy);
            string conteudo = context.Ler(caminhoArquivo);

            ViewBag.Conteudo = conteudo;
            return View("Principal");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleFavorito(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var arquivo = await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.IdUsuario == userId && !f.IsPasta);
            if (arquivo != null)
            {
                arquivo.Favorito = !arquivo.Favorito;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Principal", "Home");
        }

        // Toggle Compartilhado
        [HttpPost]
        public async Task<IActionResult> ToggleCompartilhado(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var arquivo = await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.IdUsuario == userId && !f.IsPasta);
            if (arquivo != null)
            {
                arquivo.Compartilhado = !arquivo.Compartilhado;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Principal", "Home");
        }

        // Excluir (move para lixeira física)
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var arquivo = await _context.Files.FirstOrDefaultAsync(f => f.Id == id && f.IdUsuario == userId && !f.IsPasta);
            if (arquivo != null)
            {
                // Marca como excluído
                arquivo.Excluido = true;

                // Move fisicamente para /Uploads/{userId}/Lixeira/
                var pastaUsuario = Path.Combine(_env.WebRootPath, "Uploads", userId.ToString());
                var pastaLixeira = Path.Combine(pastaUsuario, "Lixeira");
                Directory.CreateDirectory(pastaLixeira);

                var origem = Path.Combine(pastaUsuario, arquivo.NomeArquivo);
                var destino = Path.Combine(pastaLixeira, arquivo.NomeArquivo);

                if (System.IO.File.Exists(origem))
                    System.IO.File.Move(origem, destino, true);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Principal", "Home");
        }

        // Listagem: Favoritos
        public async Task<IActionResult> Favoritos()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("PagLogin", "Home");

            var arquivos = _context.Files
                .Where(f => f.IdUsuario == userId && f.Favorito && !f.IsPasta && !f.Excluido)
                .ToList();

            ViewBag.Arquivos = arquivos;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhFavoritos = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }

        // Listagem: Compartilhados
        public async Task<IActionResult> Compartilhados()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("PagLogin", "Home");

            var arquivos = _context.Files
                .Where(f => f.IdUsuario == userId && f.Compartilhado && !f.IsPasta && !f.Excluido)
                .ToList();

            ViewBag.Arquivos = arquivos;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhCompartilhados = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }

        // Listagem: Lixeira
        public async Task<IActionResult> Lixeira()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("PagLogin", "Home");

            var arquivos = _context.Files
                .Where(f => f.IdUsuario == userId && f.Excluido && !f.IsPasta)
                .ToList();

            ViewBag.Arquivos = arquivos;
            ViewBag.NomeCliente = _context.Usuario.FirstOrDefault(u => u.Id == userId)?.nome_cliente ?? "Usuário";
            ViewBag.EhLixeira = true;
            ViewBag.EspacoUsado = await CalcularEspacoUsado(); // ← ADICIONADO

            return View("~/Views/Home/Principal.cshtml");
        }

       

       
       
    }
}