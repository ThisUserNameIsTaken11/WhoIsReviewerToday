using System;
using System.Collections.Generic;

namespace WhoIsReviewerToday.Domain.Calendar
{
    public interface IHolidaysCalendar
    {
        IEnumerable<DateTime> ExcludedDates { get; }
    }
}