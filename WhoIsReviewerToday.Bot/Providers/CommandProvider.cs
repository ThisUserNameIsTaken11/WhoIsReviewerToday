using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Bot.Commands;

namespace WhoIsReviewerToday.Bot.Providers
{
    internal class CommandProvider : ICommandProvider
    {
        public CommandProvider(IEnumerable<ICommand> commands)
        {
            Commands = commands.ToArray();
        }

        public IEnumerable<ICommand> Commands { get; }
    }
}