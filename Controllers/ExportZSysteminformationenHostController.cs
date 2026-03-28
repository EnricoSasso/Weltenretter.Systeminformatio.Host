using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Weltenretter.Systeminformationen.Host.Data;

namespace Weltenretter.Systeminformationen.Host.Controllers
{
    public partial class Exportz_Systeminformationen_HostController : ExportController
    {
        private readonly z_Systeminformationen_HostContext context;
        private readonly z_Systeminformationen_HostService service;

        public Exportz_Systeminformationen_HostController(z_Systeminformationen_HostContext context, z_Systeminformationen_HostService service)
        {
            this.service = service;
            this.context = context;
        }
    }
}
