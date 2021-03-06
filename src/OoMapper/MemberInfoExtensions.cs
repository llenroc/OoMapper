using System;
using System.Reflection;

namespace OoMapper
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberType(this MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                return (memberInfo as PropertyInfo).PropertyType;
            if (memberInfo is FieldInfo)
                return (memberInfo as FieldInfo).FieldType;
			if (memberInfo is MethodInfo)
				return (memberInfo as MethodInfo).ReturnType;
            throw new NotSupportedException();
        }
    }
}