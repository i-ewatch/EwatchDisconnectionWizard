using EwatchDisconnectionWizard.Configration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.Methods
{
    public class InitialMethod
    {
        /// <summary>
        /// 初始路徑
        /// </summary>
        private static string MyWorkPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public static MySqlSetting MySqlLoad()
        {
            MySqlSetting setting = null;
            if (!Directory.Exists($"{MyWorkPath}\\stf"))
                Directory.CreateDirectory($"{MyWorkPath}\\stf");
            string SettingPath = $"{MyWorkPath}\\stf\\MySql.json";
            try
            {
                if (File.Exists(SettingPath))
                {
                    string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<MySqlSetting>(json);
                }
                else
                {
                    MySqlSetting Setting = new MySqlSetting()
                    {
                        DataSource = "127.0.0.1",
                        InitialCatalog = "Environment",
                        UserID = "root",
                        Password = "1234"
                    };
                    setting = Setting;
                    string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                    File.WriteAllText(SettingPath, output);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, " MySQLDB資訊設定載入錯誤");
            }
            return setting;
        }
    }
}
