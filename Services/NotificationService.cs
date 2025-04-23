//using Microsoft.EntityFrameworkCore;
//using ToDoApi.Services;

//public class NotificationService : INotificationService
//{
//    private readonly TodoContext _context;
//    private readonly IEmailSender _emailSender; // Your custom email sender

//    public NotificationService(TodoContext context, IEmailSender emailSender)
//    {
//        _context = context;
//        _emailSender = emailSender;
//    }

//    public async Task NotifyOverdueTasksAsync()
//    {
//        var now = DateTime.UtcNow;
//        var overdue = await _context.TodoItems
//            .Include(t => t.User)
//            .Where(t => t.DueDate < now && !t.IsCompleted)
//            .ToListAsync();

//        foreach (var task in overdue)
//        {
//            if (task.User != null)
//            {
//                await _emailSender.SendEmailAsync(
//                    to: task.User.Username + "@example.com",
//                    subject: $"Overdue Task: {task.Title}",
//                    body: $"Hey! Your task \"{task.Title}\" was due on {task.DueDate.Value.ToShortDateString()}."
//                );
//            }
//        }
//    }
//}

