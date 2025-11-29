using Finance.API.DTOs;
using Finance.API.Interfaces;

namespace Finance.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var notifications = await _repository.GetUserNotificationsAsync(userId);

            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                Type = n.Type,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            }).ToList();
        }

        public async Task MarkAsReadAsync(int id, string userId)
        {
            var notification = await _repository.GetNotificationByIdAsync(id);
            if (notification != null && notification.UserId == userId)
            {
                notification.IsRead = true;
                await _repository.UpdateNotificationAsync(notification);
            }
        }
    }
}