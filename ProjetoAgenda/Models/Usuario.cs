using Newtonsoft.Json;
using ProjetoAgenda.Libraries;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace ProjetoAgenda.Models
{
    public class Usuario
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MandatoryField")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MinLength")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "nome")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MandatoryField")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MinLength")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "senha")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MandatoryField")]
        [MinLength(4, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MinLength")]
        [MaxLength(40, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Senha { get; set; }

        public void EncodeSenha()
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(Senha);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            Senha = sb.ToString();
            sb.Clear();
        }
    }
}
