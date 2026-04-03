using Microsoft.AspNetCore.SignalR;

namespace Weltenretter.Systeminformationen.Host.Hubs
{
    /// <summary>
    /// SignalR-Hub für Live-Updates der Systemankündigung.
    /// </summary>
    public class SystemankuendigungHub : Hub
    {
        /// <summary>
        /// Broadcast an alle Clients.
        /// </summary>
        public async Task NotifyUpdate()
        {
            await Clients.All.SendAsync("SystemankuendigungUpdated");
        }
    }
}
