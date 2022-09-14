using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace App.ViewModels;

public partial class HubViewModel : ObservableObject
{
    private HubConnection _hubConnection;
    private readonly MainViewModel _mainViewModel;
    public ScrollView scrollView { get; set; }
    public string UserName { get; set; }
    public HubViewModel(IConfiguration configuration, MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        UserName = _mainViewModel.UserName;
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(configuration["ServerUrl"] + configuration["ChatHubRoute"]).Build();

        _hubConnection.On<MessageDto>("Get", (message) =>
        {
            ListMessages.Add(message);
        });
        ListMessages.CollectionChanged += async (sender, e) =>
        {
            if (e.Action != NotifyCollectionChangedAction.Add || scrollView is null) return;
            await Task.Delay(500);
            await scrollView.ScrollToAsync(scrollView.Children.LastOrDefault(), ScrollToPosition.End, true);
        };
        Shell.Current.DisplayAlert("Info", "Welcome to chat everybody", "OK");
        //_hubConnection.Closed += async (error) =>
        //{
        //    await Shell.Current.DisplayAlert("Error", "Connection closed, please enter OK for reconnect", "OK");
        //    await Connect();
        //};
    }

    public async Task Connect()
    {
        try
        {
            if (_hubConnection.State != HubConnectionState.Connected)
            {
                await _hubConnection.StartAsync();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", ex.Message, "OK");
        }
    }
    public async Task Disconnect()
    {
        await _hubConnection.StopAsync();
    }

    [ObservableProperty] private ObservableCollection<MessageDto> listMessages = new();
    [ObservableProperty] private string messageText;

    [RelayCommand]
    async Task Back()
    {
        await Shell.Current.GoToAsync("///MainPage");
    }

    [RelayCommand]
    async Task SendMessage()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(MessageText))
            {
                return;
            }
            await _hubConnection.InvokeAsync("Send", new MessageDto()
            {
                MessageText = MessageText,
                UserName = _mainViewModel.UserName
            });
            MessageText = string.Empty;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Uh Oh!", ex.Message, "OK");
        }
    }
}