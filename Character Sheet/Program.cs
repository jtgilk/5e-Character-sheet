using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Spectre.Console;

namespace CharSheet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ClassList = APICall.GetFullList();
            Console.WriteLine("What would you like infromation on?");
            string entry = Console.ReadLine().ToLower();

            if(ClassList.Results.Any(p => p.Name == entry) | ClassList.Results.Any(p => p.Index == entry))
            {
                ClassEntry ClassMainEntry = APICall.GetEntry(entry);
                string PrintClassName = ClassMainEntry.ClassName;
                string PrintHitDice= ClassMainEntry.HitDice.ToString();
                //string PrintProfChoices = ClassMainEntry.ProficiencyChoices[0].
                string completedEntry = $"\r\n{PrintClassName}" +
                    $"\r\nHit Dice is {PrintHitDice}" +
                    $"\r\nProfeciencies choices are:";
                Console.WriteLine(completedEntry);
            }
            else 
            { 
                Console.WriteLine("Couldn't find it. Oops"); 
            }
        }

        public static bool CheckForQuit(string entry)
        {
            var quitCommands = new List<string> { "stop", "exit", "quit", "q", "return" };
            if (quitCommands.Any(str => str.Contains(entry)) && entry != "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}