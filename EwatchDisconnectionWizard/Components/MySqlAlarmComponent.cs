using EwatchDisconnectionWizard.Methods;
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
        }

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
                    var Alarmdata = MySqlMethod.AiConfig_Compare_Load();
                    foreach (var item in Alarmdata)
                    {
                        var value = MySqlMethod.Ai64web(item);
                        MySqlMethod.Alarm_Procedure(item, value);
                    }
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
