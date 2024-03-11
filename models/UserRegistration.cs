using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace testingStuff.models;

public class UserRegistration{

    public required string UserName { get; set; }
    public required string passHash { get; set; }

}