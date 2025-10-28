using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.Services
{
    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            var d = new string(cpf.Where(char.IsDigit).ToArray());
            if (d.Length != 11) return false;
            if (new string(d[0], 11) == d) return false;

            // dígitos
            return Check(d, 9) && Check(d, 10);
        }

        private static bool Check(string d, int length)
        {
            int sum = 0, weight = length + 1;
            for (int i = 0; i < length; i++) sum += (d[i] - '0') * (weight--);
            int r = (sum * 10) % 11;
            if (r == 10) r = 0;
            return r == (d[length] - '0');
        }
    }
}
