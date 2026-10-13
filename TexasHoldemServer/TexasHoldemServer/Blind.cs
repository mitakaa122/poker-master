using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.main.model
{
    public class Blind
    {
        private long lower;
        private long upper;
        private long total;

        public void upBlinds(long lower, long upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public long Total()
        {
            return total;
        }

        public long getLower()
        {
            return lower;
        }

        public long getUpper()
        {
            return upper;
        }

        public long getBank()
        {
            long win = total;
            total = 0;
            return win;
        }

        public void bet(long value)
        {
            total += value;
            /*
            //check
            if (value >= upper)
            {
                total += value;
            }
            */
        }
    }
}
