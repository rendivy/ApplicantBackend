using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.Mvc;

namespace UserService;

[ApiController]
[Route("user")]
public class UserController(IBus bus): Controller
{
    [HttpPost("send_test")]
    public async Task<IActionResult> SendTest()
    {
        await bus.PubSub.PublishAsync(new EmailNotification
        {
            From = "admission@smtp.com",
            To = "applicant@gmail.com",
            Subject = "Ты не поступил",
            Message = "Увы"
        }).ConfigureAwait(false);
        return Ok();
    }
}