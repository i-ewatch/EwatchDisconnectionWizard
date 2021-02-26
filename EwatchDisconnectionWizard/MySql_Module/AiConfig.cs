using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.MySql_Module
{
    public class AiConfig
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
        /// AI點為欄
        /// </summary>
        public string Ai { get; set; }
        /// <summary>
        /// AI名稱
        /// </summary>
        public string AiName { get; set; }
        /// <summary>
        /// 計算旗標
        /// </summary>
        public bool AiCalculateFlag { get; set; }
        /// <summary>
        /// 單位
        /// </summary>
        public string Aiunit { get; set; }
        /// <summary>
        /// 最大最小值比較
        /// </summary>
        public bool CompareFlag { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public decimal AiMax { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public decimal AiMin { get; set; }
        /// <summary>
        /// 比較類型(超過上限，正常，低於下限)
        /// </summary>
        public decimal CompareType { get; set; }
        /// <summary>
        /// 群組編號
        /// </summary>
        public int? GroupNo { get; set; }
    }
}
