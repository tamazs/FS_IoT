namespace DataAccess;

public class TurbineAction
{
    public string Id { get; set; }
    public string TurbineId { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActionType { get; set; }
    
    public int? IntervalValue { get; set; }
    
    public string? StopReason { get; set; }
    
    public double? PitchAngle { get; set; }
    
}