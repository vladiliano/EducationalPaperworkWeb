﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace EducationalPaperworkWeb.Infrastructure.Infrastructure.Conventions
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature", GetFeatureName(controller.ControllerType));
        }

        private static string GetFeatureName(TypeInfo controllerType)
        {
            var tokens = controllerType.FullName.Split('.');
            if (tokens.All(t => t != "Features"))
                return "";
            var featureName = tokens
                .SkipWhile(t => !t.Equals("features", StringComparison.CurrentCultureIgnoreCase))
                .Skip(1)
                .Take(1)
                .FirstOrDefault();

            return featureName;
        }
    }
}
