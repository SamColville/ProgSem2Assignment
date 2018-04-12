using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepingScores
{
    class Program
    {
        static void Main(string[] args)
        {
            int menuOpt = 0;
            while (menuOpt != 4)
            {
                menuOpt = MenuChoice();
                switch (menuOpt)
                {
                    case 1:
                        MenuOne();
                        break;
                    case 2:
                        MenuTwo();
                        break;
                    case 3:
                        MenuThree();
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }

            }

            Console.WriteLine("Thank you for using this program.");
            Console.ReadKey();

        }

        static int MenuChoice()
        {
            int choice;
            Console.WriteLine("Menu");
            Console.WriteLine("");
            Console.WriteLine("1. Vessel Report");
            Console.WriteLine("2. Location Analysis Report");
            Console.WriteLine("3. Search for Vessel");
            Console.WriteLine("4. Exit");
            Console.WriteLine("");
            Console.Write("Enter choice :  ");
            choice = int.Parse(Console.ReadLine());
            return choice;
        }

        static void MenuOne()
        {
            Console.WriteLine("Menu option 1");
            string format = "{0,-15}{1,-25}{2,-15}{3,-10}{4,-15}{5,-20}";
            Console.WriteLine(format, "Location", "Function", "Vessel Name", "Tonnage", "Crew", "Monthly Cost");
            MenuOneReport(format);

        }

        static void MenuOneReport(string format)
        {
            string[] fields = new string[5];
            string lineIn, locationStr, typeStr;
            int tonnTot = 0, crewTot = 0, costTot = 0;
            int type, tonn, crew, cost, locale;
            FileStream fs = new FileStream("FrenchMF.txt", FileMode.Open, FileAccess.Read);
            StreamReader inputStream = new StreamReader(fs);
            lineIn = inputStream.ReadLine();

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                tonn = int.Parse(fields[2]);
                crew = int.Parse(fields[3]);
                locale = int.Parse(fields[4]);
                cost = tonn * crew;
                tonnTot += tonn;
                crewTot += crew;
                costTot += cost;
                locationStr = LocationString(locale);
                typeStr = TypeString(type);
                Console.WriteLine(format, locationStr, typeStr, fields[0], tonn, crew, cost);
                lineIn = inputStream.ReadLine();
            }
            Console.WriteLine(format, "Grand Totals", "", "", tonnTot, crewTot, costTot);
            inputStream.Close();
        }

        static string LocationString(int loc)
        {
            string location;

            if (loc == 1)
                location = "Pacific";
            else if (loc == 2)
                location = "Atlantic";
            else if (loc == 3)
                location = "Mediterranean";
            else if (loc == 4)
                location = "Indian";
            else if (loc == 5)
                location = "Other";
            else
                location = "ERROR IN LOCATION";

            return location;
        }

        static string TypeString(int type)
        {
            string typeStr;

            if (type == 1)
                typeStr = "Aircraft Carrier";
            else if (type == 2)
                typeStr = "Cruiser/Battleship";
            else if (type == 3)
                typeStr = "Destroyer";
            else if (type == 4)
                typeStr = "Frigate";
            else if (type == 5)
                typeStr = "Nuclear Submarine";
            else if (type == 6)
                typeStr = "Minelayer/Minesweeper";
            else
                typeStr = "ERROR IN TYPE";

            return typeStr;
        }

        static void MenuTwo()
        {
            Console.WriteLine("Menu option 2");
        }

        static void MenuThree()
        {
            Console.WriteLine("Menu option 3");
        }

    }
}
