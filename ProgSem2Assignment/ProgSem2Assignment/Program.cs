/************************************************************
 * Author:  Sam Colville                                    *
 * Date:    27/03/2018                                      *
 * This Program is designed to make reports based on info   *
 * in a CSV file about French bosts, located around the     *
 * world.                                                   *
 * The CSV file is located in the Debug folder contained    *
 * in this soloution.                                       *
 * Please be aware that the code is designed with this in   *
 * mind.                                                    *
 * NB: EOM signifies 'End of Method' and used               *
 * *********************************************************/


using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepingScores
{
    class Program
    {
        //Format strings for reports
        static string formatReportOne = "{0,-15}{1,-15}{2,-15}{3,10:N0}{4,15:N0}{5,20:C2}";
        static string formatReportTwo = "{0,-15}{1,11}{2,11}{3,11}{4,11}{5,11}{6,11}{7,8}";
        static string lineBreakString = "---";

        static string[] oceanNames = { "Pacific", "Atlantic", "Mediterranean", "Indian Ocean", "Other Seas" };
        static string[] vesselType = { "AirCarrier", "Battleship", "Destroyer", "Frigate", "NuclearSub", "Minesweep" };


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
                    case 6:
                        NumberOfRecord();
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
        }//EOM * * * * * * * * * * * * * * * * 





        //Method for option 1.
        static void MenuOne()
        {
            Console.WriteLine("\nMenu option 1");
            Console.WriteLine(formatReportOne, "Location", "Function", "Vessel Name", "Tonnage", "Crew", "Monthly Cost");
            Console.WriteLine("");//table formatting
            MenuOneReport();

        }//EOM * * * * * * * * * * * * * * * * 

        //Method goes through csv to calculate cost and total costs/numbers
        //for full report of fleet.
        static void MenuOneReport()
        {
            string[] fields = new string[5];
            //The following strings deal with data in the following
            //order: Tonnage, Crew, Crew Cost
            int[] subTotals = { 0, 0, 0 };
            int[] grandTotals = { 0, 0, 0 };
            int[] addToTotal = { 0, 0, 0 };
            string lineIn;
            int locationChange = 1;
            int type, cost, locale;
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            lineIn = inputStream.ReadLine();

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                addToTotal[0] = int.Parse(fields[2]);
                addToTotal[1] = int.Parse(fields[3]);
                locale = int.Parse(fields[4]);
                cost = GetCrewCost(type);
                addToTotal[2] = addToTotal[1] * cost;


                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
                 * When location changes program will add sub totals
                 * to grand totals, then re-initialise
                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                if (locale != locationChange)
                {
                    ChangeInLocation(subTotals, grandTotals);
                    locationChange++;
                }


                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
                 * Report only displays values for ships with a tonnage
                 * greater than 3500 and only acumulates data from those vessels.
                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                if (addToTotal[0] >= 3500)
                {
                    TotalsAccumulator(subTotals, addToTotal);
                    Console.WriteLine(formatReportOne, oceanNames[locale - 1], vesselType[type - 1], fields[0], addToTotal[0], addToTotal[1], addToTotal[2]);
                }

                lineIn = inputStream.ReadLine();
            }

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * 
             * Since the loop terminates on the final location,  *
             * the method displaying the subtotals must be       *
             * executed one final time.                          *
             * The program is unable to compare the last         *
             * location code with another.                       *
             * * * * * * * * * * * * * * * * * * * * * * * * * * */
            ChangeInLocation(subTotals, grandTotals);
            Console.WriteLine("");
            Console.WriteLine(formatReportOne, "Grand Totals", "---", "---", grandTotals[0], grandTotals[1], grandTotals[2]);
            Console.WriteLine("");
            inputStream.Close();

            //ReadKey to provide pause before returning to menu
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }//EOM * * * * * * * * * * * * * * * * 

        //Method to pass back the crew cost based
        //on type of vessel
        static int GetCrewCost(int type)
        {
            int crewCost;
            int[] crewCosts = { 2610, 2350, 2050, 999, 2550, 2510 };
            crewCost = crewCosts[type - 1];
            return crewCost;
        }

        static void TotalsAccumulator(int[] subTotals, int[] addToTotals)
        {
            for (int i = 0; i < subTotals.Length; i++)
            {
                subTotals[i] += addToTotals[i];
            }
        }

        //Method adds subtotals to grand total and re-initialises
        //subtotal 
        static void ChangeInLocation(int[] subTotals, int[] grandTotals)
        {
            Console.WriteLine(formatReportOne, "Sub Totals :", "---", "---", subTotals[0], subTotals[1], subTotals[2]);
            Console.WriteLine(formatReportOne, lineBreakString, lineBreakString, lineBreakString, lineBreakString, lineBreakString, lineBreakString);

            for (int i = 0; i < grandTotals.Length; i++)
            {
                grandTotals[i] += subTotals[i];
                subTotals[i] = 0;
            }
        }

        //Option 2 start
        //Method for option 2. 
        static void MenuTwo()
        {
            Console.WriteLine("");
            Console.WriteLine("Menu option 2");
            Console.WriteLine("");
            int finalTotal = MenuTwoReport();
            Console.WriteLine("");
            Console.WriteLine(formatReportTwo, "Grand Total", "", "", "", "", "", "", finalTotal);
            Console.WriteLine("");
            //Pause at the end of report before returning to menu.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 


        //Report method for displaying a breakdown of fleet
        //in each ocean
        static int MenuTwoReport()
        {
            string[] fields = new string[5];
            int[] vesselTotals = { 0, 0, 0, 0, 0, 0 };
            int[] oceanTotals = { 0, 0, 0, 0, 0 };

            string lineIn;
            int type, locale = 0, locationChange = 2, grandTotal = 0, locationTotal = 0;
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            lineIn = inputStream.ReadLine();

            Console.WriteLine(formatReportTwo, "Location", vesselType[0], vesselType[1], vesselType[2], vesselType[3], vesselType[4], vesselType[5], "Total");
            Console.WriteLine("");//table formatting

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                locale = int.Parse(fields[4]);

                VesselSortAndTotal(vesselTotals, type);
                locationTotal = vesselTotals.Sum();
                if (locale == locationChange) 
                {
                    

                    Console.WriteLine(formatReportTwo, oceanNames[locale - 2], vesselTotals[0], vesselTotals[1], vesselTotals[2], vesselTotals[3], vesselTotals[4], vesselTotals[5], locationTotal);

                    for (int i = 0; i < vesselTotals.Length; i++)
                    {
                        vesselTotals[i] = 0;
                    }
                    locationChange++;
                }



                grandTotal += 1;
                lineIn = inputStream.ReadLine();
            }

            inputStream.Close();
            Console.WriteLine(formatReportTwo, oceanNames[locale - 1], vesselTotals[0], vesselTotals[1], vesselTotals[2], vesselTotals[3], vesselTotals[4], vesselTotals[5], locationTotal);
            return grandTotal;

        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

        //Method adds 1 to the relevant vessel type array
        static void VesselSortAndTotal(int[] totlas, int t)
        {
            switch (t)
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

        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 












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
        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 

        static string SearchForShipName(string searchName)
        {
            string[] fields = new string[5];
            string lineIn, locationStr, message = "";
            int locale;
            FileStream fs = new FileStream("FrenchMF.txt", FileMode.Open, FileAccess.Read);
            StreamReader inputStream = new StreamReader(fs);
            lineIn = inputStream.ReadLine();
            message = "No match found. Please try again.";

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                locale = int.Parse(fields[4]);

                if (String.Compare(searchName, fields[0], CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                {
                    locationStr = oceanNames[locale -1];
                    message = "Location :   " + locationStr;

                }

                lineIn = inputStream.ReadLine();
            }
            inputStream.Close();
            return message;
        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        /*
        void string LocatonSearch(int locCode)
        {
            string locationStr = "";
            switch
        }*/



        //Used in development to diaplay file
        //contents for quick validation
        static void FileOutput()
        {
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            string lineIn = inputStream.ReadLine();
            while (lineIn != null)
            {
                Console.WriteLine(lineIn);
                lineIn = inputStream.ReadLine();
            }

            inputStream.Close();
        }//EOM * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

        static void NumberOfRecord()
        {
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            string lineIn = inputStream.ReadLine();
            int x = 0;
            while (lineIn != null)
            {
                x++;
                lineIn = inputStream.ReadLine();
            }
            inputStream.Close();
            Console.WriteLine(x);
        }

    }
}