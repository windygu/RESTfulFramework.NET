using System;
using System.Text;

namespace RESTfulFramework.Common
{
    public class RandomEx
    {
        public static string CreateSmsCode()
        {
            char[] constant =
                              {
                             '0','1','2','3','4','5','6','7','8','9'
                             };

            var newRandom = new StringBuilder(10);
            var rd = new Random();
            for (int i = 0; i < 6; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
    }
}
