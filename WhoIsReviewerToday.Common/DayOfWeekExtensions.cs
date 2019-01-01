using System;
using System.Linq;

namespace WhoIsReviewerToday.Common
{
    public static class DayOfWeekExtensions
    {
        public static bool InRange(this DayOfWeek dayOfWeek, params DayOfWeek[] dayOfWeeks) => dayOfWeeks.Contains(dayOfWeek);
    }
}