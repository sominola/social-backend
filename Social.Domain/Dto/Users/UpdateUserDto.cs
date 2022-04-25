namespace Social.Domain.Dto.Users;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } 
}