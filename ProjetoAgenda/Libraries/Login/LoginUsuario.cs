using Newtonsoft.Json;
using ProjetoAgenda.Models;

namespace ProjetoAgenda.Libraries.Login
{
    public class LoginUsuario
    {
        private Sessao.Sessao _sessao;
        private string Key = "Login.Usuario";

        public LoginUsuario(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Usuario usuario)
        {
            _sessao.Cadastrar(Key, JsonConvert.SerializeObject(usuario));
        }

        public Usuario GetUsuario()
        {
            string usuario = _sessao.Consultar(Key);

            if (!string.IsNullOrWhiteSpace(usuario))
                return JsonConvert.DeserializeObject<Usuario>(usuario);
            else
                return null;
        }

        public void Logout()
        {
            _sessao.RemoverTodas();
        }
    }
}
