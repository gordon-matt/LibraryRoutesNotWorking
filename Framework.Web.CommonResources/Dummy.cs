﻿namespace Framework.Web.CommonResources
{
    // This is only here, because we don't have any other classes, but a class is needed to get the reference to the assembly.
    //  See the followin in FrameworkDemo.Startup.cs:
    //      new EmbeddedFileProvider(typeof(Dummy).GetTypeInfo().Assembly, "Framework.Web.CommonResources"),
    public class Dummy
    {
    }
}