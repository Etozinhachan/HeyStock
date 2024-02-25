using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class User{

    [Key]
    public int id { get; set; }
    public required string UserName { get; set; }
    public required string passHash { get; set; }
    public string? salt { get; set; }
    public bool isAdmin { get; set; } = false;

}