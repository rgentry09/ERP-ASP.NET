﻿using System.Net;
using System.Runtime.Serialization;

namespace GridShared.Filtering
{
    /// <summary>
    ///     Structure that specifies filter settings for each column
    /// </summary>
    [DataContract]
    public struct ColumnFilterValue
    {
        [DataMember(Name = "ColumnName")]
        public string ColumnName;

        [DataMember(Name = "FilterType")] 
        public GridFilterType FilterType;

        public string FilterValue;

        [DataMember(Name = "FilterValue")]
        internal string FilterValueEncoded
        {
            get { return WebUtility.UrlEncode(FilterValue); }
            set { FilterValue = value; }
        }

        public static ColumnFilterValue Null
        {
            get { return default(ColumnFilterValue); }
        }

        public static bool operator ==(ColumnFilterValue a, ColumnFilterValue b)
        {
            return a.ColumnName == b.ColumnName && a.FilterType == b.FilterType && a.FilterValue == b.FilterValue;
        }

        public static bool operator !=(ColumnFilterValue a, ColumnFilterValue b)
        {
            return a.ColumnName != b.ColumnName || a.FilterType != b.FilterType || a.FilterValue != b.FilterValue;
        }
    }
}