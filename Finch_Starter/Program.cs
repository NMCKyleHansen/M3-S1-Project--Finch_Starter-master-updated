using FinchAPI;
using System;
using System.Linq;

namespace Finch_Starter
{
    class Program
    {
        // *************************************************************
        // Title:               Finch Control
        // Application Type:    Console
        // Description:         This program allows the user to interact 
        //                      with the Finch robot via the Finch API.
        // Author:              Kyle Hansen
        // Date Created:        09/30/2020
        // Date Modified:       10/17/2020        
        // *************************************************************

        static void Main(string[] args)
        {

            //
            // create a new Finch object
            //
            Finch myFinch;
            myFinch = new Finch();

            //
            // Display welcome screen
            DisplayWelcomeScreen();

            //
            // Display Main Menu

            DisplayMainMenuScreen(myFinch);

            //
            // Display closing Screen
            DisplayClosingScreen();
        }

        //
        // This method runs a continue prompt
        //
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.ReadKey();
        }

        //
        // This method clears the screen and writes a passed header text
        //
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine(headerText);
        }

        //
        // This method displays a welcome screen
        //
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\tThis program allows the you to interact");
            Console.WriteLine("\t\twith the Finch robot via the Finch API.");
            Console.WriteLine();
            Console.WriteLine("\t\tPlease Press any key to continue.");
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            DisplayContinuePrompt();

        }

        //
        // This method displays a closing screen
        //
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine("\t\t*******************************************");
            Console.WriteLine("\t\tThank you for using this program to interact");
            Console.WriteLine("\t\ttwith the Finch robot via the Finch API.");
            Console.WriteLine();
            Console.WriteLine("\t\tPlease Press any key to exit.");
            Console.WriteLine();
            Console.WriteLine("\t\t*******************************************");
            DisplayContinuePrompt();
            Console.Clear();
        }

        //
        // This method connects to finch robot
        //
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            string userInput;
            bool connected = false;
            bool connectedLoop = false;
            DisplayHeader("The application will attempt to connect to the finch robot. Please hit any key to contunue.");
            DisplayContinuePrompt();
            while (connectedLoop == false)
            {
                connected = finchRobot.connect();
                if (connected)
                {
                    DisplayHeader("The application is connected to the finch robot. Please hit any key to contunue.");
                    DisplayContinuePrompt();
                    connectedLoop = true;
                }
                else
                {
                    DisplayHeader("The connection failed. Make sure the robot is connected. Do you wish to try agian(y or n)?");
                    userInput = Console.ReadLine().ToLower();
                    // Keep attempting connection loop unless told not to
                    if (userInput != null && userInput == "n")
                    {
                        connectedLoop = true;
                    }
                }
            }
            return connected;
        }

        //
        // This method disconnect from a given finch robot
        //
        static bool DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayHeader("The application will disconnect from the finch robot. Please hit any key to contunue.");
            DisplayContinuePrompt();
            finchRobot.disConnect();
            Console.WriteLine("The application is disconnected from the finch robot. Please hit any key to contunue.");
            DisplayContinuePrompt();
            return true;
        }
        //
        // This method is the data recorder menu. 
        //
        static void DataRecorderDisplayMenuScreen(Finch finchRobot)
        {

            bool menuloop = true;
            string userInput;
            int numberOfDataPoints = 0;
            Double dataPointFrequency = 0;
            Double[] temperatures = null;

            do
            {
                DisplayHeader("Data Recorder Menu..Please Select from the following options:");
                Console.WriteLine("\t\t*******************************************");
                Console.WriteLine();
                Console.WriteLine("\t\t1. Number of Data Points");
                Console.WriteLine("\t\t2. Frequency of Data Points");
                Console.WriteLine("\t\t3. Get Data");
                Console.WriteLine("\t\t4. Show Data");
                Console.WriteLine("\t\t5. Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine("\t\t*******************************************");
                userInput = Console.ReadKey().Key.ToString().Substring(1, 1);
                switch (userInput)
                {
                    case "1":
                        numberOfDataPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;
                    case "2":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;
                    case "3":
                        temperatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;
                    case "4":
                        DataRecorderDisplayData(temperatures);
                        break;
                    case "5":
                        menuloop = false;
                        break;
                    default:
                        break;
                }
            } while (menuloop);
        }
        //
        // This method gets the frequency of data points. 
        //
        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double returnValue;
            string userValue;
            bool isNumber = false;
            // Validate user input string via a bool and a loop. If false stay in loop. If a number leave the loop.
            do
            {
                DisplayHeader("Please enter the frequency of the readings in seconds:");
                userValue = Console.ReadLine();
                isNumber = double.TryParse(userValue, out returnValue);
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the frequency of: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }

        //
        // This method lets the user select the number of data points
        //
        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int returnValue;
            string userValue;
            bool isNumber = false;
            // Validate user input string via a bool and a loop. If false stay in loop. If a number leave the loop.
            do
            {
                DisplayHeader("Please enter the number of data points:");
                userValue = Console.ReadLine();
                isNumber = int.TryParse(userValue, out returnValue);
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following number of data points: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }

        //
        // This method gets data(Temperature, right light sensor value, left light sensor value)
        //
        static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] returnArray = new double[numberOfDataPoints];
            int[] rightLight = new int[numberOfDataPoints];
            int[] leftLight = new int[numberOfDataPoints];
            int leftLightSum;
            int rightLightSum;
            double tempAvg;
            string userValue;



            DisplayHeader("Get Data Screen");
            Console.WriteLine("We will collect the folowing number of data points: " + numberOfDataPoints);
            Console.WriteLine("For the following Frequency: " + dataPointFrequency);
            Console.WriteLine("The program is ready to read in the data.");
            Console.WriteLine("Please hit any key to contunue.");
            Console.WriteLine("*******************************************************");
            DisplayContinuePrompt();
            Console.WriteLine("");
            for (int arrayIndex = 0; arrayIndex < numberOfDataPoints; arrayIndex++)
            {
                returnArray[arrayIndex] = ConvertCelsiusToFahrenheit(finchRobot.getTemperature());
                rightLight[arrayIndex] = finchRobot.getRightLightSensor();
                leftLight[arrayIndex] = finchRobot.getLeftLightSensor();
                Console.WriteLine("Reading " + (arrayIndex + 1) + " :" + (returnArray[arrayIndex]).ToString("n2") + " Right Light:" + rightLight[arrayIndex] + " Left Light:" + leftLight[arrayIndex]);
                finchRobot.wait((int)(dataPointFrequency * 1000));
            }
            rightLightSum = rightLight.Sum();
            leftLightSum = leftLight.Sum();
            tempAvg = returnArray.Average();
            Console.WriteLine("");
            Console.WriteLine("The average Temperature:" + tempAvg.ToString());
            Console.WriteLine("Right light sum of values:" + rightLightSum.ToString());
            Console.WriteLine("Left light sum of value:" + leftLightSum.ToString());
            Console.WriteLine("Right light Average Value:" + (rightLightSum / numberOfDataPoints).ToString());
            Console.WriteLine("Left light Average Value:" + (leftLightSum / numberOfDataPoints).ToString());
            Console.WriteLine("");
            Console.WriteLine("The data recording is now complete.");

            do
            {
                Console.WriteLine("Do you want to see a list of sorted light values? Please enter (y or n):");
                userValue = Console.ReadLine().ToLower().Substring(0, 1);
            }
            while (userValue != "y" && userValue != "n");

            if (userValue == "y")
            {
                Array.Sort(rightLight);
                Array.Sort(leftLight);
                Console.WriteLine("Right Value".PadLeft(15) + "Left Value".PadLeft(15));
                Console.WriteLine("---------".PadLeft(15) + "-----------".PadLeft(15));
                for (int i = 0; i < numberOfDataPoints; i++)
                {
                    Console.WriteLine(rightLight[i].ToString().PadLeft(15) + leftLight[i].ToString().PadLeft(15));
                }

            }
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnArray;
        }

        //
        // This method converts a Celsius double into a ToFahrenheit double( we would not need this in Canada). 
        //
        static double ConvertCelsiusToFahrenheit(double celsiusTemp)
        {
            return (celsiusTemp * (1.8)) + 32;
        }
        static void DataRecorderDisplayData(double[] data)
        {
            DisplayHeader("Displaying data:");
            Console.WriteLine("");
            DataRecorderDisplayDataTable(data);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
        }
        static void DataRecorderDisplayDataTable(double[] data)
        {
            Console.WriteLine("Reading #".PadLeft(15) + "temperature".PadLeft(15));
            Console.WriteLine("---------".PadLeft(15) + "-----------".PadLeft(15));
            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine((i+ 1).ToString().PadLeft(15) + data[i].ToString("n2").PadLeft(15));
            }
        }


        //
        // This method is the light alarm system menu. 
        //
        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {

            bool menuloop = true;
            string userInput;
            String sensorsToMonitor="";
            String rangeType = "";
            int minMaxThresholdValue=0;
            int timeToMonitor=0;
            do
            {
                DisplayHeader("Light Alarm Menu..Please Select from the following options:");
                Console.WriteLine("\t\t*******************************************");
                Console.WriteLine();
                Console.WriteLine("\t\t1. Set Sensors to Monitor");
                Console.WriteLine("\t\t2. Set Range Type");
                Console.WriteLine("\t\t3. Set Maximum/Minimum Threshold Value");
                Console.WriteLine("\t\t4. Set Time to Monitor");
                Console.WriteLine("\t\t5. Set Alarm");
                Console.WriteLine("\t\t6. Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine("\t\t*******************************************");
                userInput = Console.ReadKey().Key.ToString().Substring(1, 1);
                switch (userInput)
                {
                    case "1":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;
                    case "2":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;
                    case "3":
                        minMaxThresholdValue = LightAlarmDisplaySetMinMaxThresholdValue(rangeType,finchRobot);
                        break;
                    case "4":
                        timeToMonitor = LightAlarmDisplaySetMaximumTimeToMonitor();
                        break;
                    case "5":
                        LightAlarmDisplaySetAlarm(finchRobot,sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;
                    case "6":
                        menuloop = false;
                        break;
                    default:
                        break;
                }
            } while (menuloop);
        }


        //
        // This method gets the minimum/maximum Threshold Value for light 
        //
        private static int LightAlarmDisplaySetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            int returnValue;
            bool isNumber = false;
            // Validate user input number via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("minimum/maximum Threshold Value.");
                Console.WriteLine($"left light sensor ambient value: { finchRobot.getLeftLightSensor()}");
                Console.WriteLine($"Right light sensor ambient value: { finchRobot.getRightLightSensor()}");
                Console.WriteLine("");
                Console.WriteLine($"Enter the {rangeType} sensor value: ");
                isNumber = int.TryParse(Console.ReadLine(), out returnValue);
                if (isNumber == false)
                {
                    Console.WriteLine("Your response was not understood.");
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following value: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }
        //
        // This method lets you pick your sensors 
        //
        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string returnValue = null;
            string userValue;
            bool stayInLoop = false;
            // Validate user input string via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("Please enter the sensors that you wish to monitor(left,right,both):");
                userValue = Console.ReadLine();
                if (userValue.ToUpper().Substring(0, 1) == "L")
                {
                    returnValue = "left";
                    stayInLoop = true;
                }
                else if (userValue.ToUpper().Substring(0, 1) == "R")
                {
                    returnValue = "right";
                    stayInLoop = true;
                }
                else if (userValue.ToUpper().Substring(0, 1) == "B")
                {
                    returnValue = "both";
                    stayInLoop = true;
                }
                else
                {
                    Console.WriteLine("Your response was not understood: " + userValue);
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (stayInLoop == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following: sensor(s): " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }

        //
        // This method lets your slect the range type
        //
        static string LightAlarmDisplaySetRangeType()
        {
            string returnValue = null;
            string userValue;
            bool stayInLoop = false;
            // Validate user input string via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("Please enter the range type(minimum or maximum):");
                userValue = Console.ReadLine();
                if (userValue.ToUpper() == "MINIMUM")
                {
                    returnValue = "minimum";
                    stayInLoop = true;
                }
                else if (userValue.ToUpper() == "MAXIMUM")
                {
                    returnValue = "maximum";
                    stayInLoop = true;
                }
                else
                {
                    Console.WriteLine("Your response was not understood: " + userValue);
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (stayInLoop == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following range: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }

        //
        // This method lets you set your time interval for light
        //
        private static int LightAlarmDisplaySetMaximumTimeToMonitor()
        {
            int returnValue;
            bool isNumber = false;
            // Validate user input number via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("Set time to Monitor.");
                Console.WriteLine($"Please enter the time in secords to monitor:");
                Console.WriteLine("");
                isNumber = int.TryParse(Console.ReadLine(), out returnValue);
                if (isNumber == false)
                {
                    Console.WriteLine("Your response was not understood.");
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following monitor time: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }
        //
        // This method sets the light alarms
        //
        static void LightAlarmDisplaySetAlarm(
            Finch finchRobot, 
            string sensorsToMonitor, 
            string rangeType, 
            double minMaxThresholdValue, 
            int timeToMonitor)
        {
            int secondsRun=0;
            bool thresholdMet = false;
            double currentLightValue=0;
            DisplayHeader("Set Alarm");
            Console.WriteLine($"Sensors to monitor: {sensorsToMonitor}");
            Console.WriteLine($"Range Type {rangeType}");
            Console.WriteLine($"minimum/maximum Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"Time to monitor: {timeToMonitor}");
            Console.WriteLine("");
            Console.WriteLine("Please hit any key to start monitoring.");
            DisplayContinuePrompt();
            while (secondsRun < timeToMonitor && thresholdMet == false)
            {
                Console.SetCursorPosition(70, 0);
                Console.WriteLine("Current Clock Count: " + secondsRun);
                currentLightValue = LightAlarmDisplaySensorsCurrent(sensorsToMonitor, finchRobot);
                Console.SetCursorPosition(0, secondsRun + 6);
                switch (rangeType)
                {
                    case "minimum":
                        if (currentLightValue < minMaxThresholdValue)
                        {
                            thresholdMet = true;
                        }
                        break;
                    case "maximum":
                        if (currentLightValue > minMaxThresholdValue)
                        {
                            thresholdMet = true;
                        }
                        break;
                }
                if (thresholdMet == true)
                {
                    Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the current value of:  {currentLightValue}");
                }
                else
                {
                    Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
                }
                finchRobot.wait(1000);
                secondsRun++;
            }
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
        }

        //
        // Get the current light sensor value
        //
        static double LightAlarmDisplaySensorsCurrent(String sensorsToMonitor,Finch finchRobot)
        {
            double currentLightValue=0;
            switch (sensorsToMonitor)
            {
                case "left":
                    currentLightValue = finchRobot.getLeftLightSensor();
                    Console.SetCursorPosition(70, 1);
                    Console.WriteLine("Current left value: " + currentLightValue);
                    break;
                case "right":
                    currentLightValue = finchRobot.getRightLightSensor();
                    Console.SetCursorPosition(70, 1);
                    Console.WriteLine("Current right value: " + currentLightValue);
                    break;
                case "both":
                    currentLightValue = (finchRobot.getRightLightSensor() + finchRobot.getLeftLightSensor()) / 2;
                    Console.SetCursorPosition(70, 1);
                    Console.WriteLine("Current right left avg value: " + currentLightValue);
                    break;
            }
            return currentLightValue;
        }

        //
        // This method is the temperature alarm system menu. 
        //
        static void TemperatureAlarmDisplayMenuScreen(Finch finchRobot)
        {

            bool menuloop = true;
            string userInput;
            String rangeType = "";
            double minMaxThresholdValue = 0;
            int timeToMonitor = 0;
            do
            {
                DisplayHeader("Temperature Alarm Menu..Please Select from the following options:");
                Console.WriteLine("\t\t*******************************************");
                Console.WriteLine();
                Console.WriteLine("\t\t1. Set Range Type");
                Console.WriteLine("\t\t2. Set Maximum/Minimum Threshold Value");
                Console.WriteLine("\t\t3. Set Time to Monitor");
                Console.WriteLine("\t\t4. Set Alarm");
                Console.WriteLine("\t\t5. Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine("\t\t*******************************************");
                userInput = Console.ReadKey().Key.ToString().Substring(1, 1);
                switch (userInput)
                {
                    case "1":
                        rangeType = TemperatureAlarmDisplaySetRangeType();
                        break;
                    case "2":
                        minMaxThresholdValue = TemperatureAlarmDisplaySetMinMaxThresholdValue(rangeType, finchRobot);
                        break;
                    case "3":
                        timeToMonitor = TemperatureAlarmDisplaySetMaximumTimeToMonitor();
                        break;
                    case "4":
                        TemperatureAlarmDisplaySetAlarm(finchRobot, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;
                    case "5":
                        menuloop = false;
                        break;
                    default:
                        break;
                }
            } while (menuloop);
        }


        //
        // This method gets the minimum/maximum Threshold Value for Temperature 
        //
        private static double TemperatureAlarmDisplaySetMinMaxThresholdValue(string rangeType, Finch finchRobot)
        {
            double returnValue;
            bool isNumber = false;
            // Validate user input number via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("minimum/maximum Threshold Value.");
                Console.WriteLine($"The current Temperature is: { ConvertCelsiusToFahrenheit(finchRobot.getTemperature())}");
                Console.WriteLine("");
                Console.WriteLine($"Enter the {rangeType} sensor value: ");
                isNumber = double.TryParse(Console.ReadLine(), out returnValue);
                if (isNumber == false)
                {
                    Console.WriteLine("Your response was not understood.");
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following value: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }
       
        //
        // This method lets your select the range type
        //
        static string TemperatureAlarmDisplaySetRangeType()
        {
            string returnValue = null;
            string userValue;
            bool stayInLoop = false;
            // Validate user input string via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("Please enter the range type(minimum or maximum):");
                userValue = Console.ReadLine();
                if (userValue.ToUpper() == "MINIMUM")
                {
                    returnValue = "minimum";
                    stayInLoop = true;
                }
                else if (userValue.ToUpper() == "MAXIMUM")
                {
                    returnValue = "maximum";
                    stayInLoop = true;
                }
                else
                {
                    Console.WriteLine("Your response was not understood: " + userValue);
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (stayInLoop == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following range: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }

        //
        // This method lets you set your time interval for Temperature
        //
        private static int TemperatureAlarmDisplaySetMaximumTimeToMonitor()
        {
            int returnValue;
            bool isNumber = false;
            // Validate user input number via a bool and a loop. If false stay in loop. If a correct value leave the loop.
            do
            {
                DisplayHeader("Set time to Monitor.");
                Console.WriteLine($"Please enter the time in secords to monitor:");
                Console.WriteLine("");
                isNumber = int.TryParse(Console.ReadLine(), out returnValue);
                if (isNumber == false)
                {
                    Console.WriteLine("Your response was not understood.");
                    Console.WriteLine("Please try again.");
                    Console.WriteLine("Please hit any key to contunue.");
                    DisplayContinuePrompt();
                }
            }
            while (isNumber == false);
            Console.WriteLine();
            Console.WriteLine("You have selected the following monitor time: " + returnValue);
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
            return returnValue;
        }
        //
        // This method sets the Temperature alarms
        //
        static void TemperatureAlarmDisplaySetAlarm(
            Finch finchRobot,
            string rangeType,
            double minMaxThresholdValue,
            int timeToMonitor)
        {
            int secondsRun = 0;
            bool thresholdMet = false;
            double currentTemperatureValue;
            DisplayHeader("Set Alarm");
            Console.WriteLine("Range Type {rangeType}");
            Console.WriteLine($"minimum/maximum Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"Time to monitor: {timeToMonitor}");
            Console.WriteLine("");
            Console.WriteLine("Please hit any key to start monitoring.");
            DisplayContinuePrompt();
            while (secondsRun < timeToMonitor && thresholdMet == false)
            {
                Console.SetCursorPosition(70, 0);
                currentTemperatureValue = ConvertCelsiusToFahrenheit(finchRobot.getTemperature());
                Console.WriteLine("Current Clock Count: " + secondsRun);
                Console.SetCursorPosition(70, 1);
                Console.WriteLine("Current Temperature: " + currentTemperatureValue);
                Console.SetCursorPosition(0, secondsRun +6);
                switch (rangeType)
                {
                    case "minimum":
                        if (currentTemperatureValue < minMaxThresholdValue)
                        {
                            thresholdMet = true;
                        }
                        break;
                    case "maximum":
                        if (currentTemperatureValue > minMaxThresholdValue)
                        {
                            thresholdMet = true;
                        }
                        break;
                }
                if (thresholdMet == true)
                {
                    Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was exceeded by the current value of:  {currentTemperatureValue}");
                }
                else
                {
                    Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was not exceeded.");
                }
                finchRobot.wait(1000);
                secondsRun++;
            }
            Console.WriteLine("Please hit any key to contunue.");
            DisplayContinuePrompt();
        }

        //
        // This method is the user programming menu. It is under development.
        //
        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            DisplayHeader("The User Programming Display is under development. Please hit any key to contunue.");
            DisplayContinuePrompt();
        }

        //
        // This method is the main menu.
        //
        static void DisplayMainMenuScreen(Finch finchRobot)
        {
            //
            // Define variables
            // 
            bool connected;
            bool disconnected;
            bool menuloop = true;
            string userInput;

            do
            {
                DisplayHeader("Main Menu..Please Select from the following options:");
                Console.WriteLine("\t\t*******************************************");
                Console.WriteLine();
                Console.WriteLine("\t\ta) Connect Finch Robot");
                Console.WriteLine("\t\tb) Talent Show");
                Console.WriteLine("\t\tc) Data Recorder");
                Console.WriteLine("\t\td) Alarm System(light)");
                Console.WriteLine("\t\te) Alarm System(temperature)");
                Console.WriteLine("\t\tf) User Programming");
                Console.WriteLine("\t\tg) Disconnect Finch Robot");
                Console.WriteLine("\t\th) Exit");
                Console.WriteLine();
                Console.WriteLine("\t\t*******************************************");
                userInput = Console.ReadKey().Key.ToString().ToLower();
           
                switch (userInput)
                {
                    case "a":
                        connected = DisplayConnectFinchRobot(finchRobot);
                        break;
                    case "b":
                        TalentShowDisplayMenuScreen(finchRobot);
                        break;
                    case "c":
                        DataRecorderDisplayMenuScreen(finchRobot);
                        break;
                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;
                    case "e":
                        TemperatureAlarmDisplayMenuScreen(finchRobot);
                        break;
                    case "f":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;
                    case "g":
                        disconnected = DisplayDisconnectFinchRobot(finchRobot);
                        break;
                    case "h":
                        menuloop = false;
                        break;
                    default:
                        Console.WriteLine("");
                        break;
                }
            } while (menuloop);
            
        

        }

        //
        // This method is the talent show menu.
        //
        static void TalentShowDisplayMenuScreen(Finch finchRobot)
        {
            bool menuloop = true;
            string userInput;

            do
            {
                DisplayHeader("Talent Show Menu..Please Select from the following options:");
                Console.WriteLine("\t\t*******************************************");
                Console.WriteLine();
                Console.WriteLine("\t\t1. Light and Sound");
                Console.WriteLine("\t\t2. Dance");
                Console.WriteLine("\t\t3. Mixing It Up");
                Console.WriteLine("\t\t4. Return to Main Menu");
                Console.WriteLine();
                Console.WriteLine("\t\t*******************************************");
                userInput = Console.ReadKey().Key.ToString().Substring(1,1);
                switch (userInput)
                {
                    case "1":
                        TalentShowDisplayLightAndSound(finchRobot);
                        break;
                    case "2":
                        TalentShowDisplayDance(finchRobot);
                        break;
                    case "3":
                        TalentShowDisplayMixingItUp(finchRobot);
                        break;
                    case "4":
                        menuloop = false;
                        break;
                    default:
                        break;
                }
            } while (menuloop);
        }

        //
        // This method is the talent show light and sound show. 
        //
        static void TalentShowDisplayLightAndSound(Finch finchRobot)
        {
            DisplayHeader("It is now time to be amazed with light and sound.");

            
            //
            // Show lights
            // 
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(1000);
            finchRobot.setLED(0, 0, 0);

            //
            // Play the notes passed to the PlayNotes method with the given wait time
            // 
            PlayNotes(finchRobot, "CCGGAAGFFEEDDCGGFFEEDCCGGAAGFFEEDDCGGFFEED", 200);

            //
            // Play in loop
            // 
            for (int count = 0; count < 12; count++)
            {
                finchRobot.noteOn(261+count*4);
                finchRobot.wait(200);
                finchRobot.noteOff();
            }
            Console.WriteLine("That was cool. Hit any key to continue.");
            DisplayContinuePrompt();
        }

        //
        // This method plays notes when you pass it a finch instance, string of notes, and a duration.
        //
        static void PlayNotes(Finch finchRobot, string notes, int duration)
        {
            int note;

            for (int i = 0; i < notes.Length; i++)
            {
                switch (notes.Substring(i,1))
                {
                    case "C":
                        note = 523;
                        break;
                    case "D":
                        note = 587;
                        break;
                    case "E":
                        note = 659;
                        break;
                    case "F":
                        note = 698;
                        break;
                    case "G":
                        note = 784;
                        break;
                    case "A":
                        note = 880;
                        break;
                    case "B":
                        note = 988;
                        break;
                    default:
                        note = 1048;
                        break;
                }
                finchRobot.noteOn(note);
                finchRobot.wait(duration);
                finchRobot.noteOff();
            }
        }

        //
        // This method commands the Finch Robot to do a little dance with 2 quick turns 
        //
        static void TalentShowDisplayDance(Finch finchRobot)
        {
            DisplayHeader("It is now time to see the robot dance.  Hit any key to continue.");
            DisplayContinuePrompt();
            finchRobot.setMotors(0, 255);
            finchRobot.wait(900);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(255, 0);
            finchRobot.wait(900);
            finchRobot.setMotors(0, 0);
            Console.WriteLine("That robot has the moves. Hit any key to continue.");
            DisplayContinuePrompt();
        }

        //
        // This method commands the Finch Robot to to move, make sounds, and turn on and off lights.
        //
        static void TalentShowDisplayMixingItUp(Finch finchRobot)
        {
            DisplayHeader("It is now time to mix it up.  Hit any key to continue.");
            DisplayContinuePrompt();
            for (int count = 0; count < 12; count++)
            {
                finchRobot.noteOn(261);
                finchRobot.setLED(255, 0, 0);
                finchRobot.setMotors(255, 255);
                finchRobot.wait(200);
                finchRobot.setMotors(0, 0);
                finchRobot.setMotors(-255, -255);
                finchRobot.setLED(0, 0, 255);
                finchRobot.wait(200);
                finchRobot.setMotors(0, 0);
                finchRobot.noteOff();
            }
            finchRobot.setLED(0, 0, 0);
        }
    }
}
