using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using prep.matching_core;

namespace prep.sorting
{
    public class Sort<T>
    {
        public static IAttributeComparer<T, AttributeType> by<AttributeType>(IGetTheValueOfAnAttribute<T, AttributeType> accessor, SortOrders order = SortOrders.@ascending) where AttributeType : IComparable<AttributeType>
        {
            return new AttributeComparer<T,AttributeType>(order, accessor);
        }

        public static IComparer<T> by<AttributeType>(IGetTheValueOfAnAttribute<T, AttributeType> accessor, IList<AttributeType> predefinedOrder, 
            SortOrders order = SortOrders.@ascending)
        {
            return new AttributeComparerWithPredfinedOrder<T, AttributeType>(order, accessor, predefinedOrder);
        }

    }

    public interface IAttributeComparer<T, AttributeType> : IComparer<T>
    {
        IAttributeComparer<T, AttributeType> then_by(IGetTheValueOfAnAttribute<T, AttributeType> accessor,
            SortOrders order = SortOrders.@ascending);

        IComparer<T> then_by<AttributeType>(IGetTheValueOfAnAttribute<T, AttributeType> accessor,
            IList<AttributeType> predefinedOrder,
            SortOrders order = SortOrders.@ascending);
    }

    public class AttributeComparer<T, AttributeType> : IComparer<T>, IAttributeComparer<T, AttributeType> where AttributeType : IComparable<AttributeType>
    {
        public SortOrders sort_orders { get; set; }
        public IGetTheValueOfAnAttribute<T, AttributeType> accessor { get; set; }

        public AttributeComparer(SortOrders sortOrders, IGetTheValueOfAnAttribute<T, AttributeType> accessor)
        {
            sort_orders = sortOrders;
            this.accessor = accessor;
        }

        public int Compare(T x, T y)
        {
            AttributeType attributeX = accessor(x);
            AttributeType attribute_y = accessor(y);
            return attributeX.CompareTo(attribute_y);
        }

        public IAttributeComparer<T, AttributeType> then_by(IGetTheValueOfAnAttribute<T, AttributeType> accessor, SortOrders order = SortOrders.@ascending)
        {
           return Sort<T>.@by(accessor, order);
        }

        public IComparer<T> then_by<AttributeType1>(IGetTheValueOfAnAttribute<T, AttributeType1> accessor, IList<AttributeType1> predefinedOrder,
            SortOrders order = SortOrders.@ascending)
        {
            return Sort<T>.@by(accessor, predefinedOrder, order);
        }
    }

    public class AttributeComparerWithPredfinedOrder<T, AttributeType> : IComparer<T>
    {
        public SortOrders sort_orders { get; set; }
        public IGetTheValueOfAnAttribute<T, AttributeType> accessor { get; set; }
        public IList<AttributeType> PredefinedOrder { get; set; }

        public AttributeComparerWithPredfinedOrder(SortOrders sortOrders, IGetTheValueOfAnAttribute<T, AttributeType> accessor, IList<AttributeType> predefinedOrder)
        {
            sort_orders = sortOrders;
            this.accessor = accessor;
            PredefinedOrder = predefinedOrder;
        }

        public int Compare(T x, T y)
        {
            AttributeType attributeX = accessor(x);
            AttributeType attribute_y = accessor(y);

            return PredefinedOrder.IndexOf(attributeX).CompareTo(PredefinedOrder.IndexOf(attribute_y));

        }
    }

    

    public enum SortOrders
    {
        ascending = 0,
        descending = 1
    }
}
