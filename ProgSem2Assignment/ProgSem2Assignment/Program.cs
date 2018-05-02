/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Author:  Sam Colville                                     *
 * Date:    27/03/2018                                       *
 * This Program is designed to make reports based on info    *
 * in a CSV file about French bosts, located around the      *
 * world.                                                    *
 * The CSV file is located in the Debug folder contained     *
 * in this soloution.                                        *
 * Please be aware that the code is designed with this in    *
 * mind.                                                     *
 * NB: EOM signifies 'End of Method' and used                *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


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
        static string formatReportFour = "{0,-15}{1,-15}{2,-15}";
        static string lineBreakString = "---";

        static string[] oceanNames = { "Pacific", "Atlantic", "Mediterranean", "Indian Ocean", "Other Seas" };
        static string[] vesselType = { "AirCarrier", "Battleship", "Destroyer", "Frigate", "NuclearSub", "Minesweeper" };


        static void Main()
        {
            int menuOpt = 0;
            while (menuOpt != 5)
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
                        MenuFour();
                        break;
                    case 5:
                        MenuFive();
                        break;
                    case 6:
                        //exit program
                        break;

                        //used in development
                    case 7:
                        FileOutput();
                        break;
                    case 8:
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

        //Method to display options and read in the user's choice
        static int MenuChoice()
        {
            int choice;
            Console.WriteLine("");//Used for formatting
            Console.WriteLine("Main Menu");
            Console.WriteLine("[Enter a number corresponding to the menu option you would like]");
            Console.WriteLine("1. Vessel Report");
            Console.WriteLine("2. Location Analysis Report");
            Console.WriteLine("3. Search for Vessel");
            Console.WriteLine("4. Vessel Type Search");
            Console.WriteLine("5. Search for Vessel in Location");
            Console.WriteLine("6. Exit");
            Console.WriteLine("");
            Console.Write("Enter choice :  ");
            if(!int.TryParse(Console.ReadLine(),out choice))
            {
                Console.WriteLine("Please enter a number, 1, 2, 3 or 4.");
            }
            return choice;
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x





        //Method for option 1.
        static void MenuOne()
        {
            Console.WriteLine("\nMenu option 1:");
            Console.WriteLine("Vessel Report");
            Console.WriteLine(formatReportOne, "Location", "Function", "Vessel Name", "Tonnage", "Crew", "Monthly Cost");
            Console.WriteLine("");//table formatting
            MenuOneReport();

        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x

        //Method goes through csv to calculate cost and total costs/numbers
        //for full report of fleet.
        static void MenuOneReport()
        {
            string[] fields = new string[5];

            /*The following strings deal with data in the following
             *order: Tonnage, Crew, Crew Cost */

            //Arrayfor sub totals of ships in each area
            int[] subTotals = { 0, 0, 0 };
            //Array for the grand totals in each area
            int[] grandTotals = { 0, 0, 0 };
            //Array for mubers that are required to add to the subtotals
            int[] addToTotal = { 0, 0, 0 };

            string dataLineOne;
            int locationChange = 1, vesselCode, crewCost, localeCode;
            StreamReader inputStreamOne = new StreamReader("FrenchMF.txt");
            dataLineOne = inputStreamOne.ReadLine();

            while (dataLineOne != null)
            {
                fields = dataLineOne.Split(',');
                vesselCode = int.Parse(fields[1]);
                addToTotal[0] = int.Parse(fields[2]);
                addToTotal[1] = int.Parse(fields[3]);
                localeCode = int.Parse(fields[4]);
                crewCost = GetCrewCost(vesselCode);
                addToTotal[2] = addToTotal[1] * crewCost;


                /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
                 * When location changes program will add sub totals
                 * to grand totals, then re-initialise
                 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
                if (localeCode != locationChange)
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
                    Console.WriteLine(formatReportOne, oceanNames[localeCode - 1], vesselType[vesselCode - 1], fields[0], addToTotal[0], addToTotal[1], addToTotal[2]);
                }

                dataLineOne = inputStreamOne.ReadLine();
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
            inputStreamOne.Close();

            //ReadKey to provide pause before returning to menu
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x

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



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Menu Option 2:
         * Option 2 will generate a report that displays the breakdown of
         * the fleet in each area; detaling the number of each typr of boat
         * and total boats in the given area. 
          * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        static void MenuTwo()
        {
            Console.WriteLine("");
            Console.WriteLine("Menu option 2:");
            Console.WriteLine("Location Analysis Report");
            int finalTotal = MenuTwoReport();
            Console.WriteLine("");
            Console.WriteLine(formatReportTwo, "Grand Total", "", "", "", "", "", "", finalTotal);
            Console.WriteLine("");
            //Pause at the end of report before returning to menu.
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Method that opens the CSV and details the fleet numbers
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        static int MenuTwoReport()
        {
            string[] fields = new string[5];
            int[] vesselTotals = { 0, 0, 0, 0, 0, 0 };
            int[] oceanTotals = { 0, 0, 0, 0, 0 };

            string lineIn;
            int type, localeCode = 0, locationChange = 2, grandTotal = 0, locationTotal = 0;
            StreamReader inputStreamTwo = new StreamReader("FrenchMF.txt");
            lineIn = inputStreamTwo.ReadLine();

            Console.WriteLine(formatReportTwo, "Location", vesselType[0], vesselType[1], vesselType[2], vesselType[3], vesselType[4], vesselType[5], "Total");
            Console.WriteLine("");//table formatting

            while (lineIn != null)
            {
                fields = lineIn.Split(',');
                type = int.Parse(fields[1]);
                localeCode = int.Parse(fields[4]);
                VesselSortAndTotal(vesselTotals, type);
                locationTotal = vesselTotals.Sum();
                if (localeCode == locationChange) 
                {
                    

                    Console.WriteLine(formatReportTwo, oceanNames[localeCode - 2], vesselTotals[0], vesselTotals[1], vesselTotals[2], vesselTotals[3], vesselTotals[4], vesselTotals[5], locationTotal);

                    for (int i = 0; i < vesselTotals.Length; i++)
                    {
                        vesselTotals[i] = 0;
                    }
                    locationChange++;
                }



                grandTotal += 1;
                lineIn = inputStreamTwo.ReadLine();
            }

            inputStreamTwo.Close();
            Console.WriteLine(formatReportTwo, oceanNames[localeCode - 1], vesselTotals[0], vesselTotals[1], vesselTotals[2], vesselTotals[3], vesselTotals[4], vesselTotals[5], locationTotal);
            return grandTotal;

        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x

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

        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Menu option 3: 
         * Option 3 allows the user to searvh the CSV for the name of a specific 
         * ship. The user need not use special characters such as  or  .
         * The method will display all areas the ship name occours, but 
         * code has been included if only the final result is desired.
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * **/
        static void MenuThree()
        {
            string searchName = "", displayMessage = "";
            Console.WriteLine("\nMenu option 3:");
            Console.WriteLine("Search for ship by name. Type 'exit' to return to main menu.");

            while (searchName != "exit")
            {

                Console.Write("Enter ship name :   ");

                searchName = Console.ReadLine();
                displayMessage = SearchForShipName(searchName);
                searchName = searchName.ToLower();

                //This is used to write out only the last entry found
                /*if (searchName != "exit")
                {
                    Console.WriteLine(displayMessage);
                }*/
            }
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x


        static string SearchForShipName(string searchName)
        {
            string[] fields = new string[5];
            string dataLineThree, locationStr, message = "No mathch found. Tray again, or type 'exit' to return.";
            int locale;

            StreamReader inputStreamThree = new StreamReader("FrenchMF.txt");
            dataLineThree = inputStreamThree.ReadLine();

            while (dataLineThree != null)
            {
                fields = dataLineThree.Split(',');
                locale = int.Parse(fields[4]);

                //
                if (String.Compare(searchName, fields[0], CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
                {
                    locationStr = oceanNames[locale -1];
                    message = "Location :   " + locationStr;
                    Console.WriteLine(message);
                }

                dataLineThree = inputStreamThree.ReadLine();
            }
            inputStreamThree.Close();
            return message;
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Menu Option 4:
         * This option allows the user to see detailed
         * analysis on where each type of vessel is located,
         * and the names of those vessels.
         * * * * * * * * * * * * * * * * * * * * * * * * * * * *  * */
        static void MenuFour()
        {
            string searchCode = "";
            Console.WriteLine("Menu Option 4 :");
            Console.WriteLine("[Enter the vessel type code from the list below.");
            while (searchCode != "exit")
            {
                for (int i = 0; i < vesselType.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, vesselType[i]);
                }

                Console.Write("Enter vessel code :   ");

                searchCode = Console.ReadLine();
                searchCode = searchCode.ToLower();
                if (searchCode != "exit")
                {
                    VesselSearch(searchCode);
                }
            }
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x

        static void VesselSearch(string searchCode)
        {
            string[] fields = new string[5];
            string dataLineFour;
            int locale,typeCode;

            StreamReader inputStreamFour = new StreamReader("FrenchMF.txt");
            dataLineFour = inputStreamFour.ReadLine();
            Console.WriteLine("");
            Console.WriteLine(formatReportFour, "Vessel Type","Location","Name");
            Console.WriteLine(formatReportFour, lineBreakString, lineBreakString, lineBreakString);

            while (dataLineFour != null)
            {
                fields = dataLineFour.Split(',');
                locale = int.Parse(fields[4]);
                typeCode = int.Parse(fields[1]);
                if(searchCode == fields[1])
                {
                    Console.WriteLine(formatReportFour, vesselType[typeCode-1],oceanNames[locale-1],fields[0]);
                }

                dataLineFour = inputStreamFour.ReadLine();
            }
            Console.WriteLine("");
            inputStreamFour.Close();
        }//EOM x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x x



        static void MenuFive()
        {
            string message = "";
            int locationCode = 0, typeCode = 0;
            Console.WriteLine("");
            Console.WriteLine("Please enter a location:");
            while(locationCode != -999)
            {
                for (int i = 0; i < oceanNames.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, oceanNames[i]);
                }
                Console.Write("Enter code:  ");
                locationCode = int.Parse(Console.ReadLine());
                if(locationCode == -999)
                {
                    break;
                }


                Console.WriteLine("");
                Console.WriteLine("Now enter a vessel type:");
                for (int i = 0; i < vesselType.Length; i++)
                {
                    Console.WriteLine("{0}. {1}", i + 1, vesselType[i]);
                }
                Console.Write("Enter code:  ");
                typeCode = int.Parse(Console.ReadLine());
                Console.WriteLine("");

                message = LocationTypeSearch(locationCode, typeCode);
                Console.WriteLine(message);
                Console.WriteLine(lineBreakString);

            }
        }



        static string LocationTypeSearch(int inputLocale, int inputType)
        {
            string[] fields = new string[5];
            string dataLineFive, message = "No matches found. Try again or type -999 to return to Main Menu.";
            int locale, typeCode;

            StreamReader inputStreamFive = new StreamReader("FrenchMF.txt");
            dataLineFive = inputStreamFive.ReadLine();

            while(dataLineFive != null)
            {
                fields = dataLineFive.Split(',');
                typeCode = int.Parse(fields[1]);
                locale = int.Parse(fields[4]);

                if(locale == inputLocale && typeCode == inputType)
                {
                    Console.WriteLine("Ship found: {0}", fields[0]);
                    message = "Ship(s) found";
                    Console.WriteLine();
                }

                dataLineFive = inputStreamFive.ReadLine();
            }
            inputStreamFive.Close();
            return message;
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * The following methods are included from development process
         * or may be used as alternatives for techniques implemented
         * in the above code.
         * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        //Method may be used if array in the class head must be kept more private
        static string LocatonSearch(int locCode)
        {
            string locationStr = "";
            switch(locCode)
            {
                case 1:
                    locationStr = "Pacific";
                    break;
                case 2:
                    locationStr = "Atlantic";
                    break;
                case 3:
                    locationStr = "Mediterranean";
                    break;
                case 4:
                    locationStr = "Indian";
                    break;
                case 5:
                    locationStr = "Other";
                    break;
                default:
                    locationStr = "Error in LocationSearch()";
                    break;
            }
            return locationStr;
        }


        //static string[] vesselType = { "AirCarrier", "Battleship", "Destroyer", "Frigate", "NuclearSub", "Minesweep" };
        static string BoatType(int boatCode)
        {
            string vesselTypeStr;
            switch(boatCode)
            {
                case 1:
                    vesselTypeStr = "Aircraft Carrier";
                    break;
                case 2:
                    vesselTypeStr = "Battleship";
                    break;
                case 3:
                    vesselTypeStr = "Destroyer";
                    break;
                case 4:
                    vesselTypeStr = "Frigate";
                    break;
                case 5:
                    vesselTypeStr = "Nuclear Submarine";
                    break;
                case 6:
                    vesselTypeStr = "Minesweeper/Minelayer";
                    break;
                default:
                    vesselTypeStr = "Error in BoatType()";
                    break;
            }
            return vesselTypeStr;
        }



        //Used in development to diaplay file
        //contents for quick validation
        static void FileOutput()
        {
            StreamReader inputStream = new StreamReader("FrenchMF.txt");
            string dataLineThree = inputStream.ReadLine();
            while (dataLineThree != null)
            {
                Console.WriteLine(dataLineThree);
                dataLineThree = inputStream.ReadLine();
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