//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GridBlazorGrpc.Shared.Models
{
    [DataContract]
    public partial class Shipper
    {
        public Shipper()
        {
            this.Orders = new HashSet<Order>();
        }
        [Key]
        [DataMember(Order = 1)]
        public int ShipperID { get; set; }
        [DataMember(Order = 2)]
        public string CompanyName { get; set; }
        [DataMember(Order = 3)]
        public string Phone { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
