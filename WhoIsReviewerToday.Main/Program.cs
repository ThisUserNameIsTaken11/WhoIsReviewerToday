using System;
using Ninject;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Main
{
    internal class Program
    {
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new NinjectBindings());

            return kernel;
        }

        private static void Main(string[] args)
        {
            using (var kernel = CreateKernel())
            {
                var whoIsReviewerTodayBot = kernel.Get<IWhoIsReviewerTodayBot>();
                Console.WriteLine(whoIsReviewerTodayBot.GetGreetings());
                Console.ReadKey();
            }
        }
    }
}