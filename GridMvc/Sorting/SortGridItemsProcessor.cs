﻿using GridShared.Columns;
using GridShared.Sorting;
using System;
using System.Linq;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Settings grid items, based on current sorting settings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SortGridItemsProcessor<T> : IGridItemsProcessor<T>
    {
        private readonly ISGrid _grid;
        private IGridSortSettings _settings;

        public SortGridItemsProcessor(ISGrid grid, IGridSortSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _grid = grid;
            _settings = settings;
        }

        public void UpdateSettings(IGridSortSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            _settings = settings;
        }

        #region IGridItemsProcessor<T> Members

        public IQueryable<T> Process(IQueryable<T> items)
        {
            if (_settings.SortValues?.Count() > 0)
            {
                var sortedColumns = _settings.SortValues.OrderBy(r => r.Id).ToList();

                var gridColumn = _grid.Columns.FirstOrDefault(c => c.Name == sortedColumns[0].ColumnName) as IGridColumn<T>;
                if(gridColumn == null)
                    return items;
                items = gridColumn.Orderers.FirstOrDefault().ApplyOrder(items, sortedColumns[0].Direction);

                if(sortedColumns.Count() > 1)
                {
                    for(int i = 1; i < sortedColumns.Count(); i++)
                    {
                        gridColumn = _grid.Columns.FirstOrDefault(r => r.Name == sortedColumns[i].ColumnName) as IGridColumn<T>;
                        items = gridColumn.Orderers.FirstOrDefault().ApplyThenBy(items, sortedColumns[i].Direction);
                    }
                }

                if (string.IsNullOrEmpty(_settings.ColumnName))
                    return items;
                //determine gridColumn sortable:
                gridColumn = _grid.Columns.FirstOrDefault(c => c.Name == _settings.ColumnName) as IGridColumn<T>;
                if (gridColumn == null || !gridColumn.SortEnabled)
                    return items;
                foreach (var columnOrderer in gridColumn.Orderers)
                {
                    items = columnOrderer.ApplyThenBy(items, _settings.Direction);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_settings.ColumnName))
                    return items;
                //determine gridColumn sortable:
                var gridColumn = _grid.Columns.FirstOrDefault(c => c.Name == _settings.ColumnName) as IGridColumn<T>;
                if (gridColumn == null || !gridColumn.SortEnabled)
                    return items;
                foreach (var columnOrderer in gridColumn.Orderers)
                {
                    items = columnOrderer.ApplyOrder(items, _settings.Direction);
                }
            }
            return items;
        }

        #endregion
    }
}