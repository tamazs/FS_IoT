using DataAccess;

namespace Api.DTOs;

public class TurbineActionDto
{
    public string Id { get; set; }
    public string TurbineId { get; set; }
    public string UserName { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActionType { get; set; }
    
    public int? IntervalValue { get; set; }
    
    public string? StopReason { get; set; }
    
    public double? PitchAngle { get; set; }
}