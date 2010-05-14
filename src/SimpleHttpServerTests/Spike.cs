using System;
using System.IO;
using System.Threading;
using DDT.SimpleHttpServer;
using DDT.SimpleHttpServer.Actions;
using DDT.SimpleHttpServer.Actions.NVelocity;
using DDT.SimpleHttpServer.Routing;
using DDT.SimpleHttpServer.Storage;
using Xunit;
using Action=DDT.SimpleHttpServer.Actions.Action;

namespace DDT.SimpleHttpServerTests
{
    public class Spike
    {
        public class IndexView : Action
        {
            public override IActionResult Get(RouteContext context)
            {
                return new NVelocityActionResult("master.nv", "index.nv", new
                                                                          {
                                                                              Time = DateTime.Now, 
                                                                              Data = context.HttpContext.Session["data"] ?? "",
                                                                              context.HttpContext.SessionId
                                                                          });
            }

            public override IActionResult Post(RouteContext context)
            {
                var name = context.HttpContext.Request.PostData["name"];
                return new NVelocityActionResult("hello.nv", name);
            }
        }

        public class SetSessionAction : Action
        {
            public override IActionResult Post(RouteContext context)
            {
                var data = context.HttpContext.Request.PostData["data"];
                context.HttpContext.Session["data"] = data;

                return new RedirectResult("/Index");
            }
        }

        [Fact(Skip="Not a test")]
        public void TestName()
        {
            var logfile = new FileInfo("log-file.txt");
            if (logfile.Exists)
                logfile.Delete();

            log4net.Config.XmlConfigurator.Configure();

            using (var server = new Server(new EmbeddedFileStorage(GetType().Assembly, "DDT.SimpleHttpServerTests.SpikeFiles"), 1234))
            {
                server.Route(@".*\.nv").To<FileNotFound>();
                server.Route("index").To<IndexView>();
                server.Route("ss").To<SetSessionAction>();
                
                server.DefaultPath = "index";

                server.Start();

                while(true)
                    Thread.Sleep(100);
            }
        }
    }
}