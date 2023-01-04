namespace Bosco.Core.Models;

public class PhoneModel
{
    public string PhoneNumber { get; set;}
    public int Id { get; set; }
    public PhoneModel()
    {
        PhoneNumber = string.Empty;
    }
    public PhoneModel(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}
