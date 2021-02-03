using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.MySql_Module
{
    public class AiSetting
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 案場編號
        /// </summary>
        public string CaseNo { get; set; }
        /// <summary>
        /// AI編號
        /// </summary>
        public int AiNo { get; set; }
        /// <summary>
        /// AI名稱
        /// </summary>
        public string AiName { get; set; }
        /// <summary>
        /// 0 = 不發報 ， 1 =發報
        /// </summary>
        public bool NotifyFlag { get; set; }
        /// <summary>
        /// 斷線發報時間(分)
        /// </summary>
        public int TimeoutSpan { get; set; }
        /// <summary>
        /// 最後發報時間
        /// </summary>
        public DateTime? SendTime { get; set; }
    }
}
