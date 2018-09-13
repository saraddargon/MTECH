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
        public static bool IsSave(string Mssg = "ต้องการบันทึก ?")
        {
            return Question(Mssg, "บันทึก");
        }
        public static bool IsDel(string Mssg = "ต้องการลบ ?")
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

        public static decimal StockQty (string ItemNo, string LocationCode)
        {
            using (var db = new DataClasses1DataContext())
            {
                var g = db.Cal_QTY_Remain_Location(ItemNo, "", 0, LocationCode).Value.ToDecimal();
                return g;
            }
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

    }
    

    public enum TypeAction
    {
        Add,
        Edit,
        Delete,
        View
    }
    
    

}
