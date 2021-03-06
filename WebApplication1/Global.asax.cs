﻿namespace WebApplication1
{
    using System;
    using Castle.MonoRail;
    using Castle.MonoRail.Routing;
    using Filters;

    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Router.Instance.Match("(/:controller(/:action(/:id)))", "default", 
                                  c => c.Defaults(d => d.Controller("todo").Action("index")))
                                  .SetFilter<BeforeActionFilter>()
                                  .SetFilter<AfterActionFilter>()
                                  ;

            Router.Instance.Match("/viewcomponents/:controller(/:action(/:id))",
                                  c =>
                                  c.Match("(/:area/:controller(/:action(/:id)))", "viewcomponents",
                                          ic => ic.Defaults(d => d.Action("index")))
                );
        }
    }
}
