using Newtonsoft.Json;
using ProjetoAgenda.Libraries;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjetoAgenda.Models
{
    public class Contato
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "nome")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MandatoryField")]
        [MinLength(2, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MinLength")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "sobrenome")]
        [MaxLength(256, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Sobrenome { get; set; }

        [JsonProperty(PropertyName = "telefone")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MaxLength")]
        public string Telefone { get; set; }

        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MandatoryField")]
        [JsonPropertyName("id_usuario")]
        [JsonProperty("id_usuario")]
        public int IdUsuario { get; set; }
    }
}
