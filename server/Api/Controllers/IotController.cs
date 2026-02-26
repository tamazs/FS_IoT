using System.Text.Json;
using DataAccess;
using Mqtt.Controllers;

namespace Api.Controllers;

public class IotController(ILogger<IotController> logger, AppDbContext dbContext) : MqttController
{
    [MqttRoute("farm/TM_FS_IoT/windmill/+/telemetry")]
    public async Task ListenForMeasurements(Measurement m)
    {
        m.Id = Guid.NewGuid().ToString();
        await dbContext.Measurements.AddAsync(m);
        await dbContext.SaveChangesAsync();
    }
    
    [MqttRoute("farm/TM_FS_IoT/windmill/+/alert")]
    public async Task ListenForAlerts(Alert a)
    {
        logger.LogInformation(JsonSerializer.Serialize(a));
        a.Id = Guid.NewGuid().ToString();
        await dbContext.Alerts.AddAsync(a);
        await dbContext.SaveChangesAsync();
    }
}