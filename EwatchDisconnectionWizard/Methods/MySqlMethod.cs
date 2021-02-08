﻿using Dapper;
using EwatchDisconnectionWizard.Configration;
using EwatchDisconnectionWizard.Enums;
using EwatchDisconnectionWizard.MySql_Module;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.Methods
{
    public class MySqlMethod
    {
        /// <summary>
        /// Web資料庫連接字串
        /// </summary>
        public MySqlConnectionStringBuilder Webscsb { get; set; } = null;
        /// <summary>
        /// LOG資料庫連接字串
        /// </summary>
        public MySqlConnectionStringBuilder scsb { get; set; } = null;
        /// <summary>
        /// Mysql初始化
        /// </summary>
        /// <param name="mySqlSetting"></param>
        public MySqlMethod(MySqlSetting mySqlSetting)
        {
            if (mySqlSetting != null)
            {
                scsb = new MySqlConnectionStringBuilder()
                {
                    Database = mySqlSetting.InitialCatalog + "db",
                    Server = mySqlSetting.DataSource,
                    UserID = mySqlSetting.UserID,
                    Password = mySqlSetting.Password,
                    CharacterSet = "utf8"
                };
                Webscsb = new MySqlConnectionStringBuilder()
                {
                    Database = mySqlSetting.InitialCatalog + "Web",
                    Server = mySqlSetting.DataSource,
                    UserID = mySqlSetting.UserID,
                    Password = mySqlSetting.Password,
                    CharacterSet = "utf8"
                };
            }
        }
        /// <summary>
        /// 案場資訊
        /// </summary>
        /// <returns></returns>
        public List<CaseSetting> CaseLoad()
        {
            List<CaseSetting> setting = null;
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT * FROM CaseSetting";
                setting = Conn.Query<CaseSetting>(sql).ToList();
            }
            return setting;
        }

        /// <summary>
        /// AI資訊
        /// </summary>
        /// <returns></returns>
        public List<AiSetting> AiLoad(CaseSetting casesetting)
        {
            List<AiSetting> setting = null;
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT * FROM AiSetting Where CaseNo = @CaseNo";
                setting = Conn.Query<AiSetting>(sql, new { CaseNo = casesetting.CaseNo }).ToList();
            }
            return setting;
        }
        /// <summary>
        /// AI即時資訊比較
        /// </summary>
        /// <param name="setting"> AI設定資訊</param>
        /// <returns></returns>
        public bool AI64Load(AiSetting setting)
        {
            bool Flag = false;
            using (var Conn = new MySqlConnection(Webscsb.ConnectionString))
            {
                string sql = "SELECT TIMESTAMPDIFF(MINUTE,(SELECT ttimen FROM Ai64 Where CaseNo = @CaseNo AND AiNo = @AiNo),@Datetime)";
                var value = Conn.QuerySingle<int?>(sql, new { CaseNo = setting.CaseNo, AiNo = setting.AiNo, Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                if (value != null)
                {
                    if (value >= setting.MTimeoutSpan)
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                }
            }
            return Flag;
        }
        /// <summary>
        /// 更新AI點位最後上傳時間
        /// </summary>
        /// <param name="setting"></param>
        public void UpdataAi_Time(AiSetting setting, bool TimeFlag)
        {
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "UPDATE AiSetting SET SendTime = @SendTime WHERE CaseNo = @CaseNo AND AiNo = @AiNo";
                if (TimeFlag)
                {
                    var value = Conn.Execute(sql, new { SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CaseNo = setting.CaseNo, AiNo = setting.AiNo });
                }
                else
                {
                    var value = Conn.Execute(sql, new { SendTime = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss"), CaseNo = setting.CaseNo, AiNo = setting.AiNo });
                }
            }
        }
        /// <summary>
        /// 查詢AI點位上傳時間
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public DateTime AI_LastTime(AiSetting setting)
        {
            using (var Conn = new MySqlConnection(Webscsb.ConnectionString))
            {
                string sql = "SELECT ttimen FROM Ai64 Where CaseNo = @CaseNo AND AiNo = @AiNo";
                var value = Conn.QuerySingle<DateTime>(sql, new { CaseNo = setting.CaseNo, AiNo = setting.AiNo });
                return value;
            }
        }
        /// <summary>
        /// AI上傳間隔時間
        /// </summary>
        /// <returns></returns>
        public int? Ai_Time(AiSetting setting)
        {
            int? value = null;
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT TIMESTAMPDIFF(HOUR,(SELECT SendTime FROM AiSetting Where CaseNo = @CaseNo AND AiNo = @AiNo),@Datetime)";
                value = Conn.QuerySingle<int?>(sql, new { CaseNo = setting.CaseNo, AiNo = setting.AiNo, Datetime = DateTime.Now });
            }
            return value;
        }

        /// <summary>
        /// 電表資訊
        /// </summary>
        /// <returns></returns>
        public List<ElectricSetting> ElectricLoad(CaseSetting casesetting)
        {
            List<ElectricSetting> setting = null;
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT * FROM ElectricSetting Where CaseNo = @CaseNo";
                setting = Conn.Query<ElectricSetting>(sql, new { CaseNo = casesetting.CaseNo }).ToList();
            }
            return setting;
        }
        /// <summary>
        /// 電表即時資訊比較
        /// </summary>
        /// <param name="setting">電表設定資訊</param>
        /// <returns></returns>
        public bool ElectricMeterLoad(ElectricSetting setting)
        {
            bool Flag = false;
            using (var Conn = new MySqlConnection(Webscsb.ConnectionString))
            {
                string sql = string.Empty;
                ElectricPhaseTypeEnum phaseTypeEnum = (ElectricPhaseTypeEnum)setting.PhaseTypeEnum;
                switch (phaseTypeEnum)
                {
                    case ElectricPhaseTypeEnum.Three:
                        {
                            sql = "SELECT TIMESTAMPDIFF(MINUTE,(SELECT ttimen FROM ThreePhaseElectricMeter Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo),@Datetime)";
                        }
                        break;
                    case ElectricPhaseTypeEnum.Single:
                        {
                            sql = "SELECT TIMESTAMPDIFF(MINUTE,(SELECT ttimen FROM SinglePhaseElectricMeter Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo),@Datetime)";
                        }
                        break;
                }
                var value = Conn.QuerySingle<int?>(sql, new { CaseNo = setting.CaseNo, ElectricNo = setting.ElectricNo, Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                if (value != null)
                {
                    if (value >= setting.MTimeoutSpan)
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                }
            }
            return Flag;
        }
        /// <summary>
        /// 更新電表點位最後上傳時間
        /// </summary>
        /// <param name="setting"></param>
        public void UpdataElectricMeter_Time(ElectricSetting setting, bool TimeFlag)
        {
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "UPDATE ElectricSetting SET SendTime = @SendTime Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo";
                if (TimeFlag)
                {
                    var value = Conn.Execute(sql, new { SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CaseNo = setting.CaseNo, ElectricNo = setting.ElectricNo });
                }
                else
                {
                    var value = Conn.Execute(sql, new { SendTime = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss"), CaseNo = setting.CaseNo, ElectricNo = setting.ElectricNo });
                }
            }
        }
        /// <summary>
        /// 查詢電表點位上傳時間
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public DateTime ElectricMeter_LastTime(ElectricSetting setting)
        {
            using (var Conn = new MySqlConnection(Webscsb.ConnectionString))
            {

                string sql = string.Empty;
                ElectricPhaseTypeEnum phaseTypeEnum = (ElectricPhaseTypeEnum)setting.PhaseTypeEnum;
                switch (phaseTypeEnum)
                {
                    case ElectricPhaseTypeEnum.Three:
                        {
                            sql = "SELECT ttimen FROM ThreePhaseElectricMeter Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo";
                        }
                        break;
                    case ElectricPhaseTypeEnum.Single:
                        {
                            sql = "SELECT ttimen FROM SinglePhaseElectricMeter Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo";
                        }
                        break;
                }
                var value = Conn.QuerySingle<DateTime>(sql, new { CaseNo = setting.CaseNo, ElectricNo = setting.ElectricNo });
                return value;
            }
        }
        /// <summary>
        /// 電表上傳間隔時間
        /// </summary>
        /// <returns></returns>
        public int? ElectricMeter_Time(ElectricSetting setting)
        {
            int? value = null;
            using (var Conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT TIMESTAMPDIFF(HOUR,(SELECT SendTime FROM ElectricSetting Where CaseNo = @CaseNo AND ElectricNo = @ElectricNo),@Datetime)";

                value = Conn.QuerySingle<int?>(sql, new { CaseNo = setting.CaseNo, ElectricNo = setting.ElectricNo, Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            return value;
        }
    }
}
