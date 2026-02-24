namespace DataAccess;

public class Measurement
{
    public string Id { get; set; }
    public string turbineId { get; set; }
    public string turbineName { get; set; }
    public string farmId { get; set; }
    public DateTime timestamp { get; set; }
    public double windSpeed { get; set; }
    public double windDirection { get; set; }
    public double ambientTemperature { get; set; }
    public double rotorSpeed { get; set; }
    public double powerOutput { get; set; }
    public double nacelleDirection { get; set; }
    public double bladePitch { get; set; }
    public double generatorTemp { get; set; }
    public double gearboxTemp { get; set; }
    public double vibration { get; set; }
    public string status { get; set; }
}