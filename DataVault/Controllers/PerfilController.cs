using DataVault.Models;
using DataVault.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataVault.Controllers
{
    [Route("Perfil")]
    public class PerfilController : Controller
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilController(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        // =======================
        // Detalhes do perfil
        // =======================
        [HttpGet("Detalhes/{usuarioId}")]
        public async Task<IActionResult> Detalhes(int usuarioId)
        {
            var perfis = await _perfilRepository.GetPerfisByUsuarioAsync(usuarioId);
            var perfil = perfis.FirstOrDefault();

            if (perfil == null)
            {
                perfil = new Perfil
                {
                    IdUsuario = usuarioId,
                    ModoSite = "Claro",
                    Lingua = "PT-BR",
                    HistoricoMensagens = string.Empty
                };
                await _perfilRepository.AddAsync(perfil);
            }

            return View(perfil);
        }

        // =======================
        // Editar perfil (GET)
        // =======================
        [HttpGet("Editar/{usuarioId}")]
        public async Task<IActionResult> Editar(int usuarioId)
        {
            var perfis = await _perfilRepository.GetPerfisByUsuarioAsync(usuarioId);
            var perfil = perfis.FirstOrDefault();

            if (perfil == null)
                return NotFound();

            return View(perfil);
        }

        // =======================
        // Editar perfil (POST)
        // =======================
        [HttpPost("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Perfil perfil)
        {
            if (!ModelState.IsValid)
                return View(perfil);

            var perfis = await _perfilRepository.GetPerfisByUsuarioAsync(perfil.IdUsuario);
            var perfilExistente = perfis.FirstOrDefault(p => p.Id == perfil.Id);

            if (perfilExistente == null)
                return NotFound();

            perfilExistente.ModoSite = perfil.ModoSite;
            perfilExistente.Lingua = perfil.Lingua;
            perfilExistente.HistoricoMensagens = perfil.HistoricoMensagens;

            await _perfilRepository.UpdateAsync(perfilExistente);

            TempData["Mensagem"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Detalhes", new { usuarioId = perfil.IdUsuario });
        }

        // =======================
        // Atualizar Modo (Claro/Escuro) via fetch
        // =======================
        [HttpPost("AtualizarModo")]
        public async Task<IActionResult> AtualizarModo([FromBody] ModoRequest request)
        {
            if (request == null) return BadRequest();

            var perfis = await _perfilRepository.GetPerfisByUsuarioAsync(request.UsuarioId);
            var perfil = perfis.FirstOrDefault();

            // Normaliza o valor do modo para sempre gravar "Claro" ou "Escuro"
            string modoNormalizado = request.Modo?.ToLower() == "escuro" ? "Escuro" : "Claro";

            if (perfil == null)
            {
                perfil = new Perfil
                {
                    IdUsuario = request.UsuarioId,
                    ModoSite = modoNormalizado,
                    Lingua = "PT-BR"
                };
                await _perfilRepository.AddAsync(perfil);
            }
            else
            {
                perfil.ModoSite = modoNormalizado;
                await _perfilRepository.UpdateAsync(perfil);
            }

            return Json(new { sucesso = true, modo = perfil.ModoSite });
        }

        // =======================
        // Atualizar Lingua via fetch
        // =======================
        [HttpPost("AtualizarLingua")]
        public async Task<IActionResult> AtualizarLingua([FromBody] LinguaRequest request)
        {
            System.Diagnostics.Debug.WriteLine($"Atualizando língua: {request?.Lingua} para usuário {request?.UsuarioId}");

            if (request == null || request.UsuarioId == 0)
                return BadRequest();

            var perfis = await _perfilRepository.GetPerfisByUsuarioAsync(request.UsuarioId);
            var perfil = perfis.FirstOrDefault();

            if (perfil == null)
            {
                perfil = new Perfil
                {
                    IdUsuario = request.UsuarioId,
                    Lingua = request.Lingua,
                    ModoSite = "Claro"
                };
                await _perfilRepository.AddAsync(perfil);
            }
            else
            {
                perfil.Lingua = request.Lingua;
                await _perfilRepository.UpdateAsync(perfil);
            }

            return Json(new { sucesso = true, lingua = perfil.Lingua });
        }

        // =======================
        // Classes de request para JSON
        // =======================
        public class ModoRequest
        {
            [JsonPropertyName("usuarioId")] // ← Faz o bind com o JSON do JS
            public int UsuarioId { get; set; }

            [JsonPropertyName("modo")] // ← Opcional, mas recomendado
            public string Modo { get; set; } = string.Empty;
        }

        public class LinguaRequest
        {
            [JsonPropertyName("usuarioId")]
            public int UsuarioId { get; set; }

            [JsonPropertyName("lingua")]
            public string Lingua { get; set; } = string.Empty;
        }
    }
}