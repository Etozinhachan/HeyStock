using System.ComponentModel.DataAnnotations;

namespace heystock.models;

public class Venda
{
    [Key]
    public int id { get; set; }
    public int userId { get; set; }
    public float custo { get; set; }
    public ICollection<Produto> produtos { get; set; }    
    public User user { get; set; }
}