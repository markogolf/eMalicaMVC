using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eMalicaMVC.Common
{
    public class CheckAuthorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //if (HttpContext.Current.Session["UserID"] == null || !HttpContext.Current.Request.IsAuthenticated)
            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 302; //Found Redirection to another page. Here- login page. Check Layout ajaxError() script.
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectResult(System.Web.Security.FormsAuthentication.LoginUrl + "?ReturnUrl=" +
                         filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                }
            }
            else
            {

                ////Code HERE for page level authorization
                //if (filterContext.Controller is PetrolEditorMVC.Controllers.AdminController && HttpContext.Current.Session["UserRole"].ToString() != "admin")
                //{
                //    filterContext.HttpContext.Response.StatusCode = 302;
                //    filterContext.HttpContext.Response.End();
                //}
                //else
                //{

                //}
            }
            
        }
    }
}