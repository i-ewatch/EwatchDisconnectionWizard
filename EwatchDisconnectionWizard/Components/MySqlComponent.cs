﻿using EwatchDisconnectionWizard.Enums;
using EwatchDisconnectionWizard.Methods;
using LineNotifyLibrary;
using TelegramLibrary;
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
    public partial class MySqlComponent : Field4Component
    {
        public MySqlComponent(MySqlMethod mySqlMethod)
        {
            InitializeComponent();
            MySqlMethod = mySqlMethod;
        }

        public MySqlComponent(IContainer container)
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
        protected void Connection_Mysql()
        {
            while (myWorkState)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(ConnectionTime);
                if (timeSpan.TotalMilliseconds > 1000) //每一秒查詢動作
                {
                    var casevalue = MySqlMethod.CaseLoad();
                    foreach (var caseitem in casevalue)
                    {
                        if (caseitem.NotifyTypeEnum != 0)// 檢查是否使用發報功能 0 =不使用
                        {
                            var aivalue = MySqlMethod.AiLoad(caseitem);
                            foreach (var aiitem in aivalue)
                            {
                                if (aiitem.NotifyFlag)//AI點位需要發報
                                {
                                    if (MySqlMethod.AI64Load(aiitem))//檢查即時數值是否超過時間
                                    {
                                        var TimeValue = MySqlMethod.Ai_Time(aiitem);//檢查發報數值是否達到
                                        if (TimeValue == null || TimeValue >= aiitem.TimeoutSpan)
                                        {
                                            NotifyTypeEnum notifyTypeEnum = (NotifyTypeEnum)caseitem.NotifyTypeEnum;
                                            switch (notifyTypeEnum)
                                            {
                                                case NotifyTypeEnum.None:
                                                    break;
                                                case NotifyTypeEnum.Line:
                                                    {
                                                        LineNotifyClass lineNotifyClass = new LineNotifyClass();
                                                        lineNotifyClass.LineNotifyFunction(caseitem.NotifyToken, $"設備名稱:{aiitem.AiName} \r最後上傳時間 : {MySqlMethod.AI_LastTime(aiitem)}\r上傳逾時請檢查");
                                                    }
                                                    break;
                                                case NotifyTypeEnum.Telegram:
                                                    {
                                                        TelegramBotClass telegramBotClass = new TelegramBotClass(caseitem.NotifyApi) { Chat_ID = caseitem.NotifyToken };
                                                        telegramBotClass.Send_Message_Group($"設備名稱:{aiitem.AiName} \r最後上傳時間 : {MySqlMethod.AI_LastTime(aiitem)}\r上傳逾時請檢查");
                                                    }
                                                    break;
                                            }
                                            MySqlMethod.UpdataAi_Time(aiitem, true);
                                        }
                                    }
                                    else
                                    {
                                        MySqlMethod.UpdataAi_Time(aiitem, false);
                                    }
                                }
                            }
                            var electricvalue = MySqlMethod.ElectricLoad(caseitem);
                            foreach (var electricitem in electricvalue)
                            {
                                if (electricitem.NotifyFlag)//電表點位需要發報
                                {
                                    if (MySqlMethod.ElectricMeterLoad(electricitem))
                                    {
                                        var TimeValue = MySqlMethod.ElectricMeter_Time(electricitem);
                                        if (TimeValue == null || TimeValue >= electricitem.TimeoutSpan)
                                        {
                                            NotifyTypeEnum notifyTypeEnum = (NotifyTypeEnum)caseitem.NotifyTypeEnum;
                                            switch (notifyTypeEnum)
                                            {
                                                case NotifyTypeEnum.None:
                                                    break;
                                                case NotifyTypeEnum.Line:
                                                    {
                                                        LineNotifyClass lineNotifyClass = new LineNotifyClass();
                                                        lineNotifyClass.LineNotifyFunction(caseitem.NotifyToken, $"設備名稱:{electricitem.ElectricName} \r最後上傳時間 : {MySqlMethod.ElectricMeter_LastTime(electricitem)}\r上傳逾時請檢查");
                                                    }
                                                    break;
                                                case NotifyTypeEnum.Telegram:
                                                    {
                                                        TelegramBotClass telegramBotClass = new TelegramBotClass(caseitem.NotifyApi) { Chat_ID = caseitem.NotifyToken };
                                                        telegramBotClass.Send_Message_Group($"設備名稱:{electricitem.ElectricName} \r最後上傳時間 : {MySqlMethod.ElectricMeter_LastTime(electricitem)}\r上傳逾時請檢查");
                                                    }
                                                    break;
                                            }
                                            MySqlMethod.UpdataElectricMeter_Time(electricitem, true);
                                        }
                                    }
                                    else
                                    {
                                        MySqlMethod.UpdataElectricMeter_Time(electricitem, false);
                                    }
                                }
                            }
                        }
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
