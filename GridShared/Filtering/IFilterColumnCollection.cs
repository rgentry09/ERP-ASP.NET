﻿using GridShared.Columns;
using System.Collections.Generic;
using System.Linq;

namespace GridShared.Filtering
{
    /// <summary>
    ///     Collection of current filter options for the grid
    /// </summary>
    public interface IFilterColumnCollection : IEnumerable<ColumnFilterValue>
    {
        /// <summary>
        ///     Get column filter options by given grid column
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        IEnumerable<ColumnFilterValue> GetByColumn(IGridColumn column);
    }

    public class DefaultFilterColumnCollection : List<ColumnFilterValue>, IFilterColumnCollection
    {
        public DefaultFilterColumnCollection() : base()
        {
        }

        public DefaultFilterColumnCollection(string name, GridFilterType type, string value) : this()
        {
            Add(new ColumnFilterValue(name, type, value));
        }

        public void Add(string name, GridFilterType type, string value)
        {
            Add(new ColumnFilterValue(name, type, value));
        }

        public IEnumerable<ColumnFilterValue> GetByColumn(IGridColumn column)
        {
            return this.Where(c => c.ColumnName.ToUpper() == column.Name?.ToUpper());
        }
    }
}