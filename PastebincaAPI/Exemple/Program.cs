using System;
using PastebincaAPI;

namespace Exemple
{
    class Program
    {
        static void Main(string[] args)
        {
            Pastebin pastebin = new Pastebin();
            Console.WriteLine(@"URL: " + pastebin.SendPast("This Paste was sent using C# PastebincaAPI","Test from PastebincaAPI","This Paste was the exemple Paste of the PastebincaAPI",PasteContentType.CSharp,Expiration.day1));
            Console.WriteLine(@"RAW OUTPUT: " + pastebin.Response);
            Console.ReadKey();
        }
    }
}
