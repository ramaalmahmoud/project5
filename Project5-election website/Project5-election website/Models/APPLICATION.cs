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
    
    public partial class APPLICATION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public APPLICATION()
        {
            this.PayPalPayments_APPLICATIONS = new HashSet<PayPalPayments_APPLICATIONS>();
        }
    
        public int ID { get; set; }
        public int LocalList_ID { get; set; }
        public int PartyList_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PayPalPayments_APPLICATIONS> PayPalPayments_APPLICATIONS { get; set; }
    }
}
