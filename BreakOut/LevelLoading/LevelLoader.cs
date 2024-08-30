using System.Text.RegularExpressions;

namespace BreakOut
{
    /// <summary>
    /// A static class used for extracting information from level txt-files.
    /// </summary>
    static public class LevelLoader
    {
        static private string ReadLevel(string? path)
        {
            if (path == null)
            {
                return "";
            }

            StreamReader reader = File.OpenText(path);

            return reader.ReadToEnd();
        }

        static private Dictionary<string, string>? FindMetas(string text)
        {
            Match metaStart = Regex.Match(text, "Meta:");
            Match metaEnd = Regex.Match(text, "Meta/");

            string? metaText = null;
            if (metaStart.Success && metaEnd.Success)
            {
                metaText = text[metaStart.Index..metaEnd.Index];
            }

            if (metaText == null)
            {
                return null;
            }

            string[] metaArray = metaText.Split("\r\n");
            metaArray = metaArray[1..(metaArray.Length - 1)];

            Dictionary<string, string> meta = new();
            foreach (string i in metaArray)
            {
                string[] keyvalue = i.Split(": ");

                if (keyvalue.Length != 2)
                {
                    return null;
                }

                meta.Add(keyvalue[0], keyvalue[1]);
            }

            return meta;
        }


        static private Dictionary<string, string>? FindLegends(string text)
        {
            Match legendStart = Regex.Match(text, "Legend:");
            Match legendEnd = Regex.Match(text, "Legend/");

            string? legendText = null;
            if (legendStart.Success && legendEnd.Success)
            {
                legendText = text[legendStart.Index..legendEnd.Index];
            }

            if (legendText == null)
            {
                return null;
            }

            string[] legendArray = legendText.Split("\r\n");
            legendArray = legendArray[1..(legendArray.Length - 1)];

            Dictionary<string, string> legends = new();
            foreach (string i in legendArray)
            {
                string[] keyvalue = i.Split(") ");

                if (keyvalue.Length != 2)
                {
                    return null;
                }

                legends.Add(keyvalue[0], keyvalue[1]);
            }

            return legends;
        }

        static private string? FindMap(string text)
        {
            Match mapStart = Regex.Match(text, "Map:");
            Match mapEnd = Regex.Match(text, "Map/");

            String? map = null;

            if (mapStart.Success && mapEnd.Success)
            {
                map = text.Substring(mapStart.Index + mapStart.Length, mapEnd.Index - mapStart.Index - mapEnd.Length).Trim();
            }

            return map;
        }

        /// <summary>
        /// Extracts all relevant information from a level txt-file and returns a corresponding level which can be read by the game.
        /// </summary>
        static public Level LoadLevel(string? path)
        {
            string text = ReadLevel(path);

            string? map = FindMap(text);
            Dictionary<string, string>? meta = FindMetas(text);
            Dictionary<string, string>? legends = FindLegends(text);

            map ??= "";

            meta ??= new Dictionary<string, string>();

            legends ??= new Dictionary<string, string>();

            return new Level(map, meta, legends);
        }
    }
}
