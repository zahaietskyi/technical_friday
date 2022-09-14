using Common.Models;

namespace App.Views;

public class MessageViewSelector : DataTemplateSelector
{
    public DataTemplate SenderMessageView { get; set; }
    public DataTemplate ReceiverMessageView { get; set; }
    public string UserName { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var message = (MessageDto)item;

        return message.UserName == UserName ? SenderMessageView : ReceiverMessageView;
    }
}