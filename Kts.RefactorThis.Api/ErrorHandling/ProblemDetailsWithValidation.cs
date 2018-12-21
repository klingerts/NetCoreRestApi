using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Kts.RefactorThis.Api.ErrorHandling
{
    public class CustomProblemDetails : ProblemDetails
    {
        public CustomProblemDetails(string issue)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Instance = $"urn:{fileInfo.ProductName}-{fileInfo.ProductVersion}:badrequest:{Guid.NewGuid()}";
        }
    }

    public class ProblemDetailsWithValidation : CustomProblemDetails
    {
        public ProblemDetailsWithValidation(SerializableError error) : base("badrequest")
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            Status = 400;
            Title = "The request has validation errors";
            Detail = "See ValidationErrors for more details";
            ValidationErrors = error ?? new SerializableError();
        }

        public SerializableError ValidationErrors { get; set; } = null;
    }
}
