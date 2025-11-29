using Finance.API.DTOs;

namespace Finance.API.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int id, string userId);
    }
}