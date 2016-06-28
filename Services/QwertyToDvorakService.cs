using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DvorakConverter
    {
        public static Dictionary<char, char> KeyMapping = new Dictionary<char, char>()
        {
            {'a','a' },
            {'b','x' },
            {'c','j' },
            {'d','e' },
            {'e','.' },
            {'f','u' },
            {'g','i' },
            {'h','d' },
            {'i','c' },
            {'j','h' },
            {'k','t' },
            {'l','n' },
            {'m','m' },
            {'n','b' },
            {'o','r' },
            {'p','l' },
            {'q','\'' },
            {'r','p' },
            {'s','o' },
            {'t','y' },
            {'u','g' },
            {'v','k' },
            {'w',',' },
            {'x','q' },
            {'y','f' },
            {'z',';' },
            {',','w' },
            {'.','v' },
            {'/','z' },
            {';','s' }
        };

        public static string Convert(string qwertyString)
        {
            var chars = qwertyString.ToCharArray().Select(c =>
             {
                 if (KeyMapping.ContainsKey(c))
                 {
                     return KeyMapping[c];
                 }
                 return c;
             }).ToArray();
            return new string(chars);
        }
    }
}
