using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

public class LibraryInfo
{
    public String LibraryName { get; set; }
    public Dictionary<string, List<string>> Categories { get; set; }

    public LibraryInfo(string libraryName, Dictionary<string, List<string>> categories)
    {
        LibraryName = libraryName;
        Categories = categories;
    }

    public string SerializeToJson()
    {
        var textWriter = new StringWriter();
        (new JsonSerializer()).Serialize(textWriter, this);
        return textWriter.ToString();
    }

    public static LibraryInfo DeserializeFromJson(string serialized)
    {
        try
        {
            return (LibraryInfo)(new JsonSerializer()).Deserialize(new StringReader(serialized), typeof(LibraryInfo));
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to deserialize LibraryInfo from '" + serialized + "': " + e.Message);
        }
        return null;
    }
}
