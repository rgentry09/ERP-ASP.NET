﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GridBlazorClientSide.Shared.Models
{
    [Serializable]
    public class Truck
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual Person Person { get; set; }
        public PersonType? Type { get; set; }
    }

    public enum PersonType
    {
        Driver,
        Owner,
        DriverAndOwner
    }

    public static class PersonTypeExtensions
    {
        public static string ToText(this PersonType me)
        {
            switch (me)
            {
                case PersonType.Driver:
                    return "Driver";
                case PersonType.Owner:
                    return "Owner";
                case PersonType.DriverAndOwner:
                    return "Driver & Owner";
                default:
                    return "";
            }
        }
    }
}
