using System.Threading.Tasks;
using NotificationService.DomainEvents;
using NotificationService.Models.Enums;

namespace NotificationService.Abstractions
{
    public interface INotifier
    {
        NotifierType Type { get; }
        
        Task NewMessage(NewMessage @event);
    }
}