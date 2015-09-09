using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Ironhide.TemplateEngines.Razor
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object obj)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            Type type = obj.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                expando.Add(property.Name, property.GetValue(obj));
            }

            foreach (FieldInfo field in type.GetFields())
            {
                expando.Add(field.Name, field.GetValue(obj));
            }

            return expando as ExpandoObject;
        }
    }
}