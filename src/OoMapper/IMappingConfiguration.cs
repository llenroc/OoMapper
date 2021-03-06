using System;
using System.Linq.Expressions;

namespace OoMapper
{
    public interface IMappingConfiguration : ISourceMemberResolver
    {
        LambdaExpression BuildNew(Type sourceType, Type destinationType);
        LambdaExpression BuildExisting(Type sourceType, Type destinationType);
    }
}