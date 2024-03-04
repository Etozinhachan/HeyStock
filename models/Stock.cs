using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class Stock
{
    [Key]
    public int id { get; set; }
    public int vendaId { get; set; }
    public int fornecedorId { get; set; }
    public float precoDeVenda { get; set; }
    public float precoDeCompra { get; set; }
    public float IVA { get; set; }
    public ICollection<Produto> produtos { get; set; } 
    [JsonIgnore]
    public Fornecedor fornecedor { get; set; }
}