//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domein
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.Ganres = new HashSet<Ganre>();
            this.AspNetUsers = new HashSet<AspNetUser>();
        }

        public int BookID { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        public int AuthorID { get; set; }
        public int CountryID { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Description { get; set; }
        public Nullable<int> PagesCount { get; set; }
        public byte[] Picture { get; set; }

        public virtual Author Author { get; set; }
        public virtual Country Country { get; set; }
        public virtual ExtraAttribute ExtraAttribute { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ganre> Ganres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public object Convert { get; set; }
    }    
}