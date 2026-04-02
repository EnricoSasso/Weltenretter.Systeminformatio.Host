using Microsoft.Extensions.Options;
using Weltenretter.Systeminformationen.Models;
using Weltenretter.Systeminformationen.Services;

namespace Weltenretter.Systeminformationen.Host.Services.Systeminformationen
{
    /// <summary>
    /// Liefert den Projektschlüssel aus der Host-Konfiguration.
    /// </summary>
    public class ProjektSchluesselProvider : IProjektSchluesselProvider
    {
        private readonly IOptions<SysteminformationenOptionen> options;

        /// <summary>
        /// Erstellt den Provider mit Options-Binding.
        /// </summary>
        /// <param name="options">Konfigurationsoptionen des Moduls.</param>
        public ProjektSchluesselProvider(IOptions<SysteminformationenOptionen> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Projektschlüssel des Hosts.
        /// </summary>
        public string ProjektSchluessel => options.Value.ProjektSchluessel;
    }
}
