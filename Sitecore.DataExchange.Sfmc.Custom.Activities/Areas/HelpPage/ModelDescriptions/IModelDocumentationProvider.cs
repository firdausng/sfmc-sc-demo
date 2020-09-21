using System;
using System.Reflection;

namespace Sitecore.DataExchange.Sfmc.Custom.Activities.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}