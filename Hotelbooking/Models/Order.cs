//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotelbooking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public System.Guid Id { get; set; }
        public System.DateTime startdate { get; set; }
        public System.DateTime enddate { get; set; }
        public string totalprice { get; set; }
        public string comments { get; set; }
        public string UserId { get; set; }
        public System.Guid RoomId { get; set; }
    
        public virtual Room Room { get; set; }
    }
}