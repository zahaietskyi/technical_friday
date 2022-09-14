using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;

namespace App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IConnectivity _connectivity;
    public MainViewModel(IConnectivity connectivity)
    {
        _connectivity = connectivity;
    }
    private string userName;
    public string UserName
    {
        get => userName;
        set
        {
            SetProperty(ref userName, value);
            IsError = false;
        }
    }
    [ObservableProperty] bool isError;


    [RelayCommand]
    async Task EnterToHub()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            IsError = true;
            return;
        }
        

        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
            return;
        }
        await Shell.Current.GoToAsync("///Hub");
    }
}