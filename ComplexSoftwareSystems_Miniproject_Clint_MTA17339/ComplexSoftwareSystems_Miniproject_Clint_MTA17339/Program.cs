using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ComplexSoftwareSystems_Miniproject_Clint_MTA17339
{
    class Client
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient tcpClient = new TcpClient("127.0.0.1", 8989);
                StreamReader reader = new StreamReader(tcpClient.GetStream());
                StreamWriter writer = new StreamWriter(tcpClient.GetStream());

                string s = "";

                while (!s.Equals("Quit"))
                {
                    Console.WriteLine("Enter an animal to send to the Animal Database coorporation inc");
                    s = GetUserDefinedAnimal();
                    Console.WriteLine();
                    Console.WriteLine("Sending the new animal data to the server!");
                    Console.WriteLine();
                    writer.WriteLine(s);
                    writer.Flush();
                    string serverAnswer = reader.ReadLine();
                    Console.WriteLine(serverAnswer);
                }
                reader.Close();
                writer.Close();
                tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string GetUserDefinedAnimal()
        {
            Console.WriteLine("What type of animal would you like to log? \nPress 1 for Biped, Press 2 for Quadroped, press 3 for Auquatic, press 4 for Bird");

            string type = Console.ReadLine();
            string jsonString = "";
            Animal animal = null;

            switch (type)
            { 
                case "1":
                    string animalType1 = "Biped";
                    Animal tempAnimalBiped = GetBaseAnimal(type);
                    float intelligence = ErrorHandleFloat(
                        "Enter an intelligence in numbers for the Biped please",
                        animalType1);
                    Biped animalBiped = new Biped(
                        tempAnimalBiped.name,
                        AnimalType.Biped,
                        tempAnimalBiped.weight,
                        tempAnimalBiped.height,
                        tempAnimalBiped.color,
                        tempAnimalBiped.domesticated,
                        tempAnimalBiped.strength,
                        tempAnimalBiped.carnivore,
                        tempAnimalBiped.herbivore,
                        intelligence
                        );

                    Console.WriteLine("Printing Biped data: \n" +
                        "Name: " + animalBiped.name + "\n" +
                        "Weight: " + animalBiped.weight + "\n" +
                        "Height: " + animalBiped.height + "\n" +
                        "Color: " + animalBiped.color + "\n" +
                        "Domesticated: " + animalBiped.domesticated + "\n" +
                        "Strength: " + animalBiped.strength + "\n" +
                        "Carnivore: " + animalBiped.carnivore + "\n" +
                        "Herbivore: " + animalBiped.herbivore + "\n" +
                        "Intelligence: " + animalBiped.intelligence + "\n");
                    animal = animalBiped;
                    break;

                case "2":
                    string animalType2 = "Quadroped";
                    Animal tempAnimalQuadroped = GetBaseAnimal(type);
                    float runningSpeed = ErrorHandleFloat(
                        "Enter a running speed in numbers for the Quadroped please",
                        animalType2);
                    Quadroped animalQuadroped = new Quadroped(
                        tempAnimalQuadroped.name,
                        AnimalType.Quadroped,
                        tempAnimalQuadroped.weight,
                        tempAnimalQuadroped.height,
                        tempAnimalQuadroped.color,
                        tempAnimalQuadroped.domesticated,
                        tempAnimalQuadroped.strength,
                        tempAnimalQuadroped.carnivore,
                        tempAnimalQuadroped.herbivore,
                        runningSpeed);

                    Console.WriteLine("Printing Quadroped data: \n" +
                        "Name: " + animalQuadroped.name + "\n" +
                        "Weight: " + animalQuadroped.weight + "\n" +
                        "Height: " + animalQuadroped.height + "\n" +
                        "Color: " + animalQuadroped.color + "\n" +
                        "Domesticated: " + animalQuadroped.domesticated + "\n" +
                        "Strength: " + animalQuadroped.strength + "\n" +
                        "Carnivore: " + animalQuadroped.carnivore + "\n" +
                        "Herbivore: " + animalQuadroped.herbivore + "\n" +
                        "Running Speed: " + animalQuadroped.runningSpeed + "\n");
                    animal = animalQuadroped;
                    break;

                case "3":
                    string animalType3 = "Auqatic";
                    Animal tempAnimalAuqatic = GetBaseAnimal(type);
                    float swimSpeed = ErrorHandleFloat(
                        "Enter a swimming speed in numbers for the Auqatic please",
                        animalType3);
                    Auqatic animalAuqatic = new Auqatic(
                        tempAnimalAuqatic.name,
                        AnimalType.Auqatic,
                        tempAnimalAuqatic.weight,
                        tempAnimalAuqatic.height,
                        tempAnimalAuqatic.color,
                        tempAnimalAuqatic.domesticated,
                        tempAnimalAuqatic.strength,
                        tempAnimalAuqatic.carnivore,
                        tempAnimalAuqatic.herbivore,
                        swimSpeed);

                    Console.WriteLine("Printing Auqatic data: \n" +
                        "Name: " + animalAuqatic.name + "\n" +
                        "Weight: " + animalAuqatic.weight + "\n" +
                        "Height: " + animalAuqatic.height + "\n" +
                        "Color: " + animalAuqatic.color + "\n" +
                        "Domesticated: " + animalAuqatic.domesticated + "\n" +
                        "Strength: " + animalAuqatic.strength + "\n" +
                        "Carnivore: " + animalAuqatic.carnivore + "\n" +
                        "Herbivore: " + animalAuqatic.herbivore + "\n" +
                        "Swimming speed: " + animalAuqatic.swimSpeed + "\n");
                    animal = animalAuqatic;
                    break;

                case "4":
                    string animalType4 = "Bird";
                    Animal tempAnimalBird = GetBaseAnimal(type);
                    float wingSpan = ErrorHandleFloat(
                        "Enter a wingspan in numbers for the Bird please",
                        animalType4);
                    float maxAltitude = ErrorHandleFloat(
                        "Enter a maximum altitude in numbers for the Bird please",
                        animalType4);
                    Bird animalBird = new Bird(
                        tempAnimalBird.name,
                        AnimalType.Bird,
                        tempAnimalBird.weight,
                        tempAnimalBird.height,
                        tempAnimalBird.color,
                        tempAnimalBird.domesticated,
                        tempAnimalBird.strength,
                        tempAnimalBird.carnivore,
                        tempAnimalBird.herbivore,
                        wingSpan,
                        maxAltitude);

                    Console.WriteLine("Printing Bird data: \n" +
                        "Name: " + animalBird.name + "\n" +
                        "Weight: " + animalBird.weight + "\n" +
                        "Height: " + animalBird.height + "\n" +
                        "Color: " + animalBird.color + "\n" +
                        "Domesticated: " + animalBird.domesticated + "\n" +
                        "Strength: " + animalBird.strength + "\n" +
                        "Carnivore: " + animalBird.carnivore + "\n" +
                        "Herbivore: " + animalBird.herbivore + "\n" + 
                        "Wingspan: " + animalBird.wingSpan + "\n" +
                        "Maximum Altitude: "+ animalBird.maxAltitude + "\n");
                    animal = animalBird;   
                    break;

                default:
                    break;
            }

            jsonString = JsonConvert.SerializeObject(animal);

            if(jsonString != null)
            {
                Console.WriteLine("Serialized animal sucessfully");
            }

            return jsonString;
        }

        private static Animal GetBaseAnimal(string type)
        {
            string animalType = "";

            switch (type)
            {
                case "1":
                    animalType = "Biped";
                    break;
                case "2":
                    animalType = "Quadroped";
                    break;
                case "3":
                    animalType = "Auqatic";
                    break;
                case "4":
                    animalType = "Bird";
                    break;
                default:
                    animalType = "Animal";
                    break;
            }

            Console.WriteLine("Enter a name for the " + animalType + " please");
            string tempName = Console.ReadLine();
            float tempWeight = ErrorHandleFloat(
                "Enter a weight in numbers for the " + animalType + " please", 
                animalType);
            float tempHeight = ErrorHandleFloat(
                "Enter a height in numbers for the " + animalType + " please", 
                animalType);
            Console.WriteLine("Enter a color for the " + animalType + " please");
            string tempColor = Console.ReadLine();
            bool tempDomesticated = ErrorHandleBool(
                "Is the " + animalType + " domesticated? Enter 1 if yes, 2 if no", 
                animalType);
            float tempStrength = ErrorHandleFloat(
                "Enter a strength in numbers for the " + animalType + "please", 
                animalType);
            bool tempCarnivore = ErrorHandleBool(
                "Is the " + animalType + " a carnivore? Enter 1 if yes, 2 if no", 
                animalType);
            bool tempHerbivore = ErrorHandleBool(
                "Is the " + animalType + " a herbivore? Enter 1 if yes, 2 if no", 
                animalType);

            Animal tempAnimal = new Animal(
                tempName,
                tempWeight,
                tempHeight,
                tempColor,
                tempDomesticated,
                tempStrength,
                tempCarnivore,
                tempHerbivore);

            return tempAnimal;
        }

        // recursive error handling
        private static float ErrorHandleFloat(string consolePrompt, string animalType)
        {
            float tempWeight;
            Console.WriteLine(consolePrompt);
            string stringWeight = Console.ReadLine();

            if (!float.TryParse(stringWeight, out tempWeight))
            {
                Console.WriteLine("Error! Try Again");
                tempWeight = ErrorHandleFloat(consolePrompt, animalType);
            }

            return tempWeight;
        }

        private static bool ErrorHandleBool(string cosolePrompt, string animalType)
        {
            bool tempBool;

            Console.WriteLine(cosolePrompt);
            string tempString = Console.ReadLine();

            switch (tempString)
            {
                case "1":
                    tempBool = true;
                    break;

                case "2":
                    tempBool = false;
                    break;

                default:
                    tempBool = ErrorHandleBool(cosolePrompt, animalType);
                    break;
            }

            return tempBool;
        }
    }

    public enum AnimalType { Biped, Quadroped, Auqatic, Bird }

    class Animal
    {
        public string name { get; set; }
        public AnimalType type { get; set; }
        public float weight { get; set; }
        public float height { get; set; }
        public string color { get; set; }
        public bool domesticated { get; set; }
        public float strength { get; set; }
        public bool carnivore { get; set; }
        public bool herbivore { get; set; }

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
