namespace ToDoApi.Services
{
    public interface INotificationService
    {
        Task NotifyOverdueTasksAsync();
    }

}
