using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace heystock.models;

public class SafeUserAdmin{

    public required int id { get; set; }
    public required string email { get; set; }
    public required string UserName { get; set; }
    public required bool isAdmin { get; set; } = false;
    


}