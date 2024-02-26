using System.ComponentModel.DataAnnotations;

namespace heystock.models;

public class Distribuidor
{
    [Key]
    public int id { get; set; }
    public int MarcaId { get; set; }
    public string nome { get; set; }
    public string numero_telefone { get; set; }
    public Marca marca { get; set; }
    public ICollection<Produto> produtos { get; set; }

}