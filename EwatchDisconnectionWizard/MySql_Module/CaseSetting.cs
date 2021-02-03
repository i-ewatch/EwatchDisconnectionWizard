using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.MySql_Module
{
    public class CaseSetting
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
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 聯絡人
        /// </summary>
        public string Contacter { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 案場名稱
        /// </summary>
        public string TitleName { get; set; }
        /// <summary>
        /// 告警發報類型 0 = 不使用,1 = LineNotify，2 = Telegrambot
        /// </summary>
        public int NotifyTypeEnum { get; set; }
        /// <summary>
        /// 告警發報網址  LineNotify不需輸入，Telegrambot API網址
        /// </summary>
        public string NotifyApi { get; set; }
        /// <summary>
        /// 告警發報權杖 Line的Token，Telegrambot的Chat_ID
        /// </summary>
        public string NotifyToken { get; set; }
        /// <summary>
        /// 斷線發報時間(分)
        /// </summary>
        public int TimeoutSpan { get; set; }
    }
}
