using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace StockControl
{
    public static class baseClass
    {
        public static string ToSt(this object val)
        {
            try
            {
                if (val == null)
                    return "";
                else
                    return Convert.ToString(val);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static int ToInt(this object val)
        {
            try
            {
                if (val == null)
                    return 0;
                else
                    return Convert.ToInt32(val);
            }
            catch
            {
                return 0;
            }
        }
        public static double ToDouble(this object val)
        {
            try
            {
                if (val == null)
                    return 0;
                else
                    return Convert.ToDouble(val);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ToDecimal(this object val)
        {
            try
            {
                if (val == null)
                    return 0.00m;
                else
                    return Math.Round(Convert.ToDecimal(val), 2);
            }
            catch
            {
                return 0.00m;
            }
        }
        public static bool ToBool(this object val)
        {
            try
            {
                if (val == null)
                    return false;
                else
                    return Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
        }
        public static DateTime? ToDateTime(this object val)
        {
            try
            {
                return (DateTime?)Convert.ToDateTime(val);
            }
            catch
            {
                return null;
            }
        }
        public static string ToDtString(this DateTime val)
        {
            return val.ToString("dd/MMM/yyyy");
        }
        public static string ToDtTimeString(this DateTime val)
        {
            return val.ToString("dd/MMM/yyyy HH:mm");
        }

        public static void Info(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Infomation", MessageBoxButtons.OK, RadMessageIcon.Info);
        }
        public static void Error(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
        }
        public static void Warning(string Mssg)
        {
            RadMessageBox.Show(Mssg, "Warning", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
        }
        public static bool Question(string Mssg, string Caption = "Question")
        {
            return RadMessageBox.Show(Mssg, Caption, MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes;
        }
        public static bool IsSave(string Mssg = "Do you want to 'Save' ?")
        {
            return Question(Mssg, "บันทึก");
        }
        public static bool IsDel(string Mssg = "Do you want to 'Delete' ?")
        {
            return Question(Mssg, "ลบ");
        }

        public static string ToMoney(this object val)
        {
            try
            {
                return val.ToDecimal().ToString("#,0.00");
            }
            catch { return "0.00"; }
        }

        public static T ToEnum<T>(this object val)
        {
            return (T)Enum.Parse(typeof(T), val.ToSt(), true);
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static decimal StockQty(string ItemNo, string LocationCode)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.Cal_QTY_Remain_Location(ItemNo, "", 0, LocationCode).Value.ToDecimal();
                return g;
            }
        }

        public static int getDayOfWeek(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return 6;
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                default: return 5; //Saturday
            }
        }
        public static TimeSpan setTimeSpan(string TimeText)
        {
            TimeSpan t = new TimeSpan();
            return t;
        }
        public static ReorderType getReorderType(string ReorderTypeSt)
        {
            if (ReorderTypeSt == "Fixed Reorder Qty")
                return ReorderType.Fixed;
            else if (ReorderTypeSt == "Minimum & Maximum Qty")
                return ReorderType.MinMax;
            else
                return ReorderType.ByOrder;
        }
        public static ReplenishmentType getRepType(string RepTypeSt)
        {
            if (RepTypeSt == "Production")
                return ReplenishmentType.Production;
            else
                return ReplenishmentType.Purchase;
        }
        public static InventoryGroup getInventoryGroup(string InvGroupSt)
        {
            if (InvGroupSt == "FG")
                return InventoryGroup.FG;
            else if (InvGroupSt == "SEMI")
                return InventoryGroup.SEMI;
            else
                return InventoryGroup.RM;
        }
        public static DateTime setStandardTime(DateTime dt, bool TimeStart)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.mh_ManufacturingSetups.FirstOrDefault();
                if (TimeStart)
                    dt = dt.AddHours(g.StandardStartingTime.Hours).AddMinutes(g.StandardStartingTime.Minutes);
                else
                    dt = dt.AddHours(g.StandardEndingTime.Hours).AddMinutes(g.StandardEndingTime.Minutes);
                return dt;
            }
        }

        public static List<WorkLoad> getWorkLoad(DateTime beginDate)
        {
            var workLoads = new List<WorkLoad>();
            using (var db = new DataClasses1DataContext())
            {
                var d = db.mh_CapacityAvailables.Where(x => x.Date >= beginDate).ToList();
                foreach (var dd in d)
                {
                    var m = db.mh_CapacityLoads.Where(x => x.Date == dd.Date
                            && x.WorkCenterID == dd.WorkCenterID && x.Active).ToList();
                    decimal loadCapa = 0.00m;
                    if (m.Count > 0)
                        loadCapa = m.Sum(x => x.Capacity);
                    var wl = workLoads.Where(x => x.Date == dd.Date
                        && x.idWorkCenter == dd.WorkCenterID).FirstOrDefault();
                    if (wl == null)
                    {
                        wl = new WorkLoad();
                        workLoads.Add(wl);
                    }
                    wl.idWorkCenter = dd.WorkCenterID;
                    wl.Date = dd.Date.Date;
                    wl.CapacityAvailable = dd.Capacity.ToDecimal();
                    wl.CapacityAlocate += loadCapa;
                }
                return workLoads;
            }
        }
        public static DateTime setWorkTime(DateTime d, int idWorkCenter, decimal CapaLoad)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var m = db.mh_WorkCenters.Where(x => x.id == idWorkCenter).First();
                    int dow = getDayOfWeek(d.DayOfWeek);
                    var c = db.mh_WorkingDays.Where(x => x.idCalendar == m.Calendar && x.Active
                                && x.Day == dow).ToList();
                    TimeSpan sTime = setTimeSpan(c.OrderBy(x => x.StartingTime.Replace(":", "").ToInt()).First().StartingTime);
                    //find Ending time
                    var endingTime = "00:00";
                    if (c.Where(x => x.EndingTime.Replace(":", "").ToInt() == 0).Count() < 1)
                    {
                        endingTime = c.OrderByDescending(x => x.EndingTime.Replace(":", "").ToInt()).First().EndingTime;
                    }
                    TimeSpan eTime = setTimeSpan(endingTime);

                    //find Time

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return d;
        }

        //Customer P/O Status
        public static string setCustomerPOStatus(mh_CustomerPODT dt)
        {
            var fullQty = dt.Qty * dt.PCSUnit;
            if (dt.OutPlan == fullQty && dt.OutSO == fullQty)
                return "Waiting";
            else if (dt.OutPlan != fullQty || dt.OutSO != fullQty)
                return "Process";
            else if (dt.OutPlan == 0 && dt.OutSO == 0)
                return "Completed";
            else
                return "Waiting";
        }
    }


    public enum TypeAction
    {
        Add,
        Edit,
        Delete,
        View
    }



}
