using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Class1
    {
        public void DictTest()
        {
            List<Dictionary<string, int>> dict = new List<Dictionary<string, int>>();

            dict.Add(new Dictionary<string, int> { { "2021-May", 10 } });
            dict.Add(new Dictionary<string, int> { { "2020-Jun", 10 } });
            dict.Add(new Dictionary<string, int> { { "2020-Apr", 10 } });

            var item = dict.Where(i => i.Keys.Any(i => (i.Contains("2020") && i.Contains("May")))).ToList();
        }
    }
}
