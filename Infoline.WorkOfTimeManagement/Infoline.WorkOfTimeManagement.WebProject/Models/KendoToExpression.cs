using Infoline.Framework.Database;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Linq;

namespace Kendo.Mvc
{
    public class KendoToExpression
    {



        public static SimpleQuery Convert(DataSourceRequest request)
        {
            var query = new SimpleQuery();
            if (request.Filters != null && request.Filters.Count > 0)
            {
                UpdateFilterDescriptor(request.Filters, null, null);
                query.Filter = ParseFilter(request.Filters[0]);
            }


            var defaultSortColumnExpression = "(case when {1} IN('{0}')  then 1 else 2 end)";

            if (request.Sorts != null)
            {
                var sort = request.Sorts.Select(a => new SortDescriptor(!a.Member.Contains(":") ? a.Member : string.Format(defaultSortColumnExpression, a.Member.Split(':')[1].Replace('_', '-'), a.Member.Split(':')[0]), a.SortDirection)).ToArray();

                query.Sort = OrderColumns(sort);
            }

            if (query.Sort == null) query.Sort = new IQueryOrderItem[] { new DESC { Value = new COL("created") } };

            request.PageSize = request.PageSize == 0 ? 20 : request.PageSize;


            query.Take = request.PageSize;
            query.Skip = (request.Page - 1) * request.PageSize;
            return query;
        }



        public static void UpdateFilterDescriptor(IList<IFilterDescriptor> filters, string column, object value)
        {
            foreach (var item in filters)
            {
                if (item is CompositeFilterDescriptor)
                {
                    UpdateFilterDescriptor((CompositeFilterDescriptor)item, column, value);
                }
                else
                {
                    UpdateFilterDescriptor((FilterDescriptor)item, column, value);
                }
            }
        }

        private static void UpdateFilterDescriptor(CompositeFilterDescriptor filter, string column, object value)
        {
            foreach (var item in filter.FilterDescriptors)
            {
                if (item is CompositeFilterDescriptor)
                {
                    UpdateFilterDescriptor((CompositeFilterDescriptor)item, column, value);
                }
                else
                {
                    UpdateFilterDescriptor((FilterDescriptor)item, column, value);
                }
            }
        }

        private static void UpdateFilterDescriptor(FilterDescriptor filter, string column, object value)
        {

            if (filter.Value != null)
            {
                if (filter.Value.ToString() == System.UIHelper.Guid.Null.ToString())
                {
                    filter.Value = null;
                }
                else if (filter.Value.ToString() == System.UIHelper.String.Null.ToString())
                {
                    filter.Value = null;
                }
                else if (filter.Value.ToString() == System.UIHelper.Datetime.Null.ToString())
                {
                    filter.Value = null;
                }
                else if (filter.Value.ToString() == System.UIHelper.Int32.Null.ToString())
                {
                    filter.Value = null;
                }
            }

            if (filter.Member == column)
            {
                filter.Value = value;
            }
        }



        private static IQueryOrderItem[] OrderColumns(IList<SortDescriptor> sort)
        {
            var result = new List<IQueryOrderItem>();
            if (sort == null || sort.Count == 0)
                return null;
            foreach (var order in sort)
                if (order.SortDirection == System.ComponentModel.ListSortDirection.Ascending)
                    result.Add(new ASC { Value = new COL(order.Member) });
                else result.Add(new DESC { Value = new COL(order.Member) });


            return result.ToArray();
        }
        private static BEXP ParseFilter(IFilterDescriptor filter)
        {
            if (filter is CompositeFilterDescriptor)
                return ParseCompositeFilterDescriptor(filter as CompositeFilterDescriptor);
            else if (filter is FilterDescriptor)
                return ParseFilterDescriptor(filter as FilterDescriptor);
            return null;
        }
        private static BEXP ParseCompositeFilterDescriptor(CompositeFilterDescriptor filter)
        {
            var left = ParseFilter(filter.FilterDescriptors[0]);
            var right = ParseFilter(filter.FilterDescriptors[1]);
            switch (filter.LogicalOperator)
            {
                case FilterCompositionLogicalOperator.And:
                    return new BEXP { Operand1 = left, Operand2 = right, Operator = BinaryOperator.And };
                case FilterCompositionLogicalOperator.Or:
                    return new BEXP { Operand1 = left, Operand2 = right, Operator = BinaryOperator.Or };
                default:
                    return null;
            }
        }
        private static BEXP ParseFilterDescriptor(FilterDescriptor filter)
        {
            var left = new COL(filter.Member);
            var right = new VAL { Value = filter.Value };
            switch (filter.Operator)
            {
                case FilterOperator.IsLessThan:
                    return new BEXP { Operator = BinaryOperator.LessThan, Operand1 = left, Operand2 = right };
                case FilterOperator.IsLessThanOrEqualTo:
                    return new BEXP { Operator = BinaryOperator.LessThanOrEqual, Operand1 = left, Operand2 = right };
                case FilterOperator.IsEqualTo:
                    return new BEXP { Operator = BinaryOperator.Equal, Operand1 = left, Operand2 = right };
                case FilterOperator.IsNotEqualTo:
                    return new BEXP { Operator = BinaryOperator.NotEqual, Operand1 = left, Operand2 = right };
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return new BEXP { Operator = BinaryOperator.GreaterThanOrEqual, Operand1 = left, Operand2 = right };
                case FilterOperator.IsGreaterThan:
                    return new BEXP { Operator = BinaryOperator.GreaterThan, Operand1 = left, Operand2 = right };
                case FilterOperator.StartsWith:
                    return new BEXP
                    {
                        Operator = BinaryOperator.Like,
                        Operand1 = left,
                        Operand2 = new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { right, (VAL)"%" } }
                    };
                case FilterOperator.EndsWith:
                    return new BEXP
                    {
                        Operator = BinaryOperator.Like,
                        Operand1 = left,
                        Operand2 = new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { (VAL)"%", right } }
                    };
                case FilterOperator.Contains:
                    return new BEXP
                    {
                        Operator = BinaryOperator.Like,
                        Operand1 = left,
                        Operand2 = new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { (VAL)"%", right, (VAL)"%" } }
                    };
                case FilterOperator.IsContainedIn:
                    return new BEXP
                    {
                        Operator = BinaryOperator.Like,
                        Operand1 = right,
                        Operand2 = new FEXP { Function = QueryFunctions.Concat, Parameters = new IQueryValue[] { (VAL)"%", left, (VAL)"%" } }
                    };
                case FilterOperator.DoesNotContain:
                    return
                        new BEXP
                        {
                            Operator = BinaryOperator.Not,
                            Operand1 = new BEXP
                            {
                                Operator = BinaryOperator.Like,
                                Operand1 = left,
                                Operand2 = new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { (VAL)"%", right, (VAL)"%" } }
                            }
                        };
                default:
                    break;
            }
            return null;
        }


    }
}
