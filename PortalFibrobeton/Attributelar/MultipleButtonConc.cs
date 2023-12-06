using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PortalFibrobeton.Attributelar
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonConc : ActionNameSelectorAttribute
    {
        public string Ad { get; set; }
        public string Arguman { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValid = false;
            isValid = controllerContext.HttpContext.Request[Ad] != null &&
                controllerContext.HttpContext.Request[Ad] == Arguman;

            return isValid;
        }
    }
}