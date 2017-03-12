//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentListASP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    public partial class Student
    {
        [Key]
        public int IDStudent { get; set; }
        [Required(ErrorMessage = "Imie jest wymagane!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage="Nazwisko wymagane!")]
        public string LastName { get; set; }
        [Required(ErrorMessage =  "Numer indeksu jest Wymagany!")]
        [Index(IsUnique=true)]
        [RegularExpression("[0-9]{6}?", ErrorMessage="Numer niepoprawny!")]
        public string IndexNo { get; set; }
        
        public string BirthPlace { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime BirthDate { get; set; }

        public int IDGroup { get; set; }
        
        [Timestamp]
        public byte[] TimeStamp { get; set; }
        [ForeignKey("IDGroup")]
        public virtual Group Group { get; set; }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            st.Append(FirstName + " ").Append(LastName + " ");
            st.Append(Group.Name + " ").Append(BirthPlace + " ");
            return st.Append(IndexNo).ToString();
        }
    }
}
