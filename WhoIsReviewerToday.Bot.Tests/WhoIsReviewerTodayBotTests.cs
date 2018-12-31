using System;
using FluentAssertions;
using Xunit;

namespace WhoIsReviewerToday.Bot.Tests
{
    public class WhoIsReviewerTodayBotTests
    {
        private static WhoIsReviewerTodayBot CreateBot()
            => new WhoIsReviewerTodayBot(Environment.GetEnvironmentVariable("APPSETTING_BotToken"));

        [Fact]
        public void GetsGreetings()
        {
            var bot = CreateBot();

            var greetings = bot.GetGreetings();

            greetings.Should().MatchRegex("Hello, World! I am user [0-9]* and my name is [a-zA-Z0-9]*.");
        }
    }
}