using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kendo.Mvc.UI
{
    public static class ReadOnlyCustomDataSourceBuilderExtension
    {
        public static ReadOnlyCustomDataSourceBuilder Filter<TModel>(this ReadOnlyCustomDataSourceBuilder builder, Action<DataSourceFilterDescriptorFactory<TModel>> configurator) where TModel : class
        {

            var field = builder.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(a => a.Name == "dataSource").FirstOrDefault();

            var dataSource = field.GetValue(builder) as Kendo.Mvc.UI.DataSource;

            configurator(new DataSourceFilterDescriptorFactory<TModel>(dataSource.Filters));

            return builder;
        }
        public static ReadOnlyCustomDataSourceBuilder Filter<TModel>(this ReadOnlyCustomDataSourceBuilder builder, Expression<Func<TModel, bool>> exp) where TModel : class
        {

            var filter = ExpressionToKendo(exp);

            var field = builder.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(a => a.Name == "dataSource").FirstOrDefault();

            var dataSource = field.GetValue(builder) as Kendo.Mvc.UI.DataSource;

            dataSource.Filters.Add(filter);

            return builder;
        }
        public static IFilterDescriptor ExpressionToKendo<TModel>(Expression<Func<TModel, bool>> exp)
        {
            return ProcessExpression(exp.Body);
        }
        public static IFilterDescriptor ProcessExpression(Expression exp)
        {
            IFilterDescriptor result = null;

            if (exp is BinaryExpression)
            {
                var bexp = exp as BinaryExpression;
                if (bexp.NodeType == ExpressionType.And || bexp.NodeType == ExpressionType.AndAlso || bexp.NodeType == ExpressionType.Or || bexp.NodeType == ExpressionType.OrElse)
                {
                    return new CompositeFilterDescriptor
                    {
                        LogicalOperator = (bexp.NodeType == ExpressionType.And || bexp.NodeType == ExpressionType.AndAlso) ? FilterCompositionLogicalOperator.And : FilterCompositionLogicalOperator.Or,
                        FilterDescriptors = new Infrastructure.Implementation.FilterDescriptorCollection
                        {
                            ProcessExpression(bexp.Left),
                            ProcessExpression(bexp.Right),
                        }
                    };
                }
                else if (exp.NodeType == ExpressionType.Equal)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsEqualTo, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
                else if (exp.NodeType == ExpressionType.NotEqual)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsNotEqualTo, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
                else if (exp.NodeType == ExpressionType.GreaterThan)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsGreaterThan, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
                else if (exp.NodeType == ExpressionType.GreaterThanOrEqual)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsGreaterThanOrEqualTo, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
                else if (exp.NodeType == ExpressionType.LessThan)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsLessThanOrEqualTo, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
                else if (exp.NodeType == ExpressionType.LessThanOrEqual)
                {
                    return new FilterDescriptor { Operator = FilterOperator.IsLessThanOrEqualTo, Member = GetMember(bexp.Left).ToString(), Value = GetValue(bexp.Right) };
                }
            }
            else if (exp is MethodCallExpression)
            {
                var fexp = exp as MethodCallExpression;
                if (fexp.Method.Name.ToLower() == "contains")
                {
                    return new FilterDescriptor { Operator = FilterOperator.Contains, Member = GetMember(fexp.Object).ToString(), Value = GetValue(fexp.Arguments[0]) };
                }
                else if (fexp.Method.Name.ToLower() == "startswith")
                {
                    return new FilterDescriptor { Operator = FilterOperator.StartsWith, Member = GetMember(fexp.Object).ToString(), Value = GetValue(fexp.Arguments[0]) };
                }
                else if (fexp.Method.Name.ToLower() == "endswith")
                {
                    return new FilterDescriptor { Operator = FilterOperator.EndsWith, Member = GetMember(fexp.Object).ToString(), Value = GetValue(fexp.Arguments[0]) };
                }
            }
            else if (exp is UnaryExpression && exp.NodeType == ExpressionType.Not)
            {
                var reverse = new Dictionary<FilterOperator, FilterOperator>()
                {
                    { FilterOperator.IsEqualTo, FilterOperator.IsNotEqualTo },
                    { FilterOperator.IsNotEqualTo, FilterOperator.IsEqualTo },
                    { FilterOperator.IsGreaterThan, FilterOperator.IsLessThanOrEqualTo },
                    { FilterOperator.IsGreaterThanOrEqualTo, FilterOperator.IsLessThan },
                    { FilterOperator.IsLessThan, FilterOperator.IsGreaterThanOrEqualTo },
                    { FilterOperator.IsLessThanOrEqualTo, FilterOperator.IsGreaterThan },
                    { FilterOperator.Contains, FilterOperator.DoesNotContain },
                    { FilterOperator.DoesNotContain, FilterOperator.Contains },
                };
            }

            return result;
        }
        public static object GetMember(Expression exp)
        {
            if (exp is MemberExpression)
            {
                var mexp = exp as MemberExpression;
                return mexp.Member.Name;
            }
            else if (exp is UnaryExpression && (exp as UnaryExpression).Operand is MemberExpression)
            {
                var mexp = (exp as UnaryExpression).Operand as MemberExpression;
                return mexp.Member.Name;
            }
            else if (exp is ConstantExpression)
            {
                var cexp = exp as ConstantExpression;
                return cexp.Value;
            }
            else if (exp is NewExpression)
            {
                return Expression.Lambda(exp).Compile().DynamicInvoke();
            }
            else if (exp is UnaryExpression)
            {
                return Expression.Lambda(exp).Compile().DynamicInvoke();
            }

            return null;
        }
        public static object GetValue(Expression exp)
        {
            if (exp == null)
                return null;
            return Expression.Lambda(exp).Compile().DynamicInvoke();
        }

        public static AjaxDataSourceBuilderBase<TModel, TDataSourceBuilder> Filter<TModel, TDataSourceBuilder>(this AjaxDataSourceBuilderBase<TModel, TDataSourceBuilder> builder, Expression<Func<TModel, bool>> exp)
            where TModel : class
            where TDataSourceBuilder : AjaxDataSourceBuilderBase<TModel, TDataSourceBuilder>
        {
            var filter = ExpressionToKendo(exp);

            var field = builder.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(a => a.Name == "dataSource").FirstOrDefault();

            var dataSource = field.GetValue(builder) as Kendo.Mvc.UI.DataSource;

            dataSource.Filters.Add(filter);

            return builder;
        }

        #region Datasource

        public static DataSource Filter<TModel>(this DataSource dataSource, Expression<Func<TModel, bool>> exp) where TModel : class
        {
            var filter = ExpressionToKendo(exp);

            dataSource.Filters.Add(filter);

            return dataSource;
        }

        public static DataSource Filter<TModel>(this DataSource dataSource, Action<DataSourceFilterDescriptorFactory<TModel>> configurator) where TModel : class
        {
            configurator(new DataSourceFilterDescriptorFactory<TModel>(dataSource.Filters));

            return dataSource;
        }
        public static DataSource Sort(this DataSource dataSource, Action<ReadOnlyCustomDataSourceSortDescriptorFactory> configurator)
        {
            configurator(new ReadOnlyCustomDataSourceSortDescriptorFactory(dataSource.OrderBy));
            return dataSource;
        }

        #endregion

    }

    public static class DropDownListBuilderExtension
    {
        public static DropDownListBuilder Group(this DropDownListBuilder builder, string group)
        {
            builder.ToComponent().DataSource.Groups.Add(new Kendo.Mvc.GroupDescriptor { Member = group, MemberType = typeof(string) });
            return builder;
        }
        public static DropDownListBuilder Action(this DropDownListBuilder builder, Action<CrudOperationBuilder> configurator)
        {
            builder.DataSource(x => x.Read(configurator));
            return builder;
        }
        public static DropDownListBuilder Id(this DropDownListBuilder builder, string Id)
        {
            var temlHtml = new Dictionary<string, object>() { { "id", Id } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static DropDownListBuilder Readonly(this DropDownListBuilder builder, bool param)
        {
            if (!param)
            {
                return builder;
            }
            var temlHtml = new Dictionary<string, object>() { { "readonly", "readonly" } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

        public static DropDownListBuilder Sort(this DropDownListBuilder builder, Action<ReadOnlyCustomDataSourceSortDescriptorFactory> configurator)
        {
            builder.ToComponent().DataSource.Sort(configurator);
            return builder;
        }
        public static DropDownListBuilder Filter<TModel>(this DropDownListBuilder builder, Action<DataSourceFilterDescriptorFactory<TModel>> configurator) where TModel : class
        {
            builder.ToComponent().DataSource.Filter(configurator);
            return builder;
        }
        public static DropDownListBuilder Filter<TModel>(this DropDownListBuilder builder, Expression<Func<TModel, bool>> exp) where TModel : class
        {
            builder.ToComponent().DataSource.Filter(exp);
            return builder;
        }
        public static DropDownListBuilder Events(this DropDownListBuilder builder, Action<DataSourceEventBuilder> configurator)
        {
            builder.DataSource(x => x.Events(configurator));
            return builder;
        }
        public static DropDownListBuilder Execute(this DropDownListBuilder builder, string selectedColumnName = "id")
        {
            var value = ((Kendo.Mvc.UI.DropDownList)builder).Value;
            if (value == null)
            {
                return builder;
            }
            var selectedKeys = string.Format("{0}:{1}", selectedColumnName, value.Replace("-", "_").ToString());
            builder.ToComponent().DataSource.OrderBy.Insert(0, new SortDescriptor(selectedKeys, System.ComponentModel.ListSortDirection.Ascending));
            return builder;
        }


        public static DropDownListBuilder CascadeRequired(this DropDownListBuilder builder, bool CascadeRequired = true)
        {
            builder.Enable(false);
            builder.AutoBind(false);
            var temlHtml = new Dictionary<string, object>() { { "data-cascadeRequired", CascadeRequired.ToString().ToLower() } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

    }

    public static class MultiSelectBuilderExtension
    {
        public static MultiSelectBuilder Group(this MultiSelectBuilder builder, string group)
        {
            builder.ToComponent().DataSource.Groups.Add(new Kendo.Mvc.GroupDescriptor { Member = group, MemberType = typeof(string) });
            return builder;
        }
        public static MultiSelectBuilder Action(this MultiSelectBuilder builder, Action<CrudOperationBuilder> configurator)
        {
            //.Group(g => g.Add("Country", typeof(string)))
            builder.DataSource(x => x.Read(configurator));

            builder.TagTemplate("#=TagTemplate(data,'" + (builder.ToComponent()).DataTextField + "')#");

            return builder;
        }
        public static MultiSelectBuilder Id(this MultiSelectBuilder builder, string Id)
        {
            var temlHtml = new Dictionary<string, object>() { { "id", Id } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

        public static MultiSelectBuilder Readonly(this MultiSelectBuilder builder, bool param)
        {
            if (!param)
            {
                return builder;
            }
            var temlHtml = new Dictionary<string, object>() { { "readonly", "readonly" } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

        public static MultiSelectBuilder Sort(this MultiSelectBuilder builder, Action<ReadOnlyCustomDataSourceSortDescriptorFactory> configurator)
        {
            builder.ToComponent().DataSource.Sort(configurator);
            return builder;
        }
        public static MultiSelectBuilder Filter<TModel>(this MultiSelectBuilder builder, Action<DataSourceFilterDescriptorFactory<TModel>> configurator) where TModel : class
        {
            builder.ToComponent().DataSource.Filter(configurator);
            return builder;
        }
        public static MultiSelectBuilder Filter<TModel>(this MultiSelectBuilder builder, Expression<Func<TModel, bool>> exp) where TModel : class
        {
            builder.ToComponent().DataSource.Filter(exp);
            return builder;
        }
        public static MultiSelectBuilder Events(this MultiSelectBuilder builder, Action<DataSourceEventBuilder> configurator)
        {
            builder.DataSource(x => x.Events(configurator));
            return builder;
        }
        public static MultiSelectBuilder Value(this MultiSelectBuilder builder, object value)
        {

            if (value == null)
            {
                return builder;
            }

            var nValue = value as IEnumerable;
            if (nValue == null)
            {
                var list = new List<object>();
                list.Add(value);
                nValue = list as IEnumerable;
            }
            return builder.Value(nValue);
        }
        public static MultiSelectBuilder Execute(this MultiSelectBuilder builder, string selectedColumnName = "id")
        {
            var value = ((Kendo.Mvc.UI.MultiSelect)builder).Value;
            if (value == null)
            {
                return builder;
            }
            var keys = value.Cast<object>().ToArray().Select(a => a.ToString().Replace("-", "_"));

            if (keys.Count() < 1)
            {
                return builder;
            }

            var selectedKeys = string.Format("{0}:{1}", selectedColumnName, string.Join("','", keys));
            builder.ToComponent().DataSource.OrderBy.Insert(0, new SortDescriptor(selectedKeys, System.ComponentModel.ListSortDirection.Ascending));
            return builder;
        }
        public static MultiSelectBuilder CascadeFrom(this MultiSelectBuilder builder, string cascadeElement)
        {
            builder.Enable(false);
            builder.AutoBind(false);
            var temlHtml = new Dictionary<string, object>() { { "data-cascadeFrom", cascadeElement } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }
        public static MultiSelectBuilder CascadeFromField(this MultiSelectBuilder builder, string cascadeElementField)
        {
            builder.Enable(false);
            builder.AutoBind(false);
            var temlHtml = new Dictionary<string, object>() { { "data-cascadeFromField", cascadeElementField } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

        public static MultiSelectBuilder CascadeRequired(this MultiSelectBuilder builder, bool CascadeRequired = true)
        {
            builder.Enable(false);
            builder.AutoBind(false);
            var temlHtml = new Dictionary<string, object>() { { "data-cascadeRequired", CascadeRequired.ToString().ToLower() } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }

        public static MultiSelectBuilder CascadeToField(this MultiSelectBuilder builder, string cascadeElementField)
        {
            builder.Enable(false);
            builder.AutoBind(false);
            var temlHtml = new Dictionary<string, object>() { { "data-cascadeToField", cascadeElementField } };
            var htmlAttribute = temlHtml.Union(builder.ToComponent().HtmlAttributes).ToDictionary(k => k.Key, v => v.Value);
            builder.HtmlAttributes(htmlAttribute);
            return builder;
        }


    }

}