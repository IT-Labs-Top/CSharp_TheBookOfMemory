using CommunityToolkit.Mvvm.Messaging.Messages;
using TheBookOfMemory.Models.Entities;

namespace TheBookOfMemory.Models.Messages;

public class FilterMessage : ValueChangedMessage<Filter>
{
    public FilterMessage(Filter value) : base(value) { }
}