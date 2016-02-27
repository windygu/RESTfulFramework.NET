using System.Text;

namespace RESTfulFramework.NET.Common
{
    public static class Random
    {
        public static string CreateSmsCode()
        {
            char[] constant =
                              {
                             '0','1','2','3','4','5','6','7','8','9'
                             };

            var newRandom = new StringBuilder(10);
            var rd = new System.Random();
            for (int i = 0; i < 6; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
    }
}
