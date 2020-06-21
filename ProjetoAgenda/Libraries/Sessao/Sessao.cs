using Microsoft.AspNetCore.Http;

namespace ProjetoAgenda.Libraries.Sessao
{
    public class Sessao
    {
        IHttpContextAccessor _context;
        public Sessao(IHttpContextAccessor context)
        {
            _context = context;
        }

        public void Cadastrar(string Key, string Valor)
        {
            _context.HttpContext.Session.SetString(Key, Valor);
        }

        public void Atualizar(string Key, string Valor)
        {
            if (Existe(Key))
                _context.HttpContext.Session.Remove(Key);

            _context.HttpContext.Session.SetString(Key, Valor);
        }

        public void Remover(string Key)
        {
            if (Existe(Key))
                _context.HttpContext.Session.Remove(Key);
        }

        public string Consultar(string Key)
        {
            if (Existe(Key))
                return _context.HttpContext.Session.GetString(Key);
            else
                return string.Empty;
        }

        public bool Existe(string Key)
        {
            return _context.HttpContext.Session.GetString(Key) != null;
        }

        public void RemoverTodas()
        {
            _context.HttpContext.Session.Clear();
        }
    }
}
