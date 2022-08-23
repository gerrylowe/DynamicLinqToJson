// See https://aka.ms/new-console-template for more information

using DynamicLinq;
using Newtonsoft.Json.Linq;

var application = new Application(4, "gerry");

// Standard LINQ to objects
Console.WriteLine("*** Standard LINQ to objects ***");
Console.WriteLine($"{Evaluator.Evaluate(application, a => a.IntProp == 3)}");
Console.WriteLine($"{Evaluator.Evaluate(application, a => a.IntProp == 4)}");
Console.WriteLine("");

// Dynamic LINQ to objects
Console.WriteLine("*** Dynamic LINQ to objects ***");
Console.WriteLine($"{Evaluator.Evaluate(application, "IntProp == 3")}");
Console.WriteLine($"{Evaluator.Evaluate(application, "IntProp == 4")}");
Console.WriteLine("");


var simpleAppJson = """
{
    "IntProp":4,
    "StringProp":"gerry"
}
""";
var complexAppJson = """
{
    "SimpleProp":7,
    "NestedProp":{
        "IntProp":4,
        "StringProp":"gerry"
    }
}
""";

// Standard LINQ to JSON
Console.WriteLine("*** Standard LINQ to JSON ***");
Console.WriteLine($"{Evaluator.Evaluate(simpleAppJson, a => ((int)a["IntProp"]) == 3)}");
Console.WriteLine($"{Evaluator.Evaluate(simpleAppJson, a => ((int)a["IntProp"]) == 4)}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, a => ((int)a["NestedProp"]["IntProp"]) == 3)}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, a => ((int)a["NestedProp"]["IntProp"]) == 4)}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, a => ((int)a["SimpleProp"] == 7) && ((int)a["NestedProp"]["IntProp"] == 4))}");
Console.WriteLine("");

// Dynamic LINQ to JSON
Console.WriteLine("*** Dynamic LINQ to JSON ***");
Console.WriteLine($"{Evaluator.Evaluate(simpleAppJson, @"IntProp == 3")}");
Console.WriteLine($"{Evaluator.Evaluate(simpleAppJson, @"IntProp == 4")}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, @"NestedProp.IntProp == 3")}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, @"NestedProp.IntProp == 4")}");
Console.WriteLine($"{Evaluator.Evaluate(complexAppJson, @"SimpleProp == 7 && NestedProp.IntProp == 4")}");
Console.WriteLine("");


Console.WriteLine("Success!");
