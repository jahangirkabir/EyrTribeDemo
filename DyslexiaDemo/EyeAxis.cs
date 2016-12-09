using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DyslexiaDemo
{
    class EyeAxis
    {
        string xAxis, yAxis, time;

        public string Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        public string XAxis
        {
            get
            {
                return xAxis;
            }

            set
            {
                xAxis = value;
            }
        }

        public string YAxis
        {
            get
            {
                return yAxis;
            }

            set
            {
                yAxis = value;
            }
        }

        //public override string ToString()
        //{
        //    return xAxis+","+yAxis;
        //}

        public override string ToString()
        {
            return "EyeAxis [xAxis=" + xAxis + ", yAxis=" + yAxis + ", time=" + time + "]";
        }
    }
}
