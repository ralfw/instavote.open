using System;
using System.Configuration;
using System.Diagnostics;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using instavote.dialogs;

namespace instavote.integration.tests
{
    public class Bootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;

            if (ConfigurationManager.AppSettings["Environment"] == "Debug")
            {
                Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat("bin/", viewName));
            }
        }
    }
}