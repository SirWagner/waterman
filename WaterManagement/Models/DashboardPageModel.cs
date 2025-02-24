using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterManagement.Models
{
    public class DashboardPageModel
    {
        
            public float OrderTotal { get; set; }
            public float ProjectOrderTotal { get; set; }
            public float ProjectOrderConfirm { get; set; }
            public float ProjectOrderConfirmProgress { get; set; }
            public string ProjectOrderConfirmProgressString { get; set; }
            public float SalesOrderTotal { get; set; }
            public float SalesOrderConfirm { get; set; }
            public float SalesOrderConfirmProgress { get; set; }
            public string SalesOrderConfirmProgressString { get; set; }
            public float ServiceOrderTotal { get; set; }
            public float ServiceOrderConfirm { get; set; }
            public float ServiceOrderConfirmProgress { get; set; }
            public string ServiceOrderConfirmProgressString { get; set; }
            //public List<ProjectOrderReadDto> LastFiveProjectOrders { get; set; }
            //public List<MonthlyEarningDto> MonthlyEarnings { get; set; }
            public string CurrencyName { get; set; }
            public int CountStage1 { get; set; }
            public int CountStage2 { get; set; }
            public int CountStage3 { get; set; }
        }
    }

