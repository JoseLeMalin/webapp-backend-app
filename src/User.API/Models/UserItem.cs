using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
namespace BackendAPI.User.API.Models;


public class UserItem : IValidatableObject
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PictureUrl { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        return results;
    }
}
