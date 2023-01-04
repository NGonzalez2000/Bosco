namespace Bosco.Core.Models;

public class EmailModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public EmailModel()
    {
        Email = string.Empty;
    }
    public EmailModel(string email)
    {
        Email = email;
    }
}
