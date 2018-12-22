using System.Collections.Generic;
using WhoIsReviewerToday.Infrastructure.Commands;

namespace WhoIsReviewerToday.Infrastructure.Providers
{
    internal class BotCommandProvider : IBotCommandProvider
    {
        private readonly IEnumerable<ICommand> _commands;

        public BotCommandProvider(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public IEnumerable<ICommand> GetBotCommands() => _commands;
    }
}