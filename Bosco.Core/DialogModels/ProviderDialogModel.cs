using Bosco.Core.Data.Interface;
using Bosco.Core.Models;
using Bosco.Core.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bosco.Core.DialogModels;

public class ProviderDialogModel : INotify
{
    private readonly IDialog dialog;
    private readonly IProvidersDb dbContext;
    private CancellationToken token;
    private string? title;
    private const string dialogId = "Provider_Dialog";
    private bool closedFlag = false;
    private bool wasDeleted;
    public string? Title
    {
        get { return title; }
        set { SetProperty(ref title, value); }
    }
    private string? content;
    public string? Content
    {
        get { return content; }
        set { SetProperty(ref content, value); }
    }
    public bool IsEnable { get; set; }
    public ICommand AddEmail_Command => new RelayCommand(_ => AddEmail_Execute());
    public ICommand DeleteEmail_Command => new RelayCommand(DeleteEmail_Execute);
    public ICommand AddPhone_Command => new RelayCommand(_ => AddPhone_Execute());
    public ICommand DeletePhone_Command => new RelayCommand(DeletePhone_Execute);
    public ProviderModel Provider { get; set; }
    public ProviderDialogModel(IDialog dialog, IProvidersDb dbContext)
    {
        Provider = new ProviderModel();
        this.dbContext = dbContext;
        this.dialog = dialog;
        IsEnable = true;
        dialog.SetDataContext(this);
    }

    public async Task<ProviderModel> NewProvider(CancellationToken token)
    {
        this.token = token;

        Title = "Nuevo Proveedor";
        Content = "Crear";

        Provider.AddPhone(string.Empty);
        Provider.AddEmail(string.Empty);

        await DialogHost.Show(dialog, dialogId,ClosingEventHandler_New);

        return Provider;
    }
    public async Task<ProviderModel> EditProvider(CancellationToken token, ProviderModel provider)
    {
        this.token = token;

        Provider = provider.DeepCopy();

        OnPropertyChanged(nameof(Provider));

        Title = "Editar Proveedor.";
        Content = "Editar";

        await DialogHost.Show(dialog,dialogId,closingEventHandler: ClosingEventHandler_Edit);

        return Provider;
    }
    public async Task<bool> DeleteProvider(CancellationToken token, ProviderModel provider)
    {
        this.token = token;
        Provider = provider.DeepCopy();
        IsEnable = false;
        
        OnPropertyChanged(nameof(Provider));
        OnPropertyChanged(nameof(IsEnable));

        Title = "Eliminar Proveedor";
        Content = "Eliminar";

        await DialogHost.Show(dialog, dialogId, closingEventHandler: ClosingEventHandler_Delete);

        return wasDeleted;
    }
    private void AddEmail_Execute()
    {
        Provider.AddEmail(string.Empty);
    }
    private void DeleteEmail_Execute(object? o)
    {
        if(o is EmailModel email)
        {
            Provider.RemoveEmail(email);
        }
    }
    private void AddPhone_Execute()
    {
        Provider.AddPhone(string.Empty);
    }
    private void DeletePhone_Execute(object? o)
    {
        if(o is PhoneModel phone)
        {
            Provider.RemovePhone(phone);
        }
    }
    private async void ClosingEventHandler_New(object sender, DialogClosingEventArgs eventArgs)
    {
        if (eventArgs.Parameter is bool parameter &&
               parameter == false || closedFlag || token.IsCancellationRequested) return;

        eventArgs.Cancel();


        /* || ERRORS */
        if (!string.IsNullOrEmpty(Provider["Name"]) || !string.IsNullOrEmpty(Provider["CUIT"]))
        {
            return;
        }

        PhonesCorrector();
        EmailsCorrector();

        /* || DB CALL */
        try
        {
            List<int> Ids = new(await dbContext.Insert(Provider));

            int i = 0;
            
            Provider.Id = Ids[i++];
            Provider.Address.Id = Ids[i++];
            foreach(EmailModel email in Provider.Emails)
                email.Id = Ids[i++];
            foreach (PhoneModel phone in Provider.Phones)
                phone.Id = Ids[i++];

        }
        catch (Exception ex)
        {
            Provider.Error = ex.GetBaseException().Message;
            return;
        }

        closedFlag = true;
        eventArgs.Session.Close();
    }
    private async void ClosingEventHandler_Edit(object sender, DialogClosingEventArgs eventArgs)
    {
        if (closedFlag) return;
        if (eventArgs.Parameter is bool parameter &&
               parameter == false || token.IsCancellationRequested)
        {
            Provider.Id = -1;
            return;
        }

        eventArgs.Cancel();


        /* || ERRORS */
        if (!string.IsNullOrEmpty(Provider["Name"]) || !string.IsNullOrEmpty(Provider["CUIT"]))
        {
            return;
        }

        PhonesCorrector();
        EmailsCorrector();

        /* || DB CALL */
        try
        {
            List<int> Ids = new(await dbContext.Update(Provider));

            int i = 0;

            foreach (EmailModel email in Provider.Emails)
                if(email.Id == 0) email.Id = Ids[i++];
            foreach (PhoneModel phone in Provider.Phones)
                if(phone.Id == 0) phone.Id = Ids[i++];

        }
        catch (Exception ex)
        {
            Provider.Error = ex.GetBaseException().Message;
            return;
        }

        closedFlag = true;
        eventArgs.Session.Close();
    }
    private async void ClosingEventHandler_Delete(object sender, DialogClosingEventArgs eventArgs)
    {
        if (closedFlag) return;
        if (eventArgs.Parameter is bool parameter &&
               parameter == false || token.IsCancellationRequested)
        {
            wasDeleted = false;
            return;
        }

        eventArgs.Cancel();


        /* || ERRORS */
        if (!string.IsNullOrEmpty(Provider["Name"]) || !string.IsNullOrEmpty(Provider["CUIT"]))
        {
            return;
        }

        PhonesCorrector();
        EmailsCorrector();

        /* || DB CALL */
        try
        {
            await dbContext.Delete(Provider);
            wasDeleted = true;
        }
        catch (Exception ex)
        {
            Provider.Error = ex.GetBaseException().Message;
            return;
        }

        closedFlag = true;
        eventArgs.Session.Close();
    }

    private void PhonesCorrector()
    {
        List<PhoneModel> values= new();
        foreach (PhoneModel phone in Provider.Phones)
        {
            if(string.IsNullOrEmpty(phone.PhoneNumber) && phone.Id == 0) values.Add(phone);
        }
        foreach (PhoneModel phone in values)
        {
            Provider.RemovePhone(phone);
        }
    }
    private void EmailsCorrector()
    {
        List<EmailModel> values = new();
        foreach (EmailModel email in Provider.Emails)
        {
            if (string.IsNullOrEmpty(email.Email) && email.Id == 0) values.Add(email);
        }
        foreach (EmailModel email in values)
        {
            Provider.RemoveEmail(email);
        }
    }
}
