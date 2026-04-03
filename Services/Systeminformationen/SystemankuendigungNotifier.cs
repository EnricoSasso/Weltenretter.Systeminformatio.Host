using Microsoft.AspNetCore.SignalR;
using Weltenretter.Systeminformationen.Host.Hubs;
using Weltenretter.Systeminformationen.Services;

namespace Weltenretter.Systeminformationen.Host.Services.Systeminformationen
{
    /// <summary>
    /// Host-Implementierung für Live-Updates.
    /// </summary>
    public class SystemankuendigungNotifier : ISystemankuendigungNotifier
    {
        private readonly IHubContext<SystemankuendigungHub> hub;

        /// <summary>
        /// Erstellt den Notifier mit HubContext.
        /// </summary>
        /// <param name="hub">SignalR HubContext.</param>
        public SystemankuendigungNotifier(IHubContext<SystemankuendigungHub> hub)
        {
            this.hub = hub;
        }

        /// <inheritdoc />
        public Task NotifyUpdateAsync()
        {
            return hub.Clients.All.SendAsync("SystemankuendigungUpdated");
        }
    }
}
