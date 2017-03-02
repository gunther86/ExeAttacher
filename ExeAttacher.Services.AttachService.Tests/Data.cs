using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExeAttacher.Services.AttachService.Tests
{
    public static class Data
    {
        public static void FillWithRandomData(this byte[] array)
        {
            Random r = new Random();
            r.NextBytes(array);
        }
    }
}
