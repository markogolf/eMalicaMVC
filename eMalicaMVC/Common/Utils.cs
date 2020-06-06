using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eMalicaMVC.Common
{
    public class Utils
    {
        public static IEnumerable<SelectListItem> GetExtraStatusList(string currentStatus)
        {
            var res = new List<SelectListItem>();
            foreach (var status in Properties.Settings.Default.ExtraStatus)
            {
                SelectListItem item = new SelectListItem();
                item.Value = status;
                item.Text = status;
                item.Selected = status == currentStatus;
                res.Add(item);
            }

            return res;
        }

        //public static DateTime GetMondayFromWeekNumber(string weekNumberYear)
        //
        //    var arr = weekNumberYear.Split('|');

        //    var weekNumber = int.Parse(arr[0]);
        //    var year = int.Parse(arr[1]);

        //    DateTime startOfYear = new DateTime(year, 1, 1);

        //    // The +7 and %7 stuff is to avoid negative numbers etc.
        //    int daysToFirstCorrectDay = ((1- (int)startOfYear.DayOfWeek) + 7) % 7;
        //    return startOfYear.AddDays(7 * (weekNumber - 1) + daysToFirstCorrectDay);
        //}

        //public static int GetWeekNumberFromDate(DateTime date)
        //{
        //    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        //    Calendar cal = dfi.Calendar;

        //    return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        //}


        //public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        //{
        //    DateTime jan1 = new DateTime(year, 1, 1);
        //    int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

        //    // Use first Thursday in January to get first week of the year as
        //    // it will never be in Week 52/53
        //    DateTime firstThursday = jan1.AddDays(daysOffset);
        //    var cal = CultureInfo.CurrentCulture.Calendar;
        //    int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        //    var weekNum = weekOfYear;
        //    // As we're adding days to a date in Week 1,
        //    // we need to subtract 1 in order to get the right date for week #1
        //    if (firstWeek == 1)
        //    {
        //        weekNum -= 1;
        //    }

        //    // Using the first Thursday as starting week ensures that we are starting in the right year
        //    // then we add number of weeks multiplied with days
        //    var result = firstThursday.AddDays(weekNum * 7);

        //    // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
        //    return result.AddDays(-3);
        //}
    }
}