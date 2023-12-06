using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PortalFibrobeton.Attributelar
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonResults : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValid = false;
            isValid = controllerContext.HttpContext.Request[Name] != null &&
                controllerContext.HttpContext.Request[Name] == Argument;

            return isValid;
        }
    }
}