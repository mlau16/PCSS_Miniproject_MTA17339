using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComplexSoftwareSystems_Miniproject_MTA17339
{
    // Server Class
    class Server
    {
        static void Main(string[] args)
        {
            TcpListener tcplistener = null;
            // Try to start the server, and wait for client to connect. Will throw exception if fails
            try
            {
                tcplistener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8989);
                tcplistener.Start();
                Console.WriteLine("Server is listening for Animal connossieurs");
                Thread readThread = new Thread(CommandPrintDatabase);
                readThread.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for Animal connossieurs to connect!");
                    TcpClient tcpClient = tcplistener.AcceptTcpClient();
                    Console.WriteLine("An Animal connossieur has joined us!");
                    // Start thread for each client connected, with function ProcessClients
                    Thread t = new Thread(ProcessClients);
                    t.Start(tcpClient);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (tcplistener != null)
                {
                    tcplistener.Stop();
                }
            }
        }
        
        // Function that process the clients.
        private static void ProcessClients(object arg)
        {
            TcpClient client = (TcpClient)arg;
            // Try to create a reader and a writer for the stream
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                string s = "";
                // while the string s is not equal to "Quit" or null, the server handles the string from the stream
                while (!(s = reader.ReadLine()).Equals("Quit") || (s == null))
                {
                    HandleJsonMessage(s);
                    writer.WriteLine("From Server: Thank you for the contribution to the Animal Database");
                    writer.Flush();
                }
                
                // Closes the different objects
                reader.Close();
                writer.Close();
                client.Close();
                Console.WriteLine("Goodbye Client");
            }
            catch (IOException e)
            {
                Console.WriteLine("Problem with the Tread, Closing. Bye!");
                throw;
            }
            // Close client after exception
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }
        // Function which Prints from a database file (Json file)
        private static void CommandPrintDatabase()
        {
            while (true)
            {
                string compare;

                if ((compare = Console.ReadLine()).Equals("print"))
                {
                    Console.WriteLine("Known Command: print");
                    // Tries to load database file, and if the file exists it will continue
                    if (File.Exists(".\\Animal_Database.json"))
                    {
                        // Write that it loads file
                        Console.WriteLine("Loading File");
                        // Reads all the text from the file
                        string jsonString = File.ReadAllText(".\\Animal_Database.json");
                        //Convert text to JsonArray
                        var jArr = JArray.Parse(jsonString);
                        // Loops through Jarray elements
                        for (int i = 0; i < jArr.Count; i++)
                        {
                            // Reads the type of each Json element
                            string strType = (string)jArr[i]["type"];
                            AnimalType type;
                            // Check if type can be parsed to an enum
                            if (Enum.TryParse(strType, out type))
                            {
                                // Switch statement for each type, which creates the object and then prints it.
                                switch (type)
                                {
                                    case AnimalType.Biped:
                                        Biped tempBiped = jArr[i].ToObject<Biped>();
                                        PrintBiped(tempBiped);
                                        break;

                                    case AnimalType.Quadroped:
                                        Quadroped tempQuadroped = jArr[i].ToObject<Quadroped>();
                                        PrintQuadroped(tempQuadroped);
                                        break;

                                    case AnimalType.Auqatic:
                                        Auqatic tempAuqatic = jArr[i].ToObject<Auqatic>();
                                        PrintAuqatic(tempAuqatic);
                                        break;

                                    case AnimalType.Bird:
                                        Bird tempBird = jArr[i].ToObject<Bird>();
                                        PrintBird(tempBird);
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }

                        if (jArr.Count == 0)
                        {
                            Console.WriteLine("Failed to unpack Animals Database");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Database exists... Wait for clients to log animals");
                    }
                }
                else
                {
                    Console.WriteLine("Unknown Command");
                }
            }
        }
        // Handles Json message
        private static void HandleJsonMessage(string message)
        {
            // Checks if the string is a valid Json string. If it is valid, it can be parsed to a Json Object
            if (IsValidJson(message))
            {
               
                var jObject = JObject.Parse(message);
                // Parsing to Json Object
                string strType = (string)jObject["type"];
                AnimalType type;
                Animal animal = null;
                
                // Try to parse the type from string to enum
                if (Enum.TryParse(strType, out type))
                {
                    // Create and print object based on the type
                    switch (type)
                    {
                        case AnimalType.Biped:
                            Biped animalBiped = JsonConvert.DeserializeObject<Biped>(message);
                            PrintBiped(animalBiped);
                            animal = animalBiped;
                            break;

                        case AnimalType.Quadroped:
                            Quadroped animalQuadroped = JsonConvert.DeserializeObject<Quadroped>(message);
                            PrintQuadroped(animalQuadroped);
                            animal = animalQuadroped;
                            break;

                        case AnimalType.Auqatic:
                            Auqatic animalAuqatic = JsonConvert.DeserializeObject<Auqatic>(message);
                            PrintAuqatic(animalAuqatic);
                            animal = animalAuqatic;
                            break;

                        case AnimalType.Bird:
                            Bird animalBird = JsonConvert.DeserializeObject<Bird>(message);
                            PrintBird(animalBird);
                            animal = animalBird;
                            break;

                        default:
                            animal = null;
                            break;
                    }
                }
                else
                {
                    // we done goofd
                    Console.WriteLine("Something went wrong with the new animal :(");
                }

                if (animal != null)
                {
                    if (File.Exists(".\\Animal_Database.json"))
                    {
                        // concatinate json string to existing file
                        string existingDatabase = File.ReadAllText(".\\Animal_Database.json");
                        // Deserialize json file to list of animals
                        List<Animal> existingAnimals = JsonConvert.DeserializeObject<List<Animal>>(existingDatabase);
                        // Create new animal based on the message json string
                        Animal newAnimal = JsonConvert.DeserializeObject<Animal>(message);
                        // Add animal to existingAnimals list
                        existingAnimals.Add(newAnimal);

                        // Serialize lists to string
                        string animalDatabase = JsonConvert.SerializeObject(existingAnimals);
                        // Write string to file
                        File.WriteAllText(".\\Animal_Database.json", animalDatabase);
                        Console.WriteLine("Saved Animal database");
                    }
                    else
                    {
                        // Check if its the first animal created. 
                        Animal firstAnimal = JsonConvert.DeserializeObject<Animal>(message);
                        List<Animal> firstAnimalList = new List<Animal>();
                        firstAnimalList.Add(firstAnimal);

                        string firstAnimalDatabase = JsonConvert.SerializeObject(firstAnimalList);
                        File.WriteAllText(".\\Animal_Database.json", firstAnimalDatabase);
                        Console.WriteLine("Saved Animal database");
                    }
                }
            }
        }

        private static bool IsValidJson(string strInput)
        {
            // Removes whitespace in front of string, or in the back
            strInput = strInput.Trim();
            
            // Looks for brackets
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                // Try to parse json string, which throws exception if it cant. 
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        // Different print formatting for each Biped
        private static void PrintBiped(Biped biped)
        {
            Console.WriteLine("Printing Biped data: \n" +
                                "Name: " + biped.name + "\n" +
                                "Weight: " + biped.weight + "\n" +
                                "Height: " + biped.height + "\n" +
                                "Color: " + biped.color + "\n" +
                                "Domesticated: " + biped.domesticated + "\n" +
                                "Strength: " + biped.strength + "\n" +
                                "Carnivore: " + biped.carnivore + "\n" +
                                "Herbivore: " + biped.herbivore + "\n" +
                                "Intelligence: " + biped.intelligence + "\n");
        }
        // Different print formatting for each Quadroped
        private static void PrintQuadroped(Quadroped quadroped)
        {
            Console.WriteLine("Printing Quadroped data: \n" +
                                "Name: " + quadroped.name + "\n" +
                                "Weight: " + quadroped.weight + "\n" +
                                "Height: " + quadroped.height + "\n" +
                                "Color: " + quadroped.color + "\n" +
                                "Domesticated: " + quadroped.domesticated + "\n" +
                                "Strength: " + quadroped.strength + "\n" +
                                "Carnivore: " + quadroped.carnivore + "\n" +
                                "Herbivore: " + quadroped.herbivore + "\n" +
                                "Running Speed: " + quadroped.runningSpeed + "\n");
        }
        // Different print formatting for each Auqatic
        private static void PrintAuqatic(Auqatic auqatic)
        {
            Console.WriteLine("Printing Auqatic data: \n" +
                               "Name: " + auqatic.name + "\n" +
                               "Weight: " + auqatic.weight + "\n" +
                               "Height: " + auqatic.height + "\n" +
                               "Color: " + auqatic.color + "\n" +
                               "Domesticated: " + auqatic.domesticated + "\n" +
                               "Strength: " + auqatic.strength + "\n" +
                               "Carnivore: " + auqatic.carnivore + "\n" +
                               "Herbivore: " + auqatic.herbivore + "\n" +
                               "Swimming speed: " + auqatic.swimSpeed + "\n");
        }
        // Different print formatting for each Bird
        private static void PrintBird(Bird bird)
        {
            Console.WriteLine("Printing Bird data: \n" +
                              "Name: " + bird.name + "\n" +
                              "Weight: " + bird.weight + "\n" +
                              "Height: " + bird.height + "\n" +
                              "Color: " + bird.color + "\n" +
                              "Domesticated: " + bird.domesticated + "\n" +
                              "Strength: " + bird.strength + "\n" +
                              "Carnivore: " + bird.carnivore + "\n" +
                              "Herbivore: " + bird.herbivore + "\n" +
                              "Wingspan: " + bird.wingSpan + "\n" +
                              "Maximum Altitude: " + bird.maxAltitude + "\n");
        }
    }

    // Enum for each animal type
    public enum AnimalType { Biped, Quadroped, Auqatic, Bird }

    // Class for animal
    class Animal
    {
        public string name { get; set; }
        public float weight { get; set; }
        public float height { get; set; }
        public string color { get; set; }
        public bool domesticated { get; set; }
        public float strength { get; set; }
        public bool carnivore { get; set; }
        public bool herbivore { get; set; }
        public AnimalType type { get; set; }

        public Animal(
            string name,
            float weight,
            float height,
            string color,
            bool domesticated,
            float strength,
            bool carnivore,
            bool herbivore)
        {
            this.name = name;
            this.weight = weight;
            this.height = height;
            this.color = color;
            this.domesticated = domesticated;
            this.strength = strength;
            this.carnivore = carnivore;
            this.herbivore = herbivore;
        }
    }
    // Subclass from animal for Biped
    class Biped : Animal
    {
        public float intelligence { get; set; }

        public Biped(
            string name,
            AnimalType type,
            float weight,
            float height,
            string color,
            bool domesticated,
            float strength,
            bool carnivore,
            bool herbivore,
            float intelligence
            ) : base(name, weight, height, color, domesticated, strength, carnivore, herbivore)
        {
            this.intelligence = intelligence;
            this.type = type;
        }
    }
    // Subclass from animal for Quadroped
    class Quadroped : Animal
    {
        public float runningSpeed { get; set; }

        public Quadroped(
            string name,
            AnimalType type,
            float weight,
            float height,
            string color,
            bool domesticated,
            float strength,
            bool carnivore,
            bool herbivore,
            float runningSpeed
            ) : base(name, weight, height, color, domesticated, strength, carnivore, herbivore)
        {
            this.runningSpeed = runningSpeed;
            this.type = type;
        }
    }
      // Subclass from animal for Auqatic
    class Auqatic : Animal
    {
        public float swimSpeed { get; set; }

        public Auqatic(
            string name,
            AnimalType type,
            float weight,
            float height,
            string color,
            bool domesticated,
            float strength,
            bool carnivore,
            bool herbivore,
            float swimSpeed
            ) : base(name, weight, height, color, domesticated, strength, carnivore, herbivore)
        {
            this.swimSpeed = swimSpeed;
            this.type = type;
        }
    }
      // Subclass from animal for Bird
    class Bird : Animal
    {
        public float wingSpan { get; set; }
        public float maxAltitude { get; set; }

        public Bird(
            string name,
            AnimalType type,
            float weight,
            float height,
            string color,
            bool domesticated,
            float strength,
            bool carnivore,
            bool herbivore,
            float wingSpawn,
            float maxAltitude
            ) : base(name, weight, height, color, domesticated, strength, carnivore, herbivore)
        {
            this.wingSpan = wingSpan;
            this.maxAltitude = maxAltitude;
            this.type = type;
        }
    }
}

