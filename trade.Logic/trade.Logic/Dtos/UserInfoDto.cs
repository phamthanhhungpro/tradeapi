namespace trade.Logic.Dtos;
public class UserInfo
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public string TokenString { get; set; }
    public bool IsValid { get; set; }
}