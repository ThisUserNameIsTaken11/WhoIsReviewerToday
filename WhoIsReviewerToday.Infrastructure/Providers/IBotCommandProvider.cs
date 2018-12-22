using System.Collections.Generic;
using WhoIsReviewerToday.Infrastructure.Commands;

namespace WhoIsReviewerToday.Infrastructure.Providers
{
    public interface IBotCommandProvider
    {
        IEnumerable<ICommand> GetBotCommands();
    }
}