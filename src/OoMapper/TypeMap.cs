using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OoMapper
{
    public class TypeMap
    {
        private readonly IMappingConfiguration configuration;
        private readonly Type destinationType;
        private readonly ICollection<PropertyMap> propertyMaps;
        private readonly Type sourceType;

        public TypeMap(Type sourceType, Type destinationType, IMappingConfiguration configuration)
        {
            PropertyInfo[] sourceMembers = sourceType.GetProperties();
            PropertyInfo[] destinationMembers = destinationType.GetProperties();

            this.sourceType = sourceType;
            this.destinationType = destinationType;
            this.configuration = configuration;

            propertyMaps = destinationMembers
                .Select(destination => new PropertyMap(destination, FindMembers(destination, sourceMembers)))
                .ToList();
        }

        private SourceMemberResolver FindMembers(PropertyInfo destination, IEnumerable<PropertyInfo> sourceMembers)
        {
            var propertyInfos = new List<PropertyInfo>();
            FindMembers(propertyInfos, destination.Name, sourceMembers);
            return new SourceMemberResolver(propertyInfos, configuration);
        }

        public LambdaExpression BuildNew()
        {
            const string name = "src";

            ParameterExpression source = Expression.Parameter(sourceType, name);

            MemberAssignment[] bindings = propertyMaps
                .Where(x => x.IsIgnored == false)
                .Select(m => m.BuildBind(source))
                .ToArray();

            return Expression.Lambda(
                Expression.MemberInit(
                    Expression.New(destinationType), bindings), source);
        }

        public LambdaExpression BuildExisting()
        {
            const string name = "src";

            ParameterExpression source = Expression.Parameter(sourceType, name);
            ParameterExpression destination = Expression.Parameter(destinationType, "dst");

            Expression[] bindings = propertyMaps
                .Where(x => x.IsIgnored == false)
                .Select(m => m.BuildAssign(destination, source))
                .Concat(new[] {destination})
                .ToArray();

            return Expression.Lambda(
                Expression.Block(bindings),
                source,
                destination);
        }

        private static void FindMembers(ICollection<PropertyInfo> list, string name,
                                        IEnumerable<PropertyInfo> sourceMembers)
        {
            if (String.IsNullOrEmpty(name))
                return;
            PropertyInfo propertyInfo =
                sourceMembers.FirstOrDefault(pi => name.StartsWith(pi.Name, StringComparison.InvariantCultureIgnoreCase));
            if (propertyInfo == null)
                throw new NotSupportedException();

            list.Add(propertyInfo);
            FindMembers(list, name.Substring(propertyInfo.Name.Length), propertyInfo.PropertyType.GetProperties());
        }

        public PropertyMap GetPropertyMapFor(MemberInfo destinationMember)
        {
            return propertyMaps.FirstOrDefault(map => map.DestinationMember == destinationMember);
        }
    }
}