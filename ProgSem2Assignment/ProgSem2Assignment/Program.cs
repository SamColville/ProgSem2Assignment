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
        //Format strings for reports
        static string formatReportOne = "{0,-15}{1,-15}{2,-15}{3,10}{4,15}{5,20}";
        static string formatReportTwo = "{0,-15}{1,11}{2,11}{3,11}{4,11}{5,11}{6,11}{7,8}";

        static string[] oceanNames = { "Pacific", "Atlantic", "Mediterranean", "Indian Ocean", "Other Seas"};
        static string[] vesselType = {"AirCarrier", "Battleship", "Destroyer", "Frigate", "NuclearSub", "Minesweep"};


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
                    case 5:
                        FileOutput();
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
            Console.WriteLine("");//Used for formatting
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
        }//EOM





        //Method for option 1.
        static void MenuOne()
        {
            Console.WriteLine("\nMenu option 1");
            Console.WriteLine(formatReportOne, "Location", "Function", "Vessel Name", "Tonnage", "Crew", "Monthly Cost");
            Console.WriteLine("");//table formatting
            MenuOneReport();

        }//EOM

        //Method goes through csv to calculate cost and total costs/numbers
        //for full report of fleet.
        static void MenuOneReport()
        {
            string[] fields = new string[5];
            string lineIn;
            int tonnTot = 0, crewCost = 0, crewTot = 0, locationChange = 1;
            double costTot = 0;
            int type, tonn, crew, cost, locale;
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            lineIn = inputStream.ReadLine();

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                tonn = int.Parse(fields[2]);
                crew = int.Parse(fields[3]);
                locale = int.Parse(fields[4]);
                crewCost = GetCrewCost(type);
                cost = crewCost * crew;

                if(locale == locationChange)
                {
                    
                }
                else
                {
                    
                }
                //Report only displays values for ships with a tonnage
                //greater than 3500 and only acumulates data from
                //those vessels.
                if (tonn >= 3500)
                {
                    tonnTot += tonn;
                    crewTot += crew;
                    costTot += cost;
                    Console.WriteLine(formatReportOne, oceanNames[locale-1], vesselType[type-1], fields[0], tonn, crew, cost);
                }
                lineIn = inputStream.ReadLine();
            }
            Console.WriteLine("");
            Console.WriteLine(formatReportOne, "Grand Totals", "", "", tonnTot, crewTot, costTot);
            Console.WriteLine("");
            inputStream.Close();
            //ReadKey to provide pause before returning to menu
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }//EOM

        static int GetCrewCost(int type)
        {
            int crewCost;
            int[] crewCosts = { 2610, 2350, 2050, 999, 2550, 2510 };
            crewCost = crewCosts[type - 1];
            return crewCost;
        }



        //Option 2 start
        //Method for option 2. 
        static void MenuTwo()
        {
            int[] vesselGrandTotals = { 0, 0, 0, 0, 0, 0 };
            Console.WriteLine("");
            Console.WriteLine("Menu option 2");
            Console.WriteLine("");
            int finalTotal =  MenuTwoReport(vesselGrandTotals);
            Console.WriteLine("");
            Console.WriteLine(formatReportTwo, "Grand Total", vesselGrandTotals[0],vesselGrandTotals[1],vesselGrandTotals[2],vesselGrandTotals[3],vesselGrandTotals[4], vesselGrandTotals[5], finalTotal);
            Console.WriteLine("");
            //Pause at the end of report before returning to menu.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }//EOM

        //Report method for displaying a breakdown of fleet
        //in each ocean
        static int MenuTwoReport(int[] vesselGrandTotals)
        {
            string[] fields = new string[5];
            int[] vesselTotals = { 0, 0, 0, 0, 0, 0 };
            int[] oceanTotals = { 0, 0, 0, 0, 0 };

            string lineIn;
            int type, locale, locationChange = 1, grandTotal = 0, locationTotal = 0;
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            lineIn = inputStream.ReadLine();

            Console.WriteLine(formatReportTwo, "Location", vesselType[0],vesselType[1],vesselType[2],vesselType[3],vesselType[4],vesselType[5],"Total" );
            Console.WriteLine("");//table formatting

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                locale = int.Parse(fields[4]);

                if (locale != locationChange)
                {
                    for (int i = 0; i < vesselGrandTotals.Length; i++)
                    {
                        vesselGrandTotals[i] += vesselTotals[i];
                    }
                    locationTotal = vesselTotals.Sum();
                    Console.WriteLine(formatReportTwo, oceanNames[locale-1], vesselTotals[0], vesselTotals[1], vesselTotals[2], vesselTotals[3], vesselTotals[4], vesselTotals[5], locationTotal);
                    locationChange++;
                    for (int i = 0; i < vesselTotals.Length; i++)
                    {
                        vesselTotals[i] = 0;
                    }
                }


                switch (locale)
                {
                    case 1:
                        VesselSortAndTotal(vesselTotals, type);
                        break;
                    case 2:
                        VesselSortAndTotal(vesselTotals, type);
                        break;
                    case 3:
                        VesselSortAndTotal(vesselTotals, type);
                        break;
                    case 4:
                        VesselSortAndTotal(vesselTotals, type);
                        break;
                    case 5:
                        VesselSortAndTotal(vesselTotals, type);
                        break;
                    default:
                        Console.WriteLine("ERROR IN METHOD MENUTWOREPORT LOCALE SWITCH");
                        break;
                }
                grandTotal += 1;
                lineIn = inputStream.ReadLine();
            }
            inputStream.Close();
            return grandTotal;
        }//EOM

        //Method adds 1 to the relevant vessel type array
        static void VesselSortAndTotal(int[] totlas, int t)
        {
            switch(t)
            {
                case 1:
                    totlas[t - 1]++;
                    break;
                case 2:
                    totlas[t - 1]++;
                    break;
                case 3:
                    totlas[t - 1]++;
                    break;
                case 4:
                    totlas[t - 1]++;
                    break;
                case 5:
                    totlas[t - 1]++;
                    break;
                case 6:
                    totlas[t - 1]++;
                    break;
                default:
                    Console.WriteLine("ERROR IN VESSEL SORT AND TOTAL METHOD");
                    break;
            }
        }//EOM





        //method for option 3.
        static void MenuThree()
        {
            string searchName = "", displayMessage = "";
            Console.WriteLine("\nMenu option 3: Search for ship name. Type 'exit' to return to main menu.");

            while (searchName != "exit")
            {
                
                Console.Write("Enter ship name :   ");

                searchName = Console.ReadLine();
                displayMessage = SearchForShipName(searchName);
                searchName = searchName.ToLower();
                if (searchName != "exit")
                {
                    Console.WriteLine(displayMessage);
                }
            }
        }//EOM

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
                    locationStr = oceanNames[locale-1];
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
        }//EOM



        //Used in development to diaplay file
        //contents for quick validation
        static void FileOutput()
        {
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            string lineIn = inputStream.ReadLine();
            while(lineIn != null)
            {
                Console.WriteLine(lineIn);
                lineIn = inputStream.ReadLine();
            }

            inputStream.Close();
        }//EOM

    }
}
