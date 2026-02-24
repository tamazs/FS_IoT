using System.Text.Json;
using DataAccess;
using Mqtt.Controllers;

namespace Api.Controllers;

public class IotController(ILogger<IotController> logger) : MqttController
{
    [MqttRoute("farm/TM_FS_IoT/windmill/+/telemetry")]
    public async Task ListenForMeasurements(Measurement m)
    {
        logger.LogInformation(JsonSerializer.Serialize(m));
    }
    
    
}