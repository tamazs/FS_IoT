using DataAccess;
using Microsoft.AspNetCore.Mvc;
using StateleSSE.AspNetCore;
using StateleSSE.AspNetCore.EfRealtime;
using StateleSSE.AspNetCore.GroupRealtime;

namespace Api.Controllers;

public class WebClientController(ISseBackplane backplane,
    IRealtimeManager realtimeManager,
    AppDbContext db,
    IGroupRealtimeManager groupRealtimeManager
) : RealtimeControllerBase(backplane)
{
    
    [HttpGet(nameof(GetMeasurements))]
    public async Task<RealtimeListenResponse<List<Measurement>>> GetMeasurements(string connectionId)
    {
        var group = "measurements";
        await backplane.Groups.AddToGroupAsync(connectionId, group);
        realtimeManager.Subscribe<AppDbContext>(connectionId, group, 
            criteria: snapshot =>
            {
                return snapshot.HasChanges<Measurement>();
            },
            query: async context =>
            {
                return context.Measurements.ToList();
            }
        );
        return new RealtimeListenResponse<List<Measurement>>(group, db.Measurements.ToList());
    }
    
    [HttpGet(nameof(GetAlerts))]
    public async Task<RealtimeListenResponse<List<Alert>>> GetAlerts(string connectionId)
    {
        var group = "alerts";
        await backplane.Groups.AddToGroupAsync(connectionId, group);
        realtimeManager.Subscribe<AppDbContext>(connectionId, group, 
            criteria: snapshot =>
            {
                return snapshot.HasChanges<Alert>();
            },
            query: async context =>
            {
                return context.Alerts.ToList();
            }
        );
        return new RealtimeListenResponse<List<Alert>>(group, db.Alerts.ToList());
    }
}