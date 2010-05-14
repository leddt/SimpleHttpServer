using System;
using System.IO;
using System.Web;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;

namespace DDT.SimpleHttpServer.Actions.NVelocity
{
    public class NVelocityHelper
    {
        private static readonly VelocityEngine Engine;
        static NVelocityHelper()
        {
            Engine = new VelocityEngine();
            Engine.Init();
        }

        private readonly IHttpContext context;
        private readonly TextWriter output;

        public NVelocityHelper(IHttpContext context, TextWriter output)
        {
            this.context = context;
            this.output = output;
        }

        public void Render(string viewFileName)
        {
            Render(viewFileName, null);
        }

        public void Render(string viewFileName, object model)
        {
            var velocityContext = CreateVelocityContext(model);

            RenderInternal(viewFileName, velocityContext);
        }

        public void RenderLayout(string layoutFileName, string viewFileName, object model)
        {
            var content = new StringWriter();
            var temp = new NVelocityHelper(context, content);
            temp.Render(viewFileName, model);

            var velocityContext = CreateVelocityContext(model);
            velocityContext.Put("content", content.ToString());

            RenderInternal(layoutFileName, velocityContext);
        }

        private void RenderInternal(string viewFileName, IContext velocityContext)
        {
            using (var template = new StreamReader(context.ServerInfo.FileStorage.GetFile(viewFileName)))
            {
                Engine.Evaluate(velocityContext, output, null, template);
            }
        }

        private IContext CreateVelocityContext(object model)
        {
            var velocityContext = new VelocityContext();

            if (model != null)
                velocityContext.Put("model", model);

            velocityContext.Put("vel", this);
            velocityContext.Put("html", new HtmlHelper());

            return velocityContext;
        }
    }
}
