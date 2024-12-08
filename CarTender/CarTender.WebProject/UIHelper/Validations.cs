using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public struct Validations
    {
        public static ValidationUI UserName(bool? required = null)
        {
            return new ValidationUI
            {

                required = (required == true),
                pattern = "[a-zA-Z0-9._\\/@-]+",
                data_error = "Yanlızca sayı ve harf girişi gerçekleştirebilirsiniz.. ( 6 - 30 Karakter )",
                minlength = 6,
                maxlength = 30
            };
        }


        public static ValidationUI Text09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9]+",
                data_error = "Lütfen Özel karakter kullanmadan ve birleşik giriş yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Text09Space(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                data_error = "Lütfen Özel karakter kullanmadan giriş yapınız...",
                minlength = minLength,
                maxlength = maxLength

            };
        }

        public static ValidationUI TextTurkce09(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9ÇŞĞÜÖİçşğüâÂöı]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan birleşik giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextEveryone(bool? required = null, Int32? minLength = 0, Int32? maxLength = 50)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ:;\\-/.,&*'?+()’ ]+",
                data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkceSpace09(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-Z0-9 ÇŞĞÜÖİçşğüöıâÂ.]+",
                //data_pattern_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                data_error = "Lütfen Özel karakter kullanmadan giriş gerçekleştirin...",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI TextTurkce(bool? required = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖİâÂçşğüöı]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız.."
            };
        }

        public static ValidationUI TextTurkceSpace(bool? required = null, Int32? minLength = null, Int32? maxLength = null)
        {
            return new ValidationUI
            {
                required = (required == true),
                pattern = "[a-zA-ZÇŞĞÜÖâÂİçşğüöı .]+",
                data_error = "Lütfen Yalnızca Alfabetik Giriş Yapınız..",
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Number(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12)
        {
            return new ValidationUI
            {
                pattern = "[-+]?[0-9]*(?:.|,)?[0-9]+",
                data_error = "Geçerli bir sayı girilmedi.",
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI IP(bool? required = null)
        {
            return new ValidationUI
            {
                pattern = "\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}.\\b\\d{1,3}",
                data_error = "Geçerli bir adres girilmedi.",
                required = (required == true),
                maxlength = 15
            };
        }

        public static ValidationUI NumberOnly(bool? required = null, Int32? minLength = 0, Int32? maxLength = 12)
        {
            return new ValidationUI
            {
                pattern = "[0-9]+",
                data_error = "Geçerli bir sayı girilmedi.",
                required = (required == true),
                minlength = minLength,
                maxlength = maxLength
            };
        }

        public static ValidationUI Range(Int32? Min, Int32? Max, bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                min = Min,
                max = Max,
                required = (required == true),
            };
        }

        public static ValidationUI TelefonNo(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 13,
                minlength = 11,
                pattern = "([+][0-9])?(0)[0-9]{10}",
                data_error = "Telefon Numarası Sayısal Karakterlerden Oluşmalıdır. ( 0 ile beraber )",
                required = (required == true),
            };
        }

        public static ValidationUI VergiNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]+",
                data_error = "Vergi Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }

        public static ValidationUI IhaleNo(bool? required = null)
        {
            return new ValidationUI
            {
                minlength = 10,
                maxlength = 11,
                pattern = "[0-9]*[.][0-9]*[.][0-9]*",
                data_error = "İhale Numarası En Az 10 En Fazla 11 Sayısal Karakterden Oluşmalıdır.",
                required = (required == true),
            };
        }


        public static ValidationUI TCKimlik = new ValidationUI
        {
            maxlength = 11,
            minlength = 11,
            pattern = "[0-9]+",
            data_error = "Kimlik Numarası Sayısal Karakterlerden Oluşmalıdır. ( 11 Karakter )",
            required = true,
        };

        public static ValidationUI Required = new ValidationUI { required = true, };

        public static ValidationUI Yuzde(bool? required = null)
        {
            return new ValidationUI
            {
                type = "number",
                pattern = "(^(?:100|[1-9]?[0-9])$)",
                maxlength = 3,
                data_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                data_pattern_error = "Lütfen 0 ile 100 arasında bir değer giriniz.",
                required = (required == true)
            };
        }
        public static ValidationUI EMail(bool? required = null)
        {
            return new ValidationUI
            {
                type = "email",
                data_error = "Geçerli bir mail adresi girilmedi",
                maxlength = 50,
                minlength = 5,
                required = (required == true)
            };
        }

        public static ValidationUI Password(bool? required = null)
        {
            return new ValidationUI
            {
                autocomplete = "off",
                type = "password",
                maxlength = 14,
                minlength = 6,
                pattern = "[0-9a-zA-Z.,*!-_&]+",
                data_pattern_error = "Geçerli bir şifre girilmedi. ( 6 - 14 Karakter )",
                required = (required == true)
            };
        }

        public static ValidationUI PasswordMatch(string Match, bool? required = null)
        {
            return new ValidationUI
            {
                data_error = "Malesef Şifreler eşleşmiyor...",
                data_pattern_error = "Malesef Şifreler eşleşmiyor...",
                data_match = Match,
                required = (required == true),
                type = Password(required).type,
                pattern = Password(required).pattern,
                maxlength = Password(required).maxlength,
                minlength = Password(required).minlength,
                autocomplete = Password(required).autocomplete,
            };
        }

        public static ValidationUI Adres(bool? required = null)
        {
            return new ValidationUI
            {
                pattern = "[0-9a-zA-âÂZÇŞĞÜÖİçşğüöı \\-/.,;:]+",
                maxlength = 5000,
                minlength = 10,
                required = (required == true),

            };
        }

        public static ValidationUI URL(bool? required = null)
        {
            return new ValidationUI
            {
                maxlength = 255,
                type = "url",
                required = (required == true),
            };
        }

        public class ValidationUI
        {
            public string pattern { get; set; }
            public Int32? maxlength { get; set; }
            public Int32? minlength { get; set; }
            public string type { get; set; }
            public bool? required { get; set; }
            public string data_match { get; set; }
            public string data_error { get; set; }
            public string data_required_error { get { return "Lütfen Bu Alanı Doldurun.."; } }
            public string data_pattern_error { get; set; }
            public string autocomplete { get; set; }    //      on  -  off
            public Int32? min { get; set; }
            public Int32? max { get; set; }
        }


    }
}