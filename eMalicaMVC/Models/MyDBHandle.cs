using Dapper;
using eMalicaMVC.Common;
using eMalicaMVC.ViewModels;
using eMalicaMVC.ViewModels.Reports;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static eMalicaMVC.Common.Enums;

namespace eMalicaMVC.Models
{
    public class MyDBHandle
    {
        private SqlConnection GetConnection()
        {
            string constring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            return new SqlConnection(constring);

        }

        internal Menu GetMenu(int menuId)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT m.Id, m.Date, m.SortID, m.name, m.Price
                            FROM Menus m
                            WHERE ID = @menuId";

                return conn.QuerySingleOrDefault<Menu>(sql, new { menuId });
            }
        }

        internal IEnumerable<SubViewModel> GetMenus(string userId, DateTime from, DateTime to)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT m.Id, m.Date, m.SortID, m.name, m.Price
                            FROM Menus m
                            WHERE m.Date >= @from AND m.Date <= @to
                            order by m.Date";

                var resMenus = conn.Query<SubViewModel>(sql, new { userId, from = from.Date, to = to.Date });
                foreach (var menu in resMenus)
                {
                    menu.UserId = userId;
                    string sqlSub = @"select top 1 * from
                                    MenuOrders where MenuId = @id and UserID = @userId
                                    order by id desc";

                    var order = conn.QueryFirstOrDefault<MenuOrder>(sqlSub, new { userId, id = menu.Id });
                    menu.MenuOrder = order;
                }

                return resMenus;
            }
        }

        internal List<SelectListItem> GetUsers(string selectedUserId)
        {
            using (var conn = GetConnection())
            {
                List<SelectListItem> users = new List<SelectListItem>();
                //users.Add(new SelectListItem());
                string sql = @"select id, UserName from AspNetUsers";

                var res = conn.Query(sql);


                foreach (var r in res)
                {
                    SelectListItem li = new SelectListItem();
                    li.Text = r.UserName;
                    li.Value = r.id;
                    li.Selected = r.id == selectedUserId;
                    users.Add(li);
                }

                return users;
            }
        }

        internal IEnumerable<MenuReportViewModel> GetMenusForReport(string userId, DateTime from, DateTime to)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT m.Id, m.Date, m.SortID, m.name, m.Price
                            FROM Menus m
                            WHERE m.Date >= @from AND m.Date <= @to
                            order by m.Date";

                var resMenus = conn.Query<MenuReportViewModel>(sql, new { userId, from = from.Date, to = to.Date });
                foreach (var menu in resMenus)
                {
                    string sqlSub = @"select * from
                                    MenuOrders where MenuId = @id and MenuOrders.Status != 'preklican'
                                    order by id desc";
                    
                    var orders = conn.Query<MenuOrder>(sqlSub, new { id = menu.Id });
                    menu.NumOfOrders = orders.Count();

                    menu.MenuOrders = orders.ToList();
                }

                return resMenus;
            }
        }

        internal MenuOrder GetUserMenuOrder(int menuId, string userId)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT * FROM MenuOrders WHERE UserId = @userId AND MenuId = @menuId";

                var res = conn.QuerySingleOrDefault<MenuOrder>(sql, new { menuId, userId });
                return res;
            }
        }

        internal bool DeleteMenu(int id)
        {
            using (var conn = GetConnection())
            {
                string sql = @"Delete FROM Menus WHERE ID = @id";

                int res = conn.Execute(sql, new { id });
                if (res > 0)
                    return true;
                else
                    return false;

            }
        }

        internal void SetOrderStatus(int orderId, string status)
        {
            using (var conn = GetConnection())
            {
                string sql = "UPDATE MenuOrders SET Status = @status WHERE ID = @orderId";
                conn.Execute(sql, new { status, orderId });
            }
        }

        internal void SetOrderStatus(int orderId, string status, string comment)
        {
            using (var conn = GetConnection())
            {
                string sql = "UPDATE MenuOrders SET Status = @status, Comment = @comment  WHERE ID = @orderId";
                conn.Execute(sql, new { status, orderId, comment });
            }
        }

        internal MenuOrder GetOrder(int orderId)
        {
            using (var conn = GetConnection())
            {
                string sql = @"select *
                        FROM [dbo].[MenuOrders]
                        where ID = @orderId";

                var res = conn.QuerySingleOrDefault<MenuOrder>(sql, new { orderId });
                return res;
            }
        }

        internal List<SelectListItem> GetWeeks(DateTime selectedDate, bool extendByEmptyElement)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT DATEPART(wk, [Date]) as WeekNo, YEAR([Date]) as [year], MIN([Date]) AS DayInWeek
                        FROM Menus
                        GROUP BY YEAR([Date]), DATEPART(wk, [Date])
                        ORDER BY YEAR([Date]), DATEPART(wk, [Date])";

                var res = conn.Query(sql);
                List<SelectListItem> weeks = new List<SelectListItem>();
                foreach (var w in res)
                {
                    SelectListItem li = new SelectListItem();
                    DateTime dateInWeek = w.DayInWeek;

                    DateTime selectedFrom = dateInWeek.AddDays(-(int)dateInWeek.DayOfWeek + (int)DayOfWeek.Monday);
                    DateTime selectedTo = selectedFrom.AddDays(6);

                    li.Text = $"{selectedFrom.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} - {selectedTo.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";
                    //li.Value = Utils.GetWeekNumberFromDate(selectedFrom)+"|"+ selectedFrom.Year; 
                    li.Value = selectedFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    li.Selected = selectedDate.Date >= selectedFrom && selectedDate.Date <= selectedTo;
                   

                    weeks.Add(li);
                }

                if (extendByEmptyElement)
                {
                    weeks.Insert(0, new SelectListItem { Selected = false, Value = "" });
                }

                return weeks;
            }
        }

        internal List<SelectListItem> GetMonths(DateTime selectedDate, bool extendByEmptyElement)
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT DATEPART(mm, [Date]) as Month, YEAR([Date]) as [Year], MIN([Date]) AS DayInWeek
FROM Menus
GROUP BY YEAR([Date]), DATEPART(mm, [Date])
ORDER BY YEAR([Date]), DATEPART(mm, [Date])";

                var res = conn.Query(sql);
                List<SelectListItem> months = new List<SelectListItem>();
                foreach (var m in res)
                {
                    SelectListItem li = new SelectListItem();
                    int year = m.Year;
                    int month = m.Month;

                    li.Text = $"{month} - {year}";
                    //li.Value = Utils.GetWeekNumberFromDate(selectedFrom)+"|"+ selectedFrom.Year; 
                    li.Value = new DateTime(year, month, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    li.Selected = selectedDate.Year == year && selectedDate.Month == month;

                    months.Add(li);
                }

                months.Insert(0, new SelectListItem { Selected = false, Value = "" });
                return months;
            }

        }


        internal IEnumerable<MenuOrderDetail> GetMenuOrderDetails(int menuId)
        {
            using (var conn = GetConnection())
            {
                string sql = @"select username, Menus.Date, Menus.Name, MenuOrders.Status, MenuOrders.Comment
                        from AspNetUsers
                        join MenuOrders on MenuOrders.UserId = AspNetUsers.Id
                        join Menus on MenuOrders.MenuId = Menus.Id
                        where Menus.Id = @menuId";

                var res = conn.Query<MenuOrderDetail>(sql, new  { menuId });

                return res;
            }
        }

        internal bool CreateOrder(int id, string comment, string userId)
        {
            using (var conn = GetConnection())
            {
                string sql = @"INSERT INTO [dbo].[MenuOrders]
           ([UserId]
           ,[MenuId]
           ,[Status]
           ,[Comment])
     VALUES (@userid, @menuId, @Status,@Comment)
";

                var i = conn.Execute(sql, new { userId, menuId = id, Status = Enums.OrderStatus.Ordered, Comment = comment  });

                if (i >= 1)
                    return true;
                else
                    return false;

            }
        }

        public bool AddMenu(Menu m)
        {
            m.TrimAllStrings();

            using (var conn = GetConnection())
            {
                string sql = @"insert into Menus(Date, SortId, Name, Price)
                            values(@Date, @SortId, @Name, @Price)";

                var i = conn.Execute(sql, new { m.Date, m.SortId, m.Name, m.Price });

                if (i >= 1)
                    return true;
                else
                    return false;
            }
        }

        internal bool UpdateMenu(Menu m)
        {
            m.TrimAllStrings();

            using (var conn = GetConnection())
            {
                string sql = @"UPDATE Menus
                               SET Date = @Date,
                               Name = @Name,
                               Price =@Price
                               WHERE Id = @Id";

                var i = conn.Execute(sql, new { m.Date, m.Name, m.Price, m.Id });

                if (i >= 1)
                    return true;
                else
                    return false;
            }

        }
        internal AdminSettings GetAdminSettings()
        {
            using (var conn = GetConnection())
            {
                string sql = @"SELECT * from AdminSettings";

                var res = conn.Query<Menu>(sql);
                //return res;
                return null;
            }
        }

        internal bool CancelOrder(int id)
        {

            using (var conn = GetConnection())
            {
                string sql = @"UPDATE [dbo].[MenuOrders]
                           SET [Status] = @Status
                         WHERE Id = @Id
                        ";

                int i = conn.Execute(sql, new { Status = OrderStatus.Cancelled, Id = id } );
                if (i >= 1)
                    return true;
                else
                    return false;
            }
        }
    }
}