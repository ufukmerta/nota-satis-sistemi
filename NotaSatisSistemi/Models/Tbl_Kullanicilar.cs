//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NotaSatisSistemi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Kullanicilar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Kullanicilar()
        {
            this.Tbl_Satislar = new HashSet<Tbl_Satislar>();
        }
    
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
        public bool Yetkili { get; set; }
        public decimal Kredi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Satislar> Tbl_Satislar { get; set; }
    }
}
