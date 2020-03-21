using System;
using System.Linq;
using ComputerInventory.Data;
using ComputerInventory.Models;

namespace ComputerInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set a color you like other than green or red as this will be used later
            Console.ForegroundColor = ConsoleColor.Black;
            int result = -1;
            while (result != 9)
            {
                result = MainMenu();
            }
        }
        static int MainMenu()
        {
            int result = -1;
            ConsoleKeyInfo cki;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Welcome to Newbie Data Systems");
                WriteHeader("Main Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you would like to do today");
                Console.WriteLine("1. List All Machines in Inventory");
                Console.WriteLine("2. List All Operating Systems");
                Console.WriteLine("3. Data Entry Menu");
                Console.WriteLine("9. Exit");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        //DisplayAllMachines();
                    }
                    else if (result == 2)
                    {
                        //DisplayOperatingSystems();
                    }
                    else if (result == 3)
                    {
                        DataEntryMenu();
                    }
                    else if (result == 9)
                    {
                        // We are exiting so nothing to do
                        cont = true;
                    }
                }
                catch (System.FormatException)
                {
                    // a key that wasn't a number
                }
            } while (!cont);
            return result;
        }
        static void DataEntryMenu()
        {
            ConsoleKeyInfo cki;
            int result = -1;
            bool cont = false;
            do
            {
                Console.Clear();
                WriteHeader("Data Entry Menu");
                Console.WriteLine("\r\nPlease select from the list below for what you  would like to do today");
                Console.WriteLine("1. Add a New Machine");
                Console.WriteLine("2. Add a New Operating System");
                Console.WriteLine("3. Add a New Warranty Provider");
                Console.WriteLine("9. Exit Menu");
                cki = Console.ReadKey();
                try
                {
                    result = Convert.ToInt16(cki.KeyChar.ToString());
                    if (result == 1)
                    {
                        //AddMachine();
                    }
                    else if (result == 2)
                    {
                        AddOperatingSystem();
                    }
                    else if (result == 3)
                    {
                        //AddNewWarrantyProvider();
                    }
                    else if (result == 9)
                    {
                        // We are exiting so nothing to do
                        cont = true;
                    }

                }
                catch (System.FormatException)
                {
                    // a key that wasn't a number
                }
            } while (!cont);
        }

        static void WriteHeader(string headerText)
        {
            Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) +
            headerText.Length / 2) + "}", headerText));
        }
        static bool ValidateYorN(string entry)
        {
            bool result = false;
            if (entry.ToLower() == "y" || entry.ToLower() == "n")
            {
                result = true;
            }
            return result;
        }

        static bool CheckForExistingOS(string osName)
        {
            bool exists = false;
            using (var context = new MachineContext())
            {
                var os = context.OperatingSys.Where(o => o.Name == osName);
                if (os.Count() > 0)
                {
                    exists = true;
                }
            }
            return exists;
        }

        static void AddOperatingSystem()
        {
            Console.Clear();
            ConsoleKeyInfo cki;
            string result;
            bool cont = false;
            OperatingSys os = new OperatingSys();
            string osName = "";
            do
            {
                WriteHeader("Add New Operating System");
                Console.WriteLine("Enter the Name of the Operating System and hit Enter");
                osName = Console.ReadLine();
                if (osName.Length >= 4)
                {
                    cont = true;
                }
                else
                {
                    Console.WriteLine("Please enter a valid OS name of at least 4 characters.\r\nPress and key to continue...");
                Console.ReadKey();
                }
            } while (!cont);
            cont = false;
            os.Name = osName;
            Console.WriteLine("Is the Operating System still supported? [y or n]");
            do
            {
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYorN(result);
            } while (!cont);
            if (result.ToLower() == "y")
            {
                os.StillSupported = true;
            }
            else
            {
                os.StillSupported = false;
            }
            cont = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"You entered {os.Name} as the Operating System Name\r\nIs the OS still supported, you entered {os.StillSupported}.\r\nDo you wish to continue? [y or n]");
                cki = Console.ReadKey();
                result = cki.KeyChar.ToString();
                cont = ValidateYorN(result);
            } while (!cont);
            if (result.ToLower() == "y")
            {
                bool exists = CheckForExistingOS(os.Name);
                if (exists)
                {
                    Console.WriteLine("\r\nOperating System already exists in the database\r\nPress any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    using (var context = new MachineContext())
                    {
                        Console.WriteLine("\r\nAttempting to save changes...");
                        context.OperatingSys.Add(os);
                        int i = context.SaveChanges();
                        if (i == 1)
                        {
                            Console.WriteLine("Contents Saved\r\nPress any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }


            }
        }
    }
}

