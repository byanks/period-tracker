using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeriodTracker
{
    class Period
    {
        public DateTime previousDate
        { get; set; }

        public DateTime newDate
        { get; set; }

        public DateTime lastDate
        { get; set; }

        public string Notes
        { get; set; }

        public string CalculateDays()
        {
            TimeSpan t = newDate - previousDate;
            double dDays = t.TotalDays;
            int days = Convert.ToInt32(dDays);
            string totalDays = days.ToString();
            return totalDays;
        }

        public string CalculateDuration()
        {
            TimeSpan d = lastDate - newDate;
            double dDuration = d.TotalDays;
            int duration = Convert.ToInt32(dDuration) + 1;
            string totalDuration = duration.ToString();
            return totalDuration;
        }
    }
}
