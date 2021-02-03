using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.MySql_Module
{
    public class ThreePhaseElectric
    {
        public string ttime { get; set; }
        public DateTime ttimen { get; set; }
        public string CaseNo { get; set; }
        public int ElectricNo { get; set; }
        public decimal RV { get; set; }
        public decimal SV { get; set; }
        public decimal TV { get; set; }
        public decimal RA { get; set; }
        public decimal SA { get; set; }
        public decimal TA { get; set; }
        public decimal RSV { get; set; }
        public decimal STV { get; set; }
        public decimal TRV { get; set; }
        public decimal kW { get; set; }
        public decimal kWh { get; set; }
        public decimal kVAR { get; set; }
        public decimal kVARh { get; set; }
        public decimal PF { get; set; }
        public decimal kVA { get; set; }
        public decimal kVAh { get; set; }
        public decimal HZ { get; set; }
    }
}
