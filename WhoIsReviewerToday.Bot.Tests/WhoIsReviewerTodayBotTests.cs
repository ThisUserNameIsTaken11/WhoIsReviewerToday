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

            greetings.Should().Be("Hello, World! I am user 710490980 and my name is WhoIsReviewerToday.");
        }
    }
}