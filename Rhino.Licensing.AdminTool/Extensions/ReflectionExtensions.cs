using System;
using System.Reflection;

namespace Rhino.Licensing.AdminTool.Extensions
{
    public static class ReflectionExtensions
    {
        public static T GetAttribute<T>(this ICustomAttributeProvider attributeProvider)
            where T : Attribute
        {
            return GetAttribute<T>(attributeProvider, true);
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider attributeProvider, bool inherit)
            where T : Attribute
        {
            var attribs = attributeProvider.GetCustomAttributes(typeof (T), inherit);
            return attribs != null ? (T)attribs[0] : null;
        }
    }
}