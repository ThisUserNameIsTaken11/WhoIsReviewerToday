using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Providers;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Providers
{
    public class BotCommandProviderTests
    {
        public BotCommandProviderTests()
        {
            _commands = new[] { Mock.Of<ICommand>(), Mock.Of<ICommand>() };
        }

        private readonly ICommand[] _commands;

        private BotCommandProvider CreateProvider() => new BotCommandProvider(_commands);

        [Fact]
        public void GetsBotCommands()
        {
            var provider = CreateProvider();

            var actualCommands = provider.GetBotCommands();

            actualCommands.Should().BeSameAs(_commands);
        }
    }
}