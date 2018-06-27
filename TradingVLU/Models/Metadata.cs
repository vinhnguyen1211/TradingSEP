using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TradingVLU.Models
{
    public class USERMetadata
    {
        
        [Required(ErrorMessage = "Fullname must not be null")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [RegularExpression("[a-zA-Z][a-zA-Z0-9]{5,15}", ErrorMessage = "First character must be a character, special character is not allowed. Length 6-16 characters")]
        [StringLength(100, MinimumLength = 6,ErrorMessage = "The Username must be at least 6 characters.")]
        public string username { get; set; }

        [Required(ErrorMessage = "Password must not be null")]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{4,20}$", ErrorMessage = "Minimum four characters and maximun twenty characters, at least one uppercase letter, one lowercase letter and one number.")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Required(ErrorMessage = "Password must not be null")]
        [Compare("password", ErrorMessage = "Password Mismatched. Re-enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [NotMapped]
        public string confirmpassword { get; set; }

        [Required(ErrorMessage = "Email must not be null")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [RegularExpression("^([\\w-\\.]+@(?!(g|G)(m|M)(a|A)(i|I)(l|L))(?!(y|Y)(a|A)(h|H)(o|O)((o|O)))(?!(h|H)(o|O)(t|T)(m|M)(a|A)(i|I)(l|L))(?!(o|O)(u|U)(t|T)(l|L)(o|O)(o|O)(k|K))(?!(r|R)(e|E)(d|D)(i|I)(f|F)(f|F)(m|M)(a|A)(i|I)(l|L))([\\w-]+\\.)+[\\w-]{2,6})?$", ErrorMessage = "It must contains only @vanlanguni.vn or @vanlanguni.edu.vn and not just all number")]
        public string email { get; set; }

        [Required(ErrorMessage = "Name must not be null")]
        [Display(Name = "Name")]
        [RegularExpression("^([a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+\\s)*[a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "It must contains only characters and one space between each one")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The Name must be at least 6 characters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Please select a value")]
        [Range(1, 13)]
        [Display(Name = "Security Question")]
        public int id_security_question { get; set; }

        [Required(ErrorMessage = "Answer must not be null")]
        [Display(Name = "Answer")]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9]{5,15}", ErrorMessage = "The Answer is not just number")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The Answer must be at least 6 characters.")]
        public string answer_security_question { get; set; }

        
    }
}