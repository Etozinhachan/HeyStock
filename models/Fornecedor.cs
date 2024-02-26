using System.ComponentModel.DataAnnotations;

namespace heystock.models;

public class Fornecedor
{
    [Key]
    public int id { get; set; }
    public int marcaId { get; set; }
    public string nome { get; set; }
    public Marca marca { get; set; }
    public ICollection<Stock> stocks { get; set; }
}