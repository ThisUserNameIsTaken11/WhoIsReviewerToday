using System;
using System.Collections.Generic;
using WhoIsReviewerToday.Domain.Calendar;

namespace WhoIsReviewerToday.Infrastructure.Calendar
{
    internal class HolidaysCalendar : IHolidaysCalendar
    {
        public HolidaysCalendar()
        {
            ExcludedDates = new List<DateTime>
            {
                new DateTime(2018, 1, 1),
                new DateTime(2018, 1, 6),
                new DateTime(2018, 3, 11),
                new DateTime(2018, 3, 25),
                new DateTime(2018, 4, 1),
                new DateTime(2018, 4, 26),
                new DateTime(2018, 4, 28),
                new DateTime(2018, 5, 1),
                new DateTime(2018, 6, 17),
                new DateTime(2018, 8, 15),
                new DateTime(2018, 10, 1),
                new DateTime(2018, 10, 28),
                new DateTime(2018, 12, 25),
                new DateTime(2018, 12, 26)
            };
        }

        public IEnumerable<DateTime> ExcludedDates { get; }
    }
}