//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Roadway_History.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public int ID { get; set; }
        public Nullable<int> Statewide_ID { get; set; }
        public string Doc_Type { get; set; }
        public Nullable<System.DateTime> Order_Date { get; set; }
        public string Doc_Location { get; set; }
        public string File_Contents { get; set; }
        public string Comment { get; set; }
        public string Source { get; set; }
    }
}
