using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetoAgenda.Libraries;
using ProjetoAgenda.Libraries.Email;
using ProjetoAgenda.Libraries.Login;
using ProjetoAgenda.Models;

namespace ProjetoAgenda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string urlBase = "https://localhost:44390/api/v1/";
        private LoginUsuario _loginUsuario;

        public HomeController(ILogger<HomeController> logger, LoginUsuario loginUsuario)
        {
            _logger = logger;
            _loginUsuario = loginUsuario;
        }

        public IActionResult Index()
        {
            Usuario usuario = _loginUsuario.GetUsuario();
            if (usuario == null)
            {
                ViewBag.Layout = "_Layout";
                return View();
            }
            else
                return RedirectToAction(nameof(Painel));
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] Usuario usuario)
        {
            ViewBag.Layout = "_Layout";
            if (string.IsNullOrWhiteSpace(usuario.Nome) || usuario.Nome.Length < 4 || usuario.Nome.Length > 256)
                ViewData["MSG_E"] = "Necessário um usuário válido para logar.";
            else if (string.IsNullOrWhiteSpace(usuario.Senha) || usuario.Senha.Length < 4 || usuario.Senha.Length > 40)
                ViewData["MSG_E"] = "Necessário uma senha válida para logar.";
            else
            {
                usuario.EncodeSenha();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync(string.Concat(urlBase, "usuario/", usuario.Nome, "/", usuario.Senha)))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                usuario = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                                _loginUsuario.Login(usuario);
                                ViewBag.Login = true;
                                return RedirectToAction(nameof(Painel));
                            }
                            else if (response.StatusCode == HttpStatusCode.NotFound)
                                ViewData["MSG_E"] = "Usuário não encontrado.";
                            else
                                ViewData["MSG_E"] = "Erro ao logar. Tente novamente mais tarde!";
                        }
                    }
                }
                catch
                {
                    ViewData["MSG_E"] = "Erro ao logar. Tente novamente mais tarde!";
                }
            }
            return View();
        }

        public IActionResult CadastroUsuario()
        {
            ViewBag.Layout = "_Layout";
            Usuario usuario = _loginUsuario.GetUsuario();
            if (usuario == null)
                return View();
            else
                return View(usuario);
        }

        public IActionResult CadastroContato()
        {

            ViewBag.Layout = "_LayoutLogin";
            return View();
        }

        public async Task<IActionResult> Painel()
        {
            ViewBag.Layout = "_LayoutLogin";
            Usuario usuario = _loginUsuario.GetUsuario();
            if (usuario == null)
                return RedirectToAction(nameof(Index));

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(string.Concat(urlBase, "contato/", usuario.Id)))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                            return View(JsonConvert.DeserializeObject<List<Contato>>(response.Content.ReadAsStringAsync().Result));
                    }
                }
            }
            catch
            {
                ViewData["MSG_E"] = "Erro ao recuperar lista de contatos. Tente novamente mais tarde!";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastroUsuario([FromForm] Usuario usuario)
        {
            ViewBag.Layout = "_Layout";
            if (ModelState.IsValid)
            {
                usuario.EncodeSenha();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync(string.Concat(urlBase, "usuario/"), new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.Created)
                            {
                                _loginUsuario.Login(usuario);
                                ViewBag.Login = true;
                                return RedirectToAction(nameof(Painel));
                            }
                            else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                            {
                                RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                                ViewData["MSG_E"] = retornoErroAPI.Detail;
                            }
                            else
                                ViewData["MSG_E"] = "Erro ao cadastrar usuário. Tente novamente mais tarde!";
                        }
                    }
                }
                catch
                {
                    ViewData["MSG_E"] = "Erro ao cadastrar usuário. Tente novamente mais tarde!";
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AlterarUsuario()
        {
            Usuario usuario = _loginUsuario.GetUsuario();
            if (usuario == null)
                return RedirectToAction(nameof(Index));
            else
            {
                ViewBag.Layout = "_LayoutLogin";
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AlterarUsuario([FromForm] Usuario usuario)
        {
            ViewBag.Layout = "_LayoutLogin";
            if (ModelState.IsValid)
            {
                usuario.EncodeSenha();
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PutAsync(string.Concat(urlBase, "usuario/", usuario.Id), new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                _loginUsuario.Login(usuario);
                                ViewBag.Login = true;
                                return RedirectToAction(nameof(Painel));
                            }
                            else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                            {
                                RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                                ViewData["MSG_E"] = retornoErroAPI.Detail;
                            }
                            else
                                ViewData["MSG_E"] = "Erro ao alterar usuário. Tente novamente mais tarde!";
                        }
                    }
                }
                catch
                {
                    ViewData["MSG_E"] = "Erro ao alterar usuário. Tente novamente mais tarde!";
                }
            }
            return View(usuario);
        }


        public IActionResult Logout()
        {
            _loginUsuario.Logout();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CadastroContato([FromForm] Contato contato)
        {
            ViewBag.Layout = "_LayoutLogin";
            if (ModelState.IsValid)
            {
                contato.IdUsuario = _loginUsuario.GetUsuario().Id;
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync(string.Concat(urlBase, "contato/"), new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.Created)
                                return RedirectToAction(nameof(Painel));
                            else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                            {
                                RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                                ViewData["MSG_E"] = retornoErroAPI.Detail;
                            }
                            else
                                ViewData["MSG_E"] = "Erro ao cadastrar usuário. Tente novamente mais tarde!";
                        }
                    }
                }
                catch
                {
                    ViewData["MSG_E"] = "Erro ao cadastrar usuário. Tente novamente mais tarde!";
                }
            }
            return View();
        }

        public async Task<IActionResult> ExcluirContato(int idContato, int idUsuario)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(string.Concat(urlBase, "contato/", idContato, "/", idUsuario)))
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            ViewData["MSG_E"] = "Erro ao excluir contato. Tente novamente mais tarde!";
                    }
                }
            }
            catch
            {
                ViewData["MSG_E"] = "Erro ao excluir contato. Tente novamente mais tarde!";
            }
            return RedirectToAction(nameof(Painel));
        }

        public async Task<IActionResult> AlterarContato(int id, int idUsuario)
        {
            ViewBag.Layout = "_LayoutLogin";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(string.Concat(urlBase, "contato/", id, "/", idUsuario)))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                            return View(JsonConvert.DeserializeObject<Contato>(response.Content.ReadAsStringAsync().Result));
                        else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                        {
                            RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                            ViewData["MSG_E"] = retornoErroAPI.Detail;
                        }
                        else
                            ViewData["MSG_E"] = "Erro ao alterar contato. Tente novamente mais tarde!";
                    }
                }
            }
            catch
            {
                ViewData["MSG_E"] = "Erro ao alterar contato. Tente novamente mais tarde!";
            }

            return RedirectToAction(nameof(Painel));
        }

        [HttpPost]
        public async Task<IActionResult> AlterarContato([FromForm] Contato contato)
        {
            ViewBag.Layout = "_LayoutLogin";
            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PutAsync(string.Concat(urlBase, "contato/", contato.Id), new StringContent(JsonConvert.SerializeObject(contato), Encoding.UTF8, "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                                return RedirectToAction(nameof(Painel));
                            else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                            {
                                RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                                ViewData["MSG_E"] = retornoErroAPI.Detail;
                            }
                            else
                                ViewData["MSG_E"] = "Erro ao alterar contato. Tente novamente mais tarde!";
                        }
                    }
                }
                catch
                {
                    ViewData["MSG_E"] = "Erro ao alterar contato. Tente novamente mais tarde!";
                }
            }
            return View(contato);
        }

        public IActionResult RecuperaSenha()
        {
            ViewBag.Layout = "_Layout";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperaSenha([FromForm] Usuario usuario)
        {
            try
            {
                ViewBag.Layout = "_Layout";
                if (!string.IsNullOrWhiteSpace(usuario.Email))
                {
                    if (string.IsNullOrWhiteSpace(ConfiguracaoEmail.Senha) || string.IsNullOrWhiteSpace(ConfiguracaoEmail.SmtpHost) || string.IsNullOrWhiteSpace(ConfiguracaoEmail.SmtpPorta) || string.IsNullOrWhiteSpace(ConfiguracaoEmail.Usuario))
                        ViewData["MSG_E"] = "Necessário ter um e-mail configurado no sistema para fazer o envio da recuperação da senha.";
                    else
                    {
                        using (var httpClient = new HttpClient())
                        {
                            using (var response = await httpClient.GetAsync(string.Concat(urlBase, "usuario/recuperacao/", usuario.Email)))
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    usuario = JsonConvert.DeserializeObject<Usuario>(response.Content.ReadAsStringAsync().Result);
                                    usuario.Senha = EnvioEmail.EnviarEmail(usuario.Email, usuario.Nome, usuario.Id);

                                    usuario.EncodeSenha();
                                    using (var httpClient2 = new HttpClient())
                                    {
                                        using (var response2 = await httpClient.PutAsync(string.Concat(urlBase, "usuario/", usuario.Id), new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json")))
                                        {
                                            if (response2.StatusCode == HttpStatusCode.OK)
                                                ViewData["MSG_S"] = "Cheque seu e-mail para recuperação da senha.";
                                            else if (response.StatusCode == HttpStatusCode.NotAcceptable)
                                            {
                                                RetornoErroAPI retornoErroAPI = JsonConvert.DeserializeObject<RetornoErroAPI>(response.Content.ReadAsStringAsync().Result);
                                                ViewData["MSG_E"] = retornoErroAPI.Detail;
                                            }
                                            else
                                                ViewData["MSG_E"] = "Erro ao alterar usuário. Tente novamente mais tarde!";
                                        }
                                    }
                                }
                                else if (response.StatusCode == HttpStatusCode.NotFound)
                                    ViewData["MSG_E"] = "Usuário não encontrado.";
                                else
                                    ViewData["MSG_E"] = "Erro ao enviar e-mail para recuperação da senha. Tente novamente mais tarde!";
                            }
                        }
                    }
                }
                else
                    ViewData["MSG_E"] = "Necessário ter um e-mail válido para recuperar a senha.";
            }
            catch
            {
                ViewData["MSG_E"] = "Erro ao enviar e-mail para recuperação da senha. Tente novamente mais tarde!";
            }

            return View();
        }
    }
}
