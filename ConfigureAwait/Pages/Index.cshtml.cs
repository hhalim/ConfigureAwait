using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConfigureAwait.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public string Result { get; set; }

        readonly Stopwatch sw = new Stopwatch();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
            this.sw.Reset();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.Result = await RunAsyncTwice1() + "\n" +
                          await RunAsyncTwice2() + "\n" +
                          await RunAsyncTwice3() + "\n" +
                          await RunAsyncTwice4();

            return Page();
        }

        //Slow async
        protected async Task<string> RunAsyncTwice1()
        {
            sw.Restart();
            var result1 = await RunAsync(1);
            var result2 = await RunAsync(2);
            var finalResult = result1 + "/n" + result2;
            sw.Stop();

            return "RunAsyncTwice1: " + sw.Elapsed;
        }

        //Better performance async
        protected async Task<string> RunAsyncTwice2()
        {
            sw.Restart();
            var result1 = RunAsync(1);
            var result2 = RunAsync(2);
            var finalResult = await result1 + "/n" + await result2;
            sw.Stop();
            return "RunAsyncTwice2: " + sw.Elapsed;
        }

        //Slow async
        protected async Task<string> RunAsyncTwice3()
        {
            sw.Restart();
            var finalResult = await RunAsync(1) + "/n" + await RunAsync(2);
            sw.Stop();

            return "RunAsyncTwice3: " + sw.Elapsed;
        }

        //Better performance async
        protected async Task<string> RunAsyncTwice4()
        {
            sw.Restart();
            var finalResult = await Task.WhenAll(RunAsync(3), RunAsync(4));
            sw.Stop();
            return "RunAsyncTwice4: " + sw.Elapsed;
        }



        public async Task<string> RunAsync(int id)
        {
            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(100).ConfigureAwait(false); //Perform async task, don't run task in UI SynchronousContext
            }

            return $"Done RunAsync {id}";
        }
    }

}
