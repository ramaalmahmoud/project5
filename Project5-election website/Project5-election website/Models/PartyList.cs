//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project5_election_website.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartyList
    {
        public int ID { get; set; }
        public string Party_Name { get; set; }
        public string List_Name { get; set; }
        public int Delegate_ID { get; set; }
        public string Delegate_Name { get; set; }
        public string Delegate_Phone { get; set; }
        public string Delegate_Email { get; set; }
        public int Counter { get; set; }
        public string Status { get; set; }
        public string reason { get; set; }
        public string List_Logo { get; set; }
    }
}
