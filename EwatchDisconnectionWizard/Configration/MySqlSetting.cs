using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.Configration
{
    public class MySqlSetting
    {
        /// <summary>
        /// 資料庫IP
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 資料庫名稱
        /// </summary>
        public string InitialCatalog { get; set; }
        /// <summary>
        /// 資料庫帳號
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 資料庫密碼
        /// </summary>
        public string Password { get; set; }
    }
}
