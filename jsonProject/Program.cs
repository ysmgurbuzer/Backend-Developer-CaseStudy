using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Vertex
{
    public int x { get; set; }
    public int y { get; set; }
}

public class ShapeBoundary
{
    public List<Vertex> vertices { get; set; }
}

public class DescriptionData
{
    public string description { get; set; }
    public ShapeBoundary boundingPoly { get; set; }

    public static IEnumerable<DescriptionData> FromJsonInput(DescriptionData input)
    {
        // We created each row as a separate DescriptionData object
        var lines = input.description.Split('\n');
        foreach (var line in lines)
        {
            yield return new DescriptionData
            {
                description = line,
                boundingPoly = input.boundingPoly,
            };
        }
    }
}

class Program
{
    static void Main()
    {
        string filePath = @".\response.json";
        string json = File.ReadAllText(filePath);

        // We converted each entry in the JSON file into DescriptionData object and added it to the list
        var receiptData = new List<DescriptionData>();
        var jsonInputs = JsonConvert.DeserializeObject<List<DescriptionData>>(json);
        foreach (var jsonInput in jsonInputs)
        {
            var descriptionData = new DescriptionData
            {
                description = jsonInput.description,
                boundingPoly = jsonInput.boundingPoly
            };
            receiptData.Add(descriptionData);
        }

        int lineNumber = 1; 
        int? lastY = null; // Last y coordinate
        int threshold = 10; //We set a threshold for a new line
        Dictionary<int, string> lines = new Dictionary<int, string>(); // We used a dictionary to store the lines

        foreach (var descData in receiptData)
        {
            // We check if this is a new line
            var currentY = descData.boundingPoly.vertices[0].y;
            if (lastY.HasValue && Math.Abs(currentY - lastY.Value) > threshold)
            {
                lineNumber++;
            }
            lastY = currentY;

            if (lineNumber == 1)
            {
                lines[lineNumber] = descData.description;
            }
            else
            {
                if (lines.ContainsKey(lineNumber))
                {
                    lines[lineNumber] += " " + descData.description;
                }
                else
                {
                    lines[lineNumber] = descData.description;
                }
            }
        }

        foreach (var line in lines)
        {
            Console.WriteLine($"Line: {line.Key}, Text: {line.Value}");
        }
    }
}
