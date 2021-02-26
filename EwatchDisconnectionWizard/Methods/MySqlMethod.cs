using Dapper;
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
        /// Log資料庫連接字串
        /// </summary>
        public MySqlConnectionStringBuilder Logscsb { get; set; } = null;
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
                Logscsb = new MySqlConnectionStringBuilder()
                {
                    Database = mySqlSetting.InitialCatalog + "Log",
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

        #region AI
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
        #endregion

        #region 電表
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
        #endregion

        /// <summary>
        /// AI點為設定查詢是否需要比較
        /// </summary>
        /// <returns></returns>
        public List<AiConfig> AiConfig_Compare_Load()
        {
            List<AiConfig> aiConfigs = null;
            using (var conn = new MySqlConnection(scsb.ConnectionString))
            {
                string sql = "SELECT * FROM AiConfig WHERE CompareFlag = true";
                aiConfigs = conn.Query<AiConfig>(sql).ToList();
            }
            return aiConfigs;
        }
        public AI64 Ai64web(AiConfig config)
        {
            using (var conn = new MySqlConnection(Webscsb.ConnectionString))
            {
                string sql = "SELECT * FROM Ai64 WHERE CaseNo = @CaseNo AND AiNo = @AiNo";
                var data = conn.QuerySingle<AI64>(sql, new { config.CaseNo, config.AiNo });
                return data;
            }
        }
        /// <summary>
        /// 上下限比較紀錄
        /// </summary>
        /// <param name="config">AI點為設定資訊</param>
        public void Alarm_Procedure(AiConfig config, AI64 aI64)
        {
            using (var conn = new MySqlConnection(Logscsb.ConnectionString))
            {
                string Procedure = "AiAlarmProcedure";
                decimal[] value = new decimal[64];
                int i = 0;
                value[i] = aI64.Ai1; i++;
                value[i] = aI64.Ai2; i++;
                value[i] = aI64.Ai3; i++;
                value[i] = aI64.Ai4; i++;
                value[i] = aI64.Ai5; i++;
                value[i] = aI64.Ai6; i++;
                value[i] = aI64.Ai7; i++;
                value[i] = aI64.Ai8; i++;
                value[i] = aI64.Ai9; i++;
                value[i] = aI64.Ai10; i++;
                value[i] = aI64.Ai11; i++;
                value[i] = aI64.Ai12; i++;
                value[i] = aI64.Ai13; i++;
                value[i] = aI64.Ai14; i++;
                value[i] = aI64.Ai15; i++;
                value[i] = aI64.Ai16; i++;
                value[i] = aI64.Ai17; i++;
                value[i] = aI64.Ai18; i++;
                value[i] = aI64.Ai19; i++;
                value[i] = aI64.Ai20; i++;
                value[i] = aI64.Ai21; i++;
                value[i] = aI64.Ai22; i++;
                value[i] = aI64.Ai23; i++;
                value[i] = aI64.Ai24; i++;
                value[i] = aI64.Ai25; i++;
                value[i] = aI64.Ai26; i++;
                value[i] = aI64.Ai27; i++;
                value[i] = aI64.Ai28; i++;
                value[i] = aI64.Ai29; i++;
                value[i] = aI64.Ai30; i++;
                value[i] = aI64.Ai31; i++;
                value[i] = aI64.Ai32; i++;
                value[i] = aI64.Ai33; i++;
                value[i] = aI64.Ai34; i++;
                value[i] = aI64.Ai35; i++;
                value[i] = aI64.Ai36; i++;
                value[i] = aI64.Ai37; i++;
                value[i] = aI64.Ai38; i++;
                value[i] = aI64.Ai39; i++;
                value[i] = aI64.Ai40; i++;
                value[i] = aI64.Ai41; i++;
                value[i] = aI64.Ai42; i++;
                value[i] = aI64.Ai43; i++;
                value[i] = aI64.Ai44; i++;
                value[i] = aI64.Ai45; i++;
                value[i] = aI64.Ai46; i++;
                value[i] = aI64.Ai47; i++;
                value[i] = aI64.Ai48; i++;
                value[i] = aI64.Ai49; i++;
                value[i] = aI64.Ai50; i++;
                value[i] = aI64.Ai51; i++;
                value[i] = aI64.Ai52; i++;
                value[i] = aI64.Ai53; i++;
                value[i] = aI64.Ai54; i++;
                value[i] = aI64.Ai55; i++;
                value[i] = aI64.Ai56; i++;
                value[i] = aI64.Ai57; i++;
                value[i] = aI64.Ai58; i++;
                value[i] = aI64.Ai59; i++;
                value[i] = aI64.Ai60; i++;
                value[i] = aI64.Ai61; i++;
                value[i] = aI64.Ai62; i++;
                value[i] = aI64.Ai63; i++;
                value[i] = aI64.Ai64;
                conn.Execute(Procedure, new { nowTime = aI64.ttime, CaseNo1 = config.CaseNo, AiNo1 = config.AiNo, Ai1 = config.Ai, NowData = value[Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(config.Ai, @"[^0-9]+", "")) - 1] }, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
