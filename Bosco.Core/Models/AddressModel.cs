namespace Bosco.Core.Models;

public class AddressModel
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }

    public AddressModel()
    {
        Country = string.Empty;
        State = string.Empty;
        City = string.Empty;
        PostalCode = string.Empty;
        Street = string.Empty;
        StreetNumber = string.Empty;
    }

    public AddressModel ShallowCopy()
    {
        return (AddressModel)MemberwiseClone();
    }
}
