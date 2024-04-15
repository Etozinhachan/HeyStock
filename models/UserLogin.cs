using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace testingStuff.models;

public class UserLogin{

    public required string usernameOrEmail { get; set; }
    public required string passHash { get; set; }
    public required bool usingEmail { get; set; }

}