using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class JobsController : Controller
{

    [HttpPost("fireandforget")]
    public IActionResult fireAndForget(string mail)
    {
        // Stores the job id into a variable and adding a background job for SendMail method.
        var fireAndForgetJob = BackgroundJob.Enqueue(() => SendMail(mail));
        // Return OK (Status 200) with a message that includes the job id from the scheduled job
        return Ok($"Great! The job {fireAndForgetJob} has been completed. The mail has been sent to the user.");
    }

    [HttpPost("delayed")]
    public IActionResult Delayed(string mail)
    {
        var delayed = BackgroundJob.Schedule(() => SendMail(mail), TimeSpan.FromMinutes(1));
        return Ok($"Great! The Delayed job with id: {delayed} has been added. The delayed mail has been scheduled to the user and will be sent within 1 minute.");
    }

    [HttpPost("recurring")]
    public IActionResult Recurring(string mail)
    {
        RecurringJob.AddOrUpdate(() => SendMail(mail), Cron.Weekly);
        return Ok($"The recurring job has been scheduled for user with mail: {mail}.");
    }

    [HttpPost("continuation")]
    public IActionResult Continuation(string username, string mail)
    {
        var jobId = BackgroundJob.Enqueue(() => DeleteUserData(username));
        BackgroundJob.ContinueJobWith(jobId, () => SendConfirmationMailUponDataDeletion(mail));
        return Ok($"OK - Data for user with username: {username} has been deleted and a confirmation has been sent to: {mail}");
    }

    public void DeleteUserData(string username)
    {
        // Implement logic to delete data here for a specific user
        Console.WriteLine($"Deleted data for user {username}");
    }

    public void SendConfirmationMailUponDataDeletion(string mail)
    {
        Console.WriteLine($"Successfully sent deletion confirmation to mail: {mail}");
    }

    public void SendMail(string mail)
    {
        // Implement any logic you want - not in the controller but in some repository.
        Console.WriteLine($"This is a test - Hello {mail}");
    }
}