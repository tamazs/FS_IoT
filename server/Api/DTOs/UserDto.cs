using DataAccess;

namespace Api.DTOs;

public class UserDto
{
    public UserDto()
    {}
    
    public UserDto(User user)
    {
        UserId =  user.UserId;
        UserName = user.Username;
        Email = user.Email;
    }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}