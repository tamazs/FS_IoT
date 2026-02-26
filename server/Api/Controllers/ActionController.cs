using System.Text.Json;
using Api.DTOs.Requests;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Mqtt.Controllers;

namespace Api.Controllers;

[ApiController]
public class ActionController(IMqttClientService mqtt, AppDbContext dbContext) : BaseController
{
    [HttpPost("{turbineId}/command")]
    public async Task SendCommand(string turbineId,[FromBody] TurbineCommand command)
    {
        var action = new TurbineAction
        {
            Id = Guid.NewGuid().ToString(),
            TurbineId = turbineId,
            UserId = "85aab6ec-caff-4fc9-a169-dc394eb7ddcd",
            Timestamp = DateTime.UtcNow,
            ActionType = command switch
            {
                StartCommand      => ActionType.start.ToString(),
                StopCommand       => ActionType.stop.ToString(),
                SetIntervalCommand => ActionType.setInterval.ToString(),
                SetPitchCommand   => ActionType.setPitch.ToString(),
                _                 => throw new ArgumentOutOfRangeException()
            },
            // Optional fields — only set if relevant to the command type
            IntervalValue = command is SetIntervalCommand si ? si.value : null,
            StopReason    = command is StopCommand stop ? stop.reason : null,
            PitchAngle    = command is SetPitchCommand sp ? sp.angle : null,
        };
        
        await dbContext.TurbineActions.AddAsync(action);
        await dbContext.SaveChangesAsync();
        
        var topic = $"farm/TM_FS_IoT/windmill/{turbineId}/command";
        var payload = JsonSerializer.Serialize(command);
        Console.WriteLine(payload);
        await mqtt.PublishAsync(topic, payload);
    }
}