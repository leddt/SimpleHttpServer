using System;
using System.IO;
using NVelocity.App;

namespace DDT.SimpleHttpServer.Actions.NVelocity
{
    public class NVelocityActionResult : IActionResult
    {
        private readonly string layoutFileName;
        private static readonly VelocityEngine Engine;
        static NVelocityActionResult()
        {
            Engine = new VelocityEngine();
            Engine.Init();
        }

        private readonly string viewFileName;
        private readonly object model;

        public NVelocityActionResult(string layoutFileName, string viewFileName, object model)
        {
            this.layoutFileName = layoutFileName;
            this.viewFileName = viewFileName;
            this.model = model;
        }

        public NVelocityActionResult(string viewFileName, object model)
        {
            this.viewFileName = viewFileName;
            this.model = model;
        }

        public void Execute(IHttpContext context)
        {
            context.Response.ContentType = "text/html";
            using (var output = new StreamWriter(context.Response.OutputStream))
            {
                var helper = new NVelocityHelper(context, output);

                if (!String.IsNullOrEmpty(layoutFileName))
                    helper.RenderLayout(layoutFileName, viewFileName, model);
                else
                    helper.Render(viewFileName, model);
            }
        }
    }
}