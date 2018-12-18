using FluentAssertions;
using Xunit;

namespace WhoIsReviewerToday.Bot.Tests
{
    public class WhoIsReviewerTodayBotTests
    {
        private static WhoIsReviewerTodayBot CreateBot() => new WhoIsReviewerTodayBot("736147187:AAF1RQE7pyojK_DLym_9ckt_IiM16US_V8g");

        [Fact]
        public void GetsGreetings()
        {
            var bot = CreateBot();

            var greetings = bot.GetGreetings();

            greetings.Should().Be("Hello, World! I am user 736147187 and my name is TestBot.");
        }
    }
}