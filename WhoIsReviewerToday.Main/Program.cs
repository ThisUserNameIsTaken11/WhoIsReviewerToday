using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Modules;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Main
{
    internal class Program
    {
        private static IEnumerable<INinjectModule> GetNinjectModules()
        {
            yield return new NinjectBindings();
        }

        private static IKernel CreateKernel()
        {
            var ninjectModules = GetNinjectModules().ToArray();
            var kernel = new StandardKernel(ninjectModules);

            return kernel;
        }

        private static void Main(string[] args)
        {
            using (var kernel = CreateKernel())
            {
                var whoIsReviewerTodayService = kernel.Get<IWhoIsReviewerTodayService>();
                whoIsReviewerTodayService.Start();
                Console.ReadKey();
                whoIsReviewerTodayService.Stop();
            }
        }
    }
}