// central system class - this class is the singleton
public class CentralSystem {
    // create centralsystem as an instance
    public static CentralSystem _Instance;
    public List<SmartDevice> Devices { get; private set;}
    public List<User> Observers { get; private set; }
    private List<string> Events;

    // now get instance
    public static CentralSystem Instance {
        get {
            if (_Instance == null) {
                _Instance = new();
            }
            return _Instance;
        }
    }

    // contain all devices, observers, and events (notifications sent to some, all, or no observers) within the central system
    private CentralSystem() {
        Devices = new();
        Observers = new();
        Events = new();
    }

    // add a new device
    public void AddDevice(SmartDevice device) {
        Devices.Add(device);
        Console.WriteLine($"{device.Name} added to the system.");
    }

    //  remove an existing device
    public void RemoveDevice(SmartDevice device) {
        Devices.Remove(device);
        Console.WriteLine($"{device.Name} removed from the system.");
    }

    // list current devices and current device statuses
    public void ListDevices() {
        foreach (var device in Devices) {
            // lists a device's status (and date of that status) if available
            string status = device.Status;
            if(status == null) {
                Console.WriteLine($"{device.DeviceID} | {device.Name} | Status: N/A");
            } else {
                Console.WriteLine($"{device.DeviceID} | {device.Name} | Status: ({device.LastStatusUpdate}) {status}");
            }
        }
    }

    // list current users
    public void ListObservers() {
        foreach (var user in Observers) {
            // lists each user's name, ID, and notification history
            Console.WriteLine($"{user.UserID} | {user.Name}");
            Console.WriteLine("----------------------------");
            user.GetNotificationHistory();
            Console.WriteLine("----------------------------");
        }
    }

    // add a new observer
    public void AddObserver(User user) {
        Observers.Add(user);
        Console.WriteLine($"{user.Name} added as an observer.");
    }

    // remove existing observer
    public void RemoveObserver(User user) {
        Observers.Remove(user);
        Console.WriteLine($"{user.Name} removed as an observer.");
    }

    // notify all current subscribers of a particular device's update
    public void NotifyUsers(string message, SmartDevice device) {
        Events.Add(message);
        foreach (var user in Observers) {
            user.SendNotification(message, device);
        }
    }

    // view previous events
    public void GetEventHistory() {
        foreach (var evt in Events) {
            Console.WriteLine($"{evt}");
        }
    }
}

// user class
public class User {
    // initialising generic user information here
    public int UserID { get; }
    public string Name { get; }
    public string ContactInfo { get; set; }
    private List<string> Notifications = new();
    private List<SmartDevice> Subscriptions = new();

    // initialise user
    public User(int userID, string name) {
        UserID = userID; Name = name;
    }

    // send notification for a particular user
    public void SendNotification(string message, SmartDevice device) {
        // check that user is subscribed to notifications from that device
        if(Subscriptions.Contains(device)) {
            Notifications.Add(message);
        }
    }

    // subscribe a user to receive notifications from a particular device
    public void Subscribe(SmartDevice device) {
        Subscriptions.Add(device);
        Console.WriteLine($"{Name} will now receive notifications about {device.Name} ({device.DeviceID}).");
    }

    // view previous notifications
    public void GetNotificationHistory() {
        foreach (var not in Notifications) {
            Console.WriteLine($"{not}");
        }
    }
}

// smartdevice base class for each device
public abstract class SmartDevice {
    // generic device information
    public int DeviceID { get; protected set; }
    public string Name { get; protected set; }
    public string Status { get; protected set; }
    public DateTime LastStatusUpdate { get; protected set; }

    // initialise device
    public SmartDevice(int id, string name) {
        DeviceID = id; Name = name; Status = "Off"; LastStatusUpdate = DateTime.Now;
    }

    // change status and set last status update to current time for any device using this
    public virtual void SetStatus(string status) {
        Status = status;
        LastStatusUpdate = DateTime.Now;
        Console.WriteLine($"Status of {Name} updated.");
    }

    // notify observers of a particular provided event, and show the time of the event and the device sending it
    protected virtual void NotifyObservers(string message) {
        CentralSystem.Instance.NotifyUsers($"{DateTime.Now} | {Name}: {message}", this);
    }
}

