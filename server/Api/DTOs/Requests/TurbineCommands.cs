using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataAccess;

namespace Api.DTOs.Requests;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "action")]
[JsonDerivedType(typeof(SetIntervalCommand), "setInterval")]
[JsonDerivedType(typeof(StopCommand), "stop")]
[JsonDerivedType(typeof(StartCommand), "start")]
[JsonDerivedType(typeof(SetPitchCommand), "setPitch")]
public abstract class TurbineCommand { }

public class SetIntervalCommand : TurbineCommand
{
    [Required][Range(1, 60)]
    public int value { get; set; }
}

public class StopCommand : TurbineCommand
{
    public string? reason { get; set; }
}

public class StartCommand : TurbineCommand
{
}

public class SetPitchCommand : TurbineCommand
{
    [Required][Range(0, 30)]
    public double angle { get; set; }
}