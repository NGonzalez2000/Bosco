using System;
using System.ComponentModel;

namespace Bosco.Core.Models;

public class CUITModel : IDataErrorInfo
{

    private string cuit;
    public string CUIT
    {
        get => cuit;
        set => cuit = value; 
    }

    public string Error => string.Empty;

    public string this[string columnName] => IsValid() ? "" : "El CUIT ingresado no es valido";

    public CUITModel()
	{
		cuit = "00000000000";
	}
    public bool IsValid()
    {
        string x_cuil = cuit[..^1];
        string valid_digit = cuit[^1..];
        x_cuil = StrReverse(x_cuil);
        int SUM_MOD11 = 0;
        for (int i = 0; i < x_cuil.Length; i++)
            SUM_MOD11 += Convert.ToInt32(x_cuil[i] - '0') * (i % 6 + 2);
        SUM_MOD11 %= 11;
        return 11 - Convert.ToInt32(valid_digit[0] - '0') == SUM_MOD11;
    }

    private static string StrReverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public CUITModel ShallowCopy()
    {
        return (CUITModel)MemberwiseClone();
    }
}
