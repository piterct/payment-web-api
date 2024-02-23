using Payment.Business.Notifications;

namespace Payment.Business.Interfaces.Notifications
{
    public interface  INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}
