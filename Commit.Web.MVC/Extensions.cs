using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Commit.Web.Models.BasicViewModels;
using Infrastructure.Domain;

namespace Commit.Web
{
    public static class Extensions
    {
        
        public static IEnumerable<DropDownViewModel> ToDropDown<TSource>(this IQueryable<TSource> query,
            Expression<Func<TSource, string>> displayField) where TSource : EntityBase<long>
        {
            //make sure we have the expression

            if (displayField == null)
            {
                throw new ArgumentNullException("displayField");
            }


            //Get model information

            var memberExpression = displayField.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression");


            //Not quite sure what this does, but it's cool

            var item1 = "Id";
            var item2 = memberExpression.Member.Name;
            var sourceType = typeof(TSource);


            var itemParam = Expression.Parameter(sourceType, "x");
            var member1 = Expression.PropertyOrField(itemParam, item1);
            var member2 = Expression.PropertyOrField(itemParam, item2);
            var selector = Expression.MemberInit(Expression.New(typeof(DropDownViewModel)),
                Expression.Bind(typeof(DropDownViewModel).GetMember("Value").Single(), member1),
                Expression.Bind(typeof(DropDownViewModel).GetMember("Text").Single(), member2)
            );
            var lambda = Expression.Lambda<Func<TSource, DropDownViewModel>>(
                selector, itemParam).Compile();
            return query.Select(lambda);



        }
    }
}
