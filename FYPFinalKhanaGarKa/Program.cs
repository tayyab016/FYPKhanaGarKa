using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TinifyAPI;

namespace FYPFinalKhanaGarKa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Tinify.Key = "gGHvuLZcT4lLd-55-kcnGazWM_2LstpA";
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
