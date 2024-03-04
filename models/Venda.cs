using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class Venda
{
    [Key]
    public int id { get; set; }
    public int userId { get; set; }
    public float custo { get; set; }
    public ICollection<Produto> produtos { get; set; }
    [JsonIgnore]    
    public User user { get; set; }
}