using System.ComponentModel.DataAnnotations;

namespace heystock.models;

public class Produto
{
    [Key]
    public int id { get; set; }
    public int distribuidorId { get; set; }
    public int stockId { get; set; }
    public int marcaId { get; set; }
    public string validade { get; set; }
    public string categoria { get; set; }
    public float preco { get; set; }
    public Distribuidor distribuidor { get; set; }
    public Stock stock { get; set; }
    public Marca marca { get; set; }



}