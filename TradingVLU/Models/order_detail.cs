//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingVLU.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_detail
    {
        public int orderID { get; set; }
        public int item_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> totalprice { get; set; }
    
        public virtual item item { get; set; }
        public virtual Order Order { get; set; }
    }
}