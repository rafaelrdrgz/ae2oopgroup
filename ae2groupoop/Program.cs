using System;
using System.Threading;

public class Program
{
    public static void Main(string[] args)
    {
        // centralSystem - this is where all operations will be conducted from
        CentralSystem centralSystem = CentralSystem.Instance;

        // initializing devices
        Light light = new(1, "Living Room Light");
        SecurityCamera camera = new(2, "Front Door Camera", "4K");
        Thermostat thermostat = new(3, "Home Thermostat");

        // adding these devices to the central system
        centralSystem.AddDevice(light);
        centralSystem.AddDevice(camera);
        centralSystem.AddDevice(thermostat);

        // initializing users
        User user1 = new User(1, "Alice");
        User user2 = new User(2, "Bob");

        // adding these users to notification manager
        centralSystem.AddObserver(user1);
        centralSystem.AddObserver(user2);

        // subscribe both users to receive notifications for all devices
        user1.Subscribe(light);
        user1.Subscribe(camera);
        user1.Subscribe(thermostat);
        user2.Subscribe(light);
        user2.Subscribe(camera);
        user2.Subscribe(thermostat);

        // menu system

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("SMART HOME SYSTEM");
            Console.WriteLine("=================");
            Console.WriteLine("1. View Devices");
            Console.WriteLine("2. Control Devices");
            Console.WriteLine("3. View Observers");
            Console.WriteLine("4. Manage Observers");
            Console.WriteLine("5. View Notifications");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");

            // option choice for the user 

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    centralSystem.ListDevices();
                    Pause();
                    break;
                case "2":
                    ControlDevice(centralSystem);
                    break;
                case "3":
                    centralSystem.ListObservers();
                    Pause();
                    break;
                case "4":
                    ManageObservers(centralSystem);
                    break;
                case "5":
                    centralSystem.GetEventHistory();
                    Pause();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Pause();
                    break;
            }
        }
    }

    // method to control the devices for the user

    static void ControlDevice(CentralSystem centralSystem)
    {
        Console.WriteLine("Available Devices:");
        centralSystem.ListDevices();
        Console.Write("Enter Device ID to control: ");
        if (int.TryParse(Console.ReadLine(), out int deviceId))
        {
            var device = centralSystem.Devices.Find(d => d.DeviceID == deviceId);

            // menu for the user for the given device 

            if (device is Light light)


            {
                Console.WriteLine("1. Toggle On/Off");
                Console.WriteLine("2. Set Brightness");
                Console.WriteLine("3. Change Colour");
                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        light.ToggleOnOff();
                        break;
                    case "2":
                        Console.Write("Enter Brightness (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int brightness))
                        {
                            light.SetBrightness(brightness);
                        }
                        break;
                    case "3":
                        Console.Write("Enter Colour: ");
                        string colour = Console.ReadLine();
                        light.SetColour(colour);
                        break;
                }
            }
            else if (device is SecurityCamera camera)
            {
                Console.WriteLine("1. Toggle Recording");
                Console.WriteLine("2. Set Resolution");
                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        camera.ToggleRecording();
                        break;
                    case "2":
                        Console.Write("Enter Resolution (e.g., 4K, 1080p): ");
                        string resolution = Console.ReadLine();
                        camera.SetResolution(resolution);
                        break;
                }
            }
            else if (device is Thermostat thermostat)
            {
                Console.WriteLine("1. Set Temperature");
                Console.WriteLine("2. Set Humidity");
                Console.WriteLine("3. Change Mode");
                string action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        Console.Write("Enter Temperature: ");
                        if (float.TryParse(Console.ReadLine(), out float temp))
                        {
                            thermostat.SetTemperature(temp);
                        }
                        break;
                    case "2":
                        Console.Write("Enter Humidity: ");
                        if (float.TryParse(Console.ReadLine(), out float humidity))
                        {
                            thermostat.SetHumidity(humidity);
                        }
                        break;
                    case "3":
                        Console.Write("Enter Mode (e.g., Cooling, Heating): ");
                        string mode = Console.ReadLine();
                        thermostat.SetMode(mode);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Device not found or unsupported.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Device ID.");
        }

        Pause();
    }

    // manage observers
    static void ManageObservers(CentralSystem centralSystem)
    {
        Console.WriteLine("1. Add Observer");
        Console.WriteLine("2. Remove Observer");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.Write("Enter User ID: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                Console.Write("Enter User Name: ");
                string name = Console.ReadLine();
                User newUser = new User(userId, name);
                centralSystem.AddObserver(newUser);
            }
        }
        else if (choice == "2")
        {
            Console.Write("Enter User ID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                var user = centralSystem.Observers.Find(u => u.UserID == userId);
                if (user != null)
                {
                    centralSystem.RemoveObserver(user);
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
        }

        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}