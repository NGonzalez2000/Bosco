using Bosco.Core.Collections;
using Bosco.Core.Services;
using System.ComponentModel;

namespace Bosco.Core.Models;
public class ProviderModel : INotify, IDataErrorInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Web { get; set; }
    public string Observation { get; set; }
    public CUITModel CUIT { get; set; }
    public AddressModel Address { get; set; }
    public ListViewCollection<EmailModel> Emails { get; set; }
    public ListViewCollection<PhoneModel> Phones { get; set; }

    private string error;
    public string Error
    {
        get
        {
            return error;
        }
        set => SetProperty(ref error, value);
    }

    public string this[string columnName]
    {
        get
        {
            if(columnName == nameof(Name))
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    Error = "Nombre es obligatorio";
                    return "Campo obligatorio";
                }
            }
            if(columnName == nameof(CUIT))
            {
                return CUIT[columnName];
            }
            return string.Empty;
        }
    }

    public ProviderModel()
    {
        Name = string.Empty;
        Web = string.Empty;
        Observation = string.Empty;
        CUIT = new();
        Address = new();
        Emails = new();
        Phones = new();
        error = string.Empty;
    }
    public ProviderModel DeepCopy()
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Web = Web,
            Observation = Observation,
            CUIT = CUIT.ShallowCopy(),
            Address = Address.ShallowCopy(),
            Emails = new(Emails),
            Phones = new(Phones)
        };
    }
    public void AddEmail(string email)
    {
        Emails.Add(new(email));
    }
    public void AddEmail(EmailModel email)
    {
        Emails.Add(email);
    }
    public void RemoveEmail(EmailModel email)
    {
        Emails.Remove(email);
        if (Emails.Count == 0) Emails.Add(new());
    }
    public void AddPhone(string phone)
    {
        Phones.Add(new(phone));
    }
    public void AddPhone(PhoneModel phone)
    {
        Phones.Add(phone);
    }
    public void RemovePhone(PhoneModel phone)
    {
        Phones.Remove(phone);
        if(Phones.Count == 0) Phones.Add(new());
    }
}
