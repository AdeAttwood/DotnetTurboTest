using DotnetTurbo.Web.Turbo;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotnetTurbo.Web.Hubs;

public interface ITurboHub
{
    Task TurboStream(string content);
}

public class TurboHub : Hub<ITurboHub>
{
    public async Task Send(string content)
    {
        await Clients.All.TurboStream(content);
    }
}