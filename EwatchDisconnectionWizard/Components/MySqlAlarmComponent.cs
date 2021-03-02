using EwatchDisconnectionWizard.Enums;
using EwatchDisconnectionWizard.Methods;
using EwatchDisconnectionWizard.MySql_Module;
using LineNotifyLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EwatchDisconnectionWizard.Components
{
    public partial class MySqlAlarmComponent : Field4Component
    {
        public MySqlAlarmComponent(MySqlMethod mySqlMethod)
        {
            InitializeComponent();
            MySqlMethod = mySqlMethod;
            AiConfigs = MySqlMethod.AiConfig_Compare_Load();
        }
        private List<AiConfig> AiConfigs { get; set; } = new List<AiConfig>();
        private List<CaseSetting> CaseSettings { get; set; } = new List<CaseSetting>();
        public MySqlAlarmComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                ConnectionThread = new System.Threading.Thread(Connection_Mysql);
                ConnectionThread.Start();
            }
            else
            {
                if (ConnectionThread != null)
                {
                    ConnectionThread.Abort();
                }
            }
        }
        private void Connection_Mysql()
        {
            while (myWorkState)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(ConnectionTime);
                if (timeSpan.TotalMilliseconds > 1000)
                {
                    CaseSettings = MySqlMethod.CaseLoad();
                    var Alarmdata = MySqlMethod.AiConfig_Compare_Load();
                    foreach (var item in Alarmdata)
                    {
                        var caseSetting = CaseSettings.Where(g => g.CaseNo == item.CaseNo).ToList()[0];
                        var value = MySqlMethod.Ai64web(item);
                        var nowdata = MySqlMethod.Alarm_Procedure(item, value);

                        var Adata = AiConfigs.Where(g => g.PK == item.PK).ToList();
                        if (Adata.Count > 0)
                        {
                            if (item.CompareType != Adata[0].CompareType)
                            {
                                switch (item.CompareType)
                                {
                                    case 0: //上限
                                        {
                                            LineNotifyClass lineNotifyClass = new LineNotifyClass();
                                            lineNotifyClass.LineNotifyFunction(caseSetting.NotifyToken, $"\r案場名稱: {caseSetting.TitleName}\r點位名稱: {item.AiName}\r超過上限值: {item.AiMax}\r目前數值為 {nowdata}");
                                        }
                                        break;
                                    case 1: //正常
                                        {
                                            LineNotifyClass lineNotifyClass = new LineNotifyClass();
                                            lineNotifyClass.LineNotifyFunction(caseSetting.NotifyToken, $"\r案場名稱: {caseSetting.TitleName}\r點位名稱: {item.AiName}\r上限值: {item.AiMax}\r下限值: {item.AiMin}\r恢復正常，目前數值為 {nowdata}");
                                        }
                                        break;
                                    case 2: //下限
                                        {
                                            LineNotifyClass lineNotifyClass = new LineNotifyClass();
                                            lineNotifyClass.LineNotifyFunction(caseSetting.NotifyToken, $"\r案場名稱: {caseSetting.TitleName}\r點位名稱: {item.AiName}\r低於下限值: {item.AiMin}\r目前數值為 {nowdata}");
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    AiConfigs = Alarmdata;
                    ConnectionTime = DateTime.Now;
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
