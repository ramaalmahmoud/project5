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
    
    public partial class ElectionAdvertisement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ElectionAdvertisement()
        {
            this.PayPalPayments = new HashSet<PayPalPayment>();
        }
    
        public int ID { get; set; }
        public long National_ID { get; set; }
        public string Name { get; set; }
        public string List_Or_Party_Name { get; set; }
        public Nullable<int> Circle_ID { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Status { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayPalPayment> PayPalPayments { get; set; }
    }
}
