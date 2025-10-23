function mudarZIndex() {
    document.getElementById('feedback').style.zIndex = '20';
    document.getElementById('feedback').style.opacity = '1';
    document.getElementById('Customizacao').style.zIndex = '-20';
}

function mudarZIndexCust() {
    document.getElementById('feedback').style.zIndex = '-20';
    document.getElementById('feedback').style.opacity = '0';
    document.getElementById('Customizacao').style.zIndex = '20';
}


function ativarmodoclaro() {
    document.body.classList.remove('tema-escuro');
    salvarModo('claro');
}

function ativarmodoescuro() {
    document.body.classList.add('tema-escuro');
    salvarModo('escuro');
}

// Função que chama o endpoint no backend
function salvarModo(modo) {
    fetch('/Perfil/AtualizarModo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            usuarioId: userId, // ← nome correto
            modo: modo
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.sucesso) {
                // Aplica imediatamente na UI
                if (modo === 'escuro') {
                    document.body.classList.add('tema-escuro');
                } else {
                    document.body.classList.remove('tema-escuro');
                }
            }
        });
}

// Opcional: aplicar modo do banco ao carregar a página
/*
window.addEventListener('DOMContentLoaded', () => {
    // Se você passar o modo via ViewBag, por exemplo:
    const modoDoBanco = '@ViewBag.ModoSite'; // "claro" ou "escuro"
    if (modoDoBanco === 'escuro') {
        document.body.classList.add('tema-escuro');
    } else {
        document.body.classList.remove('tema-escuro');
    }
});
*/
// =======================
// IDIOMA
// =======================
const traducoes = {
    'pt': {
        configuracoes: "Configurações",
        customizacao: "Customização",
        conta: "Conta",
        arquivos: "Arquivos",
        configuracoesCliente: "Configurações do Cliente",
        favoritos: "Favoritos",
        brilhoSite: "Brilho do Site",
        feedback: "Feedback",
        feedbackTexto: "Escreva seu feedback abaixo:",
        enviar: "Enviar",
        recomendadas: "Configurações Recomendadas",
        recentes: "Recentes e tópicos comuns",
        idioma: "Língua: Português (Brasil)",
        trocar_lingua: "Trocar Língua",
        portugues: "Português (Brasil)",
        ingles: "Inglês",
        espanhol: "Espanhol",
        frances: "Francês",
        alemao: "Alemão",
        modo: "Site modo: (Modo Light)",
        senha: "Mudar senha",
        fonte: "Fonte: Montserrat(26)",
        ConfiguraçõesDatavault: "Configurações DataVault",
        Trocar_Fonte: "Trocar Fonte",
        Montserrat: "Montserrat",
        Helvetica: "Helvetica",
        Arial: "Arial",
        Times: "Times New Roman",
        Futura: "Futura",
        Modo_Claro: "Modo Claro",
        modo_escuro: "Modo Escuro",
        // Novas palavras
        Recentes: "Recentes",
        Pastas: "Pastas",
        Compartilhados: "Compartilhados",
        Lixeira: "Lixeira",
        Armazenamento: "Armazenamento",
        Outros: "Outros",
        Carregar: "Carregar"
    },
    'en': {
        configuracoes: "Settings",
        customizacao: "Customization",
        conta: "Account",
        arquivos: "Files",
        configuracoesCliente: "Client Settings",
        favoritos: "Favorites",
        brilhoSite: "Site Brightness",
        feedback: "Feedback",
        feedbackTexto: "Write your feedback below:",
        enviar: "Send",
        recomendadas: "Recommended Settings",
        recentes: "Recent and common topics",
        idioma: "Language: English (US)",
        trocar_lingua: "Change Language",
        portugues: "Portuguese (Brazil)",
        ingles: "English",
        espanhol: "Spanish",
        frances: "French",
        alemao: "German",
        modo: "Site mode: (Light Mode)",
        senha: "Change password",
        fonte: "Font: Montserrat(26)",
        ConfiguraçõesDatavault: "DataVault Settings",
        Trocar_Fonte: "Change Font",
        Montserrat: "Montserrat",
        Helvetica: "Helvetica",
        Arial: "Arial",
        Times: "Times New Roman",
        Futura: "Futura",
        Modo_Claro: "Light Mode",
        modo_escuro: "Dark Mode",
        // Novas palavras
        Recentes: "Recent",
        Pastas: "Folders",
        Favoritos: "Favorites",
        Compartilhados: "Shared",
        Lixeira: "Trash",
        Armazenamento: "Storage",
        Outros: "Others",
        Carregar: "Upload"
    },
    'es': {
        configuracoes: "Configuraciones",
        customizacao: "Personalización",
        conta: "Cuenta",
        arquivos: "Archivos",
        configuracoesCliente: "Configuraciones del Cliente",
        favoritos: "Favoritos",
        brilhoSite: "Brillo del Sitio",
        feedback: "Comentarios",
        feedbackTexto: "Escribe tus comentarios abajo:",
        enviar: "Enviar",
        recomendadas: "Configuraciones recomendadas",
        recentes: "Temas recientes y comunes",
        idioma: "Idioma: Español",
        trocar_lingua: "Cambiar idioma",
        portugues: "Portugués (Brasil)",
        ingles: "Inglés",
        espanhol: "Español",
        frances: "Francés",
        alemao: "Alemán",
        modo: "Modo del sitio: (Modo claro)",
        senha: "Cambiar contraseña",
        fonte: "Fuente: Montserrat(26)",
        ConfiguraçõesDatavault: "Configuraciones de DataVault",
        Trocar_Fonte: "Cambiar fuente",
        Montserrat: "Montserrat",
        Helvetica: "Helvetica",
        Arial: "Arial",
        Times: "Times New Roman",
        Futura: "Futura",
        Modo_Claro: "Modo claro",
        modo_escuro: "Modo oscuro",
        // Novas palavras
        Recentes: "Recientes",
        Pastas: "Carpetas",
        Favoritos: "Favoritos",
        Compartilhados: "Compartidos",
        Lixeira: "Papelera",
        Armazenamento: "Almacenamiento",
        Outros: "Otros",
        Carregar: "Cargar"
    },
    'al': {
        configuracoes: "Einstellungen",
        customizacao: "Anpassung",
        conta: "Konto",
        arquivos: "Dateien",
        configuracoesCliente: "Kundeneinstellungen",
        favoritos: "Favoriten",
        brilhoSite: "Helligkeit der Seite",
        feedback: "Feedback",
        feedbackTexto: "Schreiben Sie unten Ihr Feedback:",
        enviar: "Senden",
        recomendadas: "Empfohlene Einstellungen",
        recentes: "Aktuelle und häufige Themen",
        idioma: "Sprache: Deutsch",
        trocar_lingua: "Sprache wechseln",
        portugues: "Portugiesisch (Brasilien)",
        ingles: "Englisch",
        espanhol: "Spanisch",
        frances: "Französisch",
        alemao: "Deutsch",
        modo: "Website-Modus: (Lichtmodus)",
        senha: "Passwort ändern",
        fonte: "Schriftart: Montserrat(26)",
        ConfiguraçõesDatavault: "DataVault-Einstellungen",
        Trocar_Fonte: "Schriftart wechseln",
        Montserrat: "Montserrat",
        Helvetica: "Helvetica",
        Arial: "Arial",
        Times: "Times New Roman",
        Futura: "Futura",
        Modo_Claro: "Heller Modus",
        modo_escuro: "Dunkler Modus",
        // Novas palavras
        Recentes: "Neueste",
        Pastas: "Ordner",
        Favoritos: "Favoriten",
        Compartilhados: "Geteilt",
        Lixeira: "Papierkorb",
        Armazenamento: "Speicher",
        Outros: "Andere",
        Carregar: "Hochladen"
    },
    'fr': {
        configuracoes: "Paramètres",
        customizacao: "Personnalisation",
        conta: "Compte",
        arquivos: "Fichiers",
        configuracoesCliente: "Paramètres du client",
        favoritos: "Favoris",
        brilhoSite: "Luminosité du site",
        feedback: "Retour d'information",
        feedbackTexto: "Écrivez vos commentaires ci-dessous :",
        enviar: "Envoyer",
        recomendadas: "Paramètres recommandés",
        recentes: "Sujets récents et courants",
        idioma: "Langue : Français",
        trocar_lingua: "Changer de langue",
        portugues: "Portugais (Brésil)",
        ingles: "Anglais",
        espanhol: "Espagnol",
        frances: "Français",
        alemao: "Allemand",
        modo: "Mode du site : (Mode clair)",
        senha: "Changer le mot de passe",
        fonte: "Police : Montserrat(26)",
        ConfiguraçõesDatavault: "Paramètres DataVault",
        Trocar_Fonte: "Changer de police",
        Montserrat: "Montserrat",
        Helvetica: "Helvetica",
        Arial: "Arial",
        Times: "Times New Roman",
        Futura: "Futura",
        Modo_Claro: "Mode clair",
        modo_escuro: "Mode sombre",
        // Novas palavras
        Recentes: "Récents",
        Pastas: "Dossiers",
        Favoritos: "Favoris",
        Compartilhados: "Partagés",
        Lixeira: "Corbeille",
        Armazenamento: "Stockage",
        Outros: "Autres",
        Carregar: "Téléverser"
    }
};

function traduzirIdioma(idioma) {
    if (!userId || userId === 0) {
        console.error("userId não definido");
        return;
    }

    fetch('/Perfil/AtualizarLingua', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            usuarioId: userId, // ← nome correto
            lingua: idioma
        })
    })
        .then(res => res.json())
        .then(data => {
            if (data.sucesso) {
                // Atualiza o idioma global
                window.idiomaAtual = idioma;

                // Aplica tradução na UI
                document.querySelectorAll('[data-i18n]').forEach(el => {
                    const chave = el.getAttribute('data-i18n');
                    if (traducoes[idioma] && traducoes[idioma][chave]) {
                        el.textContent = traducoes[idioma][chave];
                    }
                });
            }
        });
}
