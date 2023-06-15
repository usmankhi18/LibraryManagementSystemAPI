namespace Common.Utils
{
    public class Regexx
    {
        public const string Alphabet = @"[A-Za-z]+$";
        public const string AlphabetWithSpaces = @"[A-Za-z ]+$";
        public const string AlphaNumericSpaces = @"[0-9A-Za-z ]+$";
        public const string AlphaNumeric = @"[0-9A-Za-z]+$";
        public const string AlphaNumericDash = @"[0-9A-Za-z-]+$";
        public const string AlphaNumericDashSpaces = @"[0-9A-Za-z- ]+$";
        public const string AlphaNumericDashHyphen = @"[A-Za-z0-9-_]+$";
        public const string AlphaNumericHyphenSlash = @"[A-Za-z0-9-_/]+$";
        public const string MobileNumber = @"^\+?\d{0,2}\-?\d{4,5}\-?\d{4,8}";
        public const string Numeric = @"[0-9]+$";
        public const string NumberOnly = @"^\d+$";
        public const string EmailAddress = @"^(([^<>()[\]\\.,;:\s@\""]+" + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@" + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                   + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+" + @"[a-zA-Z]{2,}))$";
        public const string Amount = "^[0-9]*(?:\\.[0-9]{0,3})?$";
        public const string Url = @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$";
        public const string LocationCoordinates = @"^\d+(.\d+){0,1}$";
    }
}
