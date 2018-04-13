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
        static string formatReportOne = "{0,-15}{1,-25}{2,-15}{3,10}{4,15}{5,20}";
        static string formatReportTwo = "{0,-15}{1,5}{2,5}{3,5}{4,5}{5,5}{6,5}{7,5}";
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

        //Method for option 1.
        static void MenuOne()
        {
            Console.WriteLine("Menu option 1");
            Console.WriteLine(formatReportOne, "Location", "Function", "Vessel Name", "Tonnage", "Crew", "Monthly Cost");
            Console.WriteLine("\n");
            MenuOneReport();

        }

        //Method goes through csv to calculate cost and total costs/numbers
        //for full report of fleet.
        static void MenuOneReport()
        {
            string[] fields = new string[5];
            string lineIn, locationStr, typeStr;
            int tonnTot = 0, crewTot = 0;
            double costTot = 0;
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
                locationStr = LocationString(locale);
                typeStr = TypeString(type);
                //Report only displays values for ships with a tonnage
                //greater than 3500 and only acumulates data from
                //those vessels.
                if (tonn >= 3500)
                {
                    tonnTot += tonn;
                    crewTot += crew;
                    costTot += cost;
                    Console.WriteLine(formatReportOne, locationStr, typeStr, fields[0], tonn, crew, cost);
                }
                lineIn = inputStream.ReadLine();
            }
            Console.WriteLine(formatReportOne, "Grand Totals", "", "", tonnTot, crewTot, costTot);
            inputStream.Close();
            //ReadKey to provide pause before returning to menu
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void MenuTwoReport()
        {
            string[] fields = new string[5];
            string lineIn, locationStr, typeStr;
            int type, locale;
            FileStream fs = new FileStream("FrenchMF.txt", FileMode.Open, FileAccess.Read);
            StreamReader inputStream = new StreamReader(fs);
            lineIn = inputStream.ReadLine();

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                locale = int.Parse(fields[4]);

                switch (locale)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }


            }

            inputStream.Close();
   
        }

        static void ShipTypeSort(int type)
        {
            if (type == 1)
            {
                
            }
            else if (type == 2)
            {
                
            }
            else if (type == 3)
            {
                
            }
            else if (type == 4)
            {
                
            }
            else if (type == 5)
            {
                
            }
            else if (type == 6)
            {
                
            }
            else
            {
                
            }

        }

        //Method to sort location from csv file adn returns location as a string.
        static string LocationString(int loc)
        {
            string location;

            switch (loc)
            {
                case 1:
                    location = "Pacific";
                    break;
                case 2:
                    location = "Atlantic";
                    break;
                case 3:
                    location = "Mediterranean";
                    break;
                case 4:
                    location = "Indian";
                    break;
                case 5:
                    location = "Other";
                    break;
                default:
                    location = "ERROR IN LOCATION";
                    break;
            }

            return location;
        }

        //Method to sort csv file and return string of the 
        //vessel type.
        static string TypeString(int type)
        {
            string typeStr;

            switch (type)
            {
                case 1:
                    typeStr = "Aircraft Carrier";
                    break;
                case 2:
                    typeStr = "Cruiser/Battleship";
                    break;
                case 3:
                    typeStr = "Destroyer";
                    break;
                case 4:
                    typeStr = "Frigate";
                    break;
                case 5:
                    typeStr = "Nuclear Submarine";
                    break;
                case 6:
                    typeStr = "Minelayer/Minesweeper";
                    break;
                default:
                    typeStr = "ERROR IN TYPE";
                    break;
            }

            return typeStr;
        }

        //Method for option 2. 
        static void MenuTwo()
        {
            Console.WriteLine("Menu option 2");
        }

        //method for option 3.
        static void MenuThree()
        {
            string searchName="", displayMessage="";
            Console.WriteLine("Menu option 3: Search for ship name. Type exit to return to main menu.");
            while (searchName != "exit")
            {
                Console.Write("Enter ship name :   ");
                searchName = Console.ReadLine();

                displayMessage = SearchForShipName(searchName);
                Console.WriteLine(displayMessage);
                searchName.ToLower();

            }



        }

        static string SearchForShipName(string searchName)
        {
            string[] fields = new string[5];
            string lineIn, locationStr, message ="";
            int locale;
            FileStream fs = new FileStream("FrenchMF.txt", FileMode.Open, FileAccess.Read);
            StreamReader inputStream = new StreamReader(fs);
            lineIn = inputStream.ReadLine();

            while(lineIn != null)
            {
                fields = lineIn.Split(',');
                locale = int.Parse(fields[4]);

                if(searchName == fields[0])
                {
                    locationStr = LocationString(locale);
                    message = "Location :   " + locationStr;
                }
                else
                {
                    message = "No match found. Please try again";
                }

                lineIn = inputStream.ReadLine();
            }



            inputStream.Close();
            return message;
        }

    }
}
