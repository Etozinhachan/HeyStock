using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class Produto
{
    [Key]
    public int id { get; set; }
    public string nome { get; set; }
    public int distribuidorId { get; set; }
    public int stockId { get; set; }
    public int marcaId { get; set; }
    public int vendaId { get; set; }
    public string validade { get; set; }
    public string categoria { get; set; }
    public float preco { get; set; }
    [JsonIgnore]
    public Distribuidor distribuidor { get; set; }
    [JsonIgnore]
    public Venda venda { get; set; }
    [JsonIgnore]
    public Stock stock { get; set; }
    [JsonIgnore]
    public Marca marca { get; set; }



}