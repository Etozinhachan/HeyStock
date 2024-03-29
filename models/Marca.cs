using System.ComponentModel.DataAnnotations;

namespace heystock.models;

public class Marca
{
    
    [Key]
    public int id { get; set; }
    public int fornecedorId { get; set; }
    public int distribuidorId { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string numero_telefone { get; set; }
    public ICollection<Fornecedor> fornecedores { get; set; }
    public ICollection<Distribuidor> distribuidores { get; set; }
    public ICollection<Produto> produtos { get; set; }

}