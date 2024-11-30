public class Program
{
    public static void Main(string[] args)
    {
        // centralSystem - this is where all operations will be conducted from
        CentralSystem centralSystem = CentralSystem.Instance;

        // initialising devices
        Light light = new(1, "Living Room Light");
        SecurityCamera camera = new(2, "Front Door Camera", "4K");
        Thermostat thermostat = new(3, "Home Thermostat");

        // adding these devices to the central system
        centralSystem.AddDevice(light);
        centralSystem.AddDevice(camera);
        centralSystem.AddDevice(thermostat);

        // users
        User user1 = new User(1, "aldjaslkdj");
        User user2 = new User (2, "BOB");

        // adding these users to notification manager
        centralSystem.AddObserver(user1);
        centralSystem.AddObserver(user2);

        // turn camera off, which will notify observers but keep status the same (camera is off by default)
        camera.ToggleRecording();
        camera.ToggleRecording();

        Thread.Sleep(1000);

        // increase thermostat by 10 degrees, which notifies observers
        thermostat.SetTemperature(10);

        Thread.Sleep(1000);

        // increase thermostat humidity by 25%, which notifies observers
        thermostat.SetHumidity(25);

        // turn light on, changing status
        light.ToggleOnOff();

        // while loop that shows device lists and statuses as well as ALL notifications (not just ones observers have opted into)
        while(true) {
            Console.Clear();
            Console.WriteLine("SYSTEM VIEW");
            Console.WriteLine("============");
            Console.WriteLine("Devices:");
            Console.WriteLine("============");
            centralSystem.ListDevices();
            Console.WriteLine("============");
            centralSystem.GetEventHistory();
            Console.WriteLine("============");
            Thread.Sleep(1000);
        }
    }
}