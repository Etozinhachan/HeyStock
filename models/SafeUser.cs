using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class SafeUser{

    public required int id { get; set; }
    public required string UserName { get; set; }
    public required bool isAdmin { get; set; } = false;
    


}