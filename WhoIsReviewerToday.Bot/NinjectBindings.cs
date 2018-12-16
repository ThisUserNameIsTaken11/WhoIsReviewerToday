using Ninject.Modules;

namespace WhoIsReviewerToday.Bot
{
    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IWhoIsReviewerTodayBot>().To<WhoIsReviewerTodayBot>().InSingletonScope();
        }
    }
}