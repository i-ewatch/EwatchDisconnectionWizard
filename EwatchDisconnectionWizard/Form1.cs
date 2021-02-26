using EwatchDisconnectionWizard.Components;
using EwatchDisconnectionWizard.Configration;
using EwatchDisconnectionWizard.Methods;
using EwatchDisconnectionWizard.MySql_Module;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EwatchDisconnectionWizard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\log\\log-.txt",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();        //宣告Serilog初始化
            MySqlSetting = InitialMethod.MySqlLoad();
            mySqlMethod = new MySqlMethod(MySqlSetting);
            mySqlComponent = new MySqlComponent(mySqlMethod);
            mySqlComponent.MyWorkState = true;
            MySqlAlarmComponent = new MySqlAlarmComponent(mySqlMethod);
            MySqlAlarmComponent.MyWorkState = true;
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }
        private MySqlMethod mySqlMethod { get; set; }
        private MySqlSetting MySqlSetting { get; set; }
        private MySqlComponent mySqlComponent { get; set; }
        private MySqlAlarmComponent MySqlAlarmComponent { get; set; }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MySqlTimelabel.Text = mySqlComponent.ConnectionTime.ToString("G");
            MySqlAlarmTimelabel.Text = MySqlAlarmComponent.ConnectionTime.ToString("G");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mySqlComponent.MyWorkState = false;
            MySqlAlarmComponent.MyWorkState = false;
            this.Dispose();
        }
    }
}
