using App.ViewModels;

namespace App.Pages;

public partial class HubPage : ContentPage
{
    private readonly HubViewModel _hubViewModel;
    public HubPage(HubViewModel vm)
    {
        _hubViewModel = vm;
        InitializeComponent();
        vm.scrollView = mainScrollView;
        messageDataTemplateSelector.UserName = _hubViewModel.UserName;
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _hubViewModel.Connect();
        inputEntry.Focus();
    }
    protected override async void OnDisappearing()
    {
        await _hubViewModel.Disconnect();
        base.OnDisappearing();
    }
}