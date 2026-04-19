using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Settings
{
    public class JwtSetting
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpiryInDays { get; set; } = 7;
    }
}
