﻿using System.Collections.Generic;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IDeveloperRepository
    {
        IEnumerable<Developer> Items { get; }
    }
}