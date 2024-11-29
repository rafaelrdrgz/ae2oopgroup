class Program //implement the notimplementedexception pls !!!
{

    static void Main(string[] args){
        ControlSystemManager controlManager = ControlSystemManager.Instance;

        KitchenLight kitchenLight1 = new KitchenLight("kl01", true, 5);
        controlManager.AddDevice(kitchenLight1);

        User user = new User("User001");
        SecuritySystem securitySystem = new SecuritySystem("Main Security");

        controlManager.AddObs(user);
        controlManager.AddObs(securitySystem);

        kitchenLight1.SetBrightnessLvl(7);

    }
    abstract class Singleton<T> where T : class{
        private static T _Instance = null!;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = (T)Activator.CreateInstance(typeof(T), true);
                return _Instance;
            }
        }
    }
    interface IObserver{
        void Alert(string message);
    }

    class User : IObserver{ //CONRETE OBSERVER 1, RECEIVE FROM OBSERVABLE
        public string UserID;

        public User(string userid)
        {
            UserID = userid;
        }

        public void Alert(string msg)
        {
            Console.WriteLine($"Sending User:{UserID} alert: {msg}");
        }
    }

    class SecuritySystem : IObserver{ //CONRETE OBSERVER 2
        public string SystemType;
        public SecuritySystem(string systemType)
        {
            SystemType = systemType;


        }
        public void Alert(string msg)
        {
            Console.WriteLine($"Security system was alerted because: {msg}");
        }
    }

    class ControlSystemManager : Singleton<ControlSystemManager>{
        private List<Device> devices;
        private List<IObserver> observers;
        private ControlSystemManager()
        {
            devices = new List<Device>();
            observers = new List<IObserver>();
        }
        public void AddDevice(Device device)
        {
            devices.Add(device);
            Console.WriteLine("device added bruh");
        }
        public void removeDevice(Device device)
        {
            devices.Remove(device);
            Console.WriteLine("device removed idk lol");
        }
        public void AddObs(IObserver observer)
        {
            observers.Add(observer);
            Console.WriteLine("new obserever");
        }
        public void RemoveObs(IObserver observer)
        {
            observers.Remove(observer);
            Console.WriteLine("gone ");
        }
        public void somethingObserved(string msg)
        { //goes thru each observer and alerts it when something is observed 
            foreach (var observer in observers)
            {
                observer.Alert(msg); //doesnt need to know the specific observer classes, just implement the interface !!
            }
        }

    }

    abstract class Device{ //abstract observabel, idk add more here too, 
        public string DeviceID;
        public bool Connected;

        public Device(string deviceid, bool connected)
        {
            DeviceID = deviceid;
            Connected = connected;
        }
        public abstract void NotifyObservers();
    }

    class KitchenLight : Device{ //CONCRETE OBSERVABLE EXAMPLE================================
        private int brightness;
        public KitchenLight(string deviceid, bool connected, int brightness) : base(deviceid, connected)
        {
            this.brightness = brightness;
        }
        public void SetBrightnessLvl(int newLvl)
        {
            brightness = newLvl;
            NotifyObservers();
        }
        public override void NotifyObservers()
        {
            throw new NotImplementedException();
        }
    }
    //                  ¦
    //other observables v


}//PRGOGRAM END 