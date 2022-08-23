using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DynamicLinq
{
    public static class Evaluator
    {
        // Standard LINQ to objects
        public static bool Evaluate(Application application, Func<Application, bool> predicate)
        {
            var applications = new List<Application>();
            applications.Add(application);

            return applications.Any(predicate);
        }

        // Dynamic LINQ to objects
        public static bool Evaluate(Application application, string predicate)
        {
            var applications = new List<Application>();
            applications.Add(application);

            return applications.AsQueryable().Any(predicate);
        }

        // Standard LINQ to JSON
        public static bool Evaluate(string application, Func<JToken, bool> predicate)
        {
            var appObj = JObject.Parse(application);
            JArray applications = new JArray();
            applications.Insert(0, appObj);
            return applications.Any(predicate);
        }

        // Dynamic LINQ to JSON
        public static bool Evaluate(string application, string predicate)
        {
            var appJObject = JObject.Parse(application);
            var appLinqSource = appJObject.GenerateDynamicLinqSource();
            return new[] { appJObject }.AsQueryable().Select(appLinqSource).Any(predicate);
        }
    }
}
