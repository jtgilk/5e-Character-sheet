using System.Net;
using Newtonsoft.Json;
using System.IO;
using System;

namespace CharSheet
{
    public class APICall
    {
        private static string APIUrl = "https://www.dnd5eapi.co/api/";
        private static string APIUrlClasses = "https://www.dnd5eapi.co/api/classes/";
        private static WebClient webClient = new WebClient();

        //WebRequest attempts to download the requested information.
        //If it fails, it starts a loop that allows the user to retry or quit.
        //If successful, it returns a byte[].
        private static byte[] WebRequest(string url)
        {
            var data = new byte[] { };
            bool tryAgain = true;
            while (tryAgain)
            {
                try
                {
                    data = webClient.DownloadData(url);
                    tryAgain = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Hmm...It would appear that I do not have a connection.");
                    Console.WriteLine("Enter \"quit\" to exit the program. Enter anything to try again.");
                    string errorEntry = Console.ReadLine();
                    bool quit = BackStuff.CheckForQuit(errorEntry);
                    if (quit == false)
                    {
                        Console.WriteLine("See ya later!");
                        Environment.Exit(1);
                    }
                }
            }
            return data;
        }
        //Assigns the returned information to byte[] and builds it out into a string.
        private static string ReturnWebRequest(string url)
        {
            byte[] data = WebRequest(url);
            using (var stream = new MemoryStream(data))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        public static DnDList GetFullList()
        {
            return DeserializeClass(ReturnWebRequest(APIUrlClasses));
        }
        private static DnDList DeserializeClass(string json)
        {
            return JsonConvert.DeserializeObject<DnDList>(json);
        }
        public static ClassEntry GetEntry(string entry)
        {
            return DeserializeEntryJson(ReturnWebRequest("https://www.dnd5eapi.co/api/classes/" + entry));
        }
        private static ClassEntry DeserializeEntryJson(string json)
        {
            return JsonConvert.DeserializeObject<ClassEntry>(json);
        }
    }
}