// light as a derived class of smartdevice established with its given features
// brightness and colour as settings and methods that can be configured by the user
// option to notify observer if subscribed to notifications
public class Light : SmartDevice, ILightBulb {
    public int Brightness { get; private set; }
    public string Colour { get; private set; }
    public Boolean IsOn { get; private set; }

    public Light(int id, string name) : base(id, name) {
        Brightness = 0; Colour = "White"; SetStatus("Off");
    }

    // set lightbulb brightness
    public void SetBrightness(int brightness) {
        Brightness = brightness;
        Console.WriteLine($"Brightness set to {Brightness}.");
        NotifyObservers("Brightness adjusted.");
    }

    // set lightbulb colour
    public void SetColour(string colour) {
        Colour = colour;
        Console.WriteLine($"Colour changed to {Colour}.");
        NotifyObservers("Colour updated.");
    }

    // toggle light on or off
    public void ToggleOnOff() {
        IsOn = !IsOn;
        if (IsOn) {
            SetStatus("On");
            NotifyObservers("Light turned on.");
        } else {
            SetStatus("Off");
            NotifyObservers("Light turned off.");
        }
    }
}

// interface for any given light bulb, ensures there's a function for brightness, colour, and turning it on/off
public interface ILightBulb {
    public void SetBrightness( int brightness);
    public void SetColour(string colour);
    public void ToggleOnOff();
}

// securitycamera as a derived class of smartdevice established with its given features
// resolution can be set by the user
// isrecording is configured by the startrecording() and stoprecording() methods
// option to receive camera feed and gallery
// notify the observer if subscribed to notifications
public class SecurityCamera : SmartDevice, ICameraRecording {
    public string Resolution { get; private set; }
    public Boolean IsRecording { get; private set; }

    public SecurityCamera(int id, string name, string resolution) : base(id, name) {
        Resolution = resolution;
    }

    // set camera resolution, update status as needed
    public void SetResolution(string resolution) {
        Resolution = resolution;
        Console.WriteLine($"Resolution set to {Resolution}.");
        NotifyObservers("Resolution updated.");
    }

    // change camera from recording to not recording and vice versa, and update status accordingly
    public void ToggleRecording() {
        IsRecording = !IsRecording;
        if (IsRecording) {
            SetStatus($"Recording at {Resolution}");
            NotifyObservers("Recording started.");
        } else {
            SetStatus("Not recording");
            NotifyObservers("Recording stopped.");
        }
    }
}

// interface for any type camera, ensures there's a method to turn it on/off
public interface ICameraRecording {
    public void ToggleRecording();
}

// thermostat as a derived class of smartdevice established with its given features
// temperature, humidity, and mode as settings and methods that can be configured by the user
// observer will be notified if subscribed to notifications for the device and notification is sent
public class Thermostat : SmartDevice, ITemperatureManagement{
    public float Temperature { get; private set; }
    public float Humidity { get; private set; }
    public string Mode { get; private set; }

    public Thermostat(int id, string name) : base(id, name) {
        Temperature = 22.0f; Humidity = 40.0f; Mode = "Cooling";
        SetStatus($"Temperature: {Temperature}°C, Humidity: {Humidity}%, Mode: {Mode}");
    }

    // change temperature, change status to reflect this, if temperature changes, notify observers
    public void SetTemperature(float newTemp) {
        Temperature = newTemp;
        SetStatus($"Temperature: {Temperature}°C, Mode: {Mode}");
        NotifyObservers("Temperature updated.");
    }

    // change humidity, if humidity changed, notify observers
    public void SetHumidity(float newHumidity) {
        Humidity = newHumidity;
        Console.WriteLine($"Humidity set to {Humidity}%.");
        NotifyObservers("Humidity updated.");
    }

    // change mode, set status to reflect this, notify observers of mode change
    public void SetMode(string mode) {
        Mode = mode;
        Console.WriteLine($"Mode set to {Mode}.");
        NotifyObservers($"Mode changed to {Mode}.");
    }
}

// interface for any given thermostat, ensures it has ability to set temperature, humidity, and mode
public interface ITemperatureManagement {
    public void SetTemperature(float temperature);
    public void SetHumidity(float humidity);
    public void SetMode(string mode);
}