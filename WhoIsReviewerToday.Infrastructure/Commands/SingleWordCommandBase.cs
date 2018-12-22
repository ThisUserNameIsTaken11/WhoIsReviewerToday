namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public abstract class SingleWordCommandBase : CommandBase
    {
        public override bool Matches(string commandName) => commandName.Contains(Code);
    }
}