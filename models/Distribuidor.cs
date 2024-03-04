using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class Distribuidor
{
    [Key]
    public int id { get; set; }
    public int MarcaId { get; set; }
    public string nome { get; set; }
    public string numero_telefone { get; set; }
    [JsonIgnore]
    public Marca marca { get; set; }
    public ICollection<Produto> produtos { get; set; }

}