using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicLinq;

// See: https://stackoverflow.com/questions/72173795/dynamic-linq-to-evaluate-a-jobject
// The code below is lifted from: https://github.com/WireMock-Net/WireMock.Net/blob/master/src/WireMock.Net/Util/JsonUtils.cs
public static class JsonUtils
{
    public static string GenerateDynamicLinqSource(this JToken jsonObject)
    {
        var lines = new List<string>();
        WalkNode(jsonObject, null, null, lines);

        return lines.First();
    }

    private static void WalkNode(JToken node, string? path, string? propertyName, List<string> lines)
    {
        switch (node.Type)
        {
            case JTokenType.Object:
                ProcessObject(node, propertyName, lines);
                break;

            case JTokenType.Array:
                ProcessArray(node, propertyName, lines);
                break;

            default:
                ProcessItem(node, path ?? "it", propertyName, lines);
                break;
        }
    }

    private static void ProcessObject(JToken node, string? propertyName, List<string> lines)
    {
        var items = new List<string>();
        var text = new StringBuilder("new (");

        // In case of Object, loop all children. Do a ToArray() to avoid `Collection was modified` exceptions.
        foreach (var child in node.Children<JProperty>().ToArray())
        {
            WalkNode(child.Value, child.Path, child.Name, items);
        }

        text.Append(string.Join(", ", items));
        text.Append(")");

        if (!string.IsNullOrEmpty(propertyName))
        {
            text.AppendFormat(" as {0}", propertyName);
        }

        lines.Add(text.ToString());
    }

    private static void ProcessArray(JToken node, string? propertyName, List<string> lines)
    {
        var items = new List<string>();
        var text = new StringBuilder("(new [] { ");

        // In case of Array, loop all items. Do a ToArray() to avoid `Collection was modified` exceptions.
        var idx = 0;
        foreach (var child in node.Children().ToArray())
        {
            WalkNode(child, $"{node.Path}[{idx}]", null, items);
            idx++;
        }

        text.Append(string.Join(", ", items));
        text.Append("})");

        if (!string.IsNullOrEmpty(propertyName))
        {
            text.AppendFormat(" as {0}", propertyName);
        }

        lines.Add(text.ToString());
    }

    private static void ProcessItem(JToken node, string path, string? propertyName, List<string> lines)
    {
        var castText = node.Type switch
        {
            JTokenType.Boolean => $"bool({path})",
            JTokenType.Date => $"DateTime({path})",
            JTokenType.Float => $"double({path})",
            JTokenType.Guid => $"Guid({path})",
            JTokenType.Integer => $"long({path})",
            JTokenType.Null => "null",
            JTokenType.String => $"string({path})",
            JTokenType.TimeSpan => $"TimeSpan({path})",
            JTokenType.Uri => $"Uri({path})",
            _ => throw new NotSupportedException($"JTokenType '{node.Type}' cannot be converted to a Dynamic Linq cast operator.")
        };

        if (!string.IsNullOrEmpty(propertyName))
        {
            castText += $" as {propertyName}";
        }

        lines.Add(castText);
    }
}