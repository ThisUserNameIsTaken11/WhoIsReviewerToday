using System.Collections.Generic;
using WhoIsReviewerToday.Bot.Commands;

namespace WhoIsReviewerToday.Bot.Providers
{
    internal interface ICommandProvider
    {
        IEnumerable<ICommand> Commands { get; }
    }
}