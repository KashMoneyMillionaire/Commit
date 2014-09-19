using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Created By: Clay Howell
//cfh101020

namespace HuntTheWumpus
{
    public enum GameObjectType
    {
        Wumpus,
        Bat,
        Pit,
        None
    }

    class Game
    {
        //Random Number Generator
        public static Random rand;
        public static int seed;

        //End of game
        public static bool end;

        static void Main(string[] args)
        {
            bool same = false;
            bool cont = true;
            while (cont)
            {
                if (!same)
                    seed = (new Random()).Next(10000);
                rand = new Random(seed);

                Console.WriteLine("\n\tWelcome to Hunt the Wumpus");
                StartGame();

                string input = "";
                while (!input.ToUpper().Contains('Y') && !input.ToUpper().Contains('N'))
                {
                    Console.Write("Again? (y/n): ");
                    input = Console.ReadLine();
                }

                if (input.ToUpper().Contains('Y'))
                {
                    input = "";
                    while (!input.ToUpper().Contains('Y') && !input.ToUpper().Contains('N'))
                    {
                        Console.Write("Same Map? (y/n): ");
                        input = Console.ReadLine();
                    }

                    if (input.ToUpper().Contains('Y'))
                        same = true;
                    else
                        same = false;
                }
                else
                    cont = false;
            }
        }

        static void StartGame()
        {
            Game.end = false;

            //Create map and GameObjects
            Map map = new Map();
            map.Add(new Wumpus());
            map.Add(new Bat());
            map.Add(new Pit(2));

            //Spawn Player
            Player player = new Player();

            //Game Loop
            while(!Game.end)
            {
                //Blank Line
                Console.WriteLine();

                Wumpus.Move();

                //Check Hazards
                if (Map.Check(Player.location) == GameObjectType.Wumpus)
                    Console.WriteLine("THE WUMPUS IS HERE! RUN!");
                else
                {
                    bool once = false;
                    GameObjectType[] hazards = Map.CheckAdjacent(Player.location);
                    foreach (GameObjectType hazard in hazards)
                    {
                        switch (hazard)
                        {
                            case GameObjectType.Wumpus:
                                Console.WriteLine("I smell a Wumpus...");
                                break;
                            case GameObjectType.Bat:
                                Console.WriteLine("I hear bats...");
                                break;
                            case GameObjectType.Pit:
                                if(!once)
                                    Console.WriteLine("It's drafty...");
                                once = true;
                                break;
                            default:
                                break;
                        }
                    }
                }

                //Get user action
                string input = "";
                while(!input.ToUpper().Contains('M') && !input.ToUpper().Contains('S'))
                {
                    Console.WriteLine("Current Room: " + Player.location);
                    Console.WriteLine("Adjacent Rooms: " + Map.AdjacentString(Player.location));
                    Console.Write("Type (M) to Move or (S) to Shoot: ");
                    input = Console.ReadLine();
                }

                //Move
                if(input.ToUpper().Contains('M'))
                {
                    int moveTo = -1;
                    while (moveTo == -1)
                    {
                        Console.Write("Type a room number: ");
                        input = Console.ReadLine();
                        Int32.TryParse(input, out moveTo);
                        if (Map.isAdjacent(Player.location,moveTo))
                            Player.location = moveTo;
                        else
                        {
                            Console.WriteLine("\nBad Room Input");
                            moveTo = -1;
                        }
                    }
                }
                //Shoot
                else
                {
                    Player.arrows--;
                    bool correct = false;
                    while(!correct)
                    {
                        Console.Write("Enter the arrows path: ");
                        correct = map.FireArrow(Player.location, Console.ReadLine());
                        if(!correct)
                            Console.WriteLine("\nBad Path.");
                    }

                    if (!Game.end && Player.arrows == 0)
                    {
                        Console.WriteLine("That was your last arrow. You're doomed.");
                        Game.end = true;
                    }
                }

                //End turn check room
                if (!Game.end)
                {
                    switch (Map.Check(Player.location))
                    {
                        case GameObjectType.Wumpus:
                            if (Wumpus.awake)
                            {
                                Console.WriteLine("EATEN BY THE WUMPUS");
                                Game.end = true;
                            }
                            else
                            {
                                Console.WriteLine("YOU'VE AWOKEN THE WUMPUS!");
                                Wumpus.awake = true;
                                Wumpus.Move();
                            }
                            break;
                        case GameObjectType.Pit:
                            Console.WriteLine("You fall down a pit to your death.");
                            Game.end = true;
                            break;
                        case GameObjectType.Bat:
                            Console.WriteLine("AHH BATS! You sprint away madly...");
                            //Another random room
                            Player.location = (Player.location + Game.rand.Next(19)+1) % 20;
                            if (Map.Check(Player.location) == GameObjectType.Wumpus)
                            {
                                if (Wumpus.awake)
                                {
                                    Console.WriteLine("...AND RUN INTO THE WUMPUS. DEAD MEAT!");
                                    Game.end = true;
                                }
                                else
                                {
                                    Console.WriteLine("...AND BUMP THE WUMPUS. IT'S AWAKE!");
                                    Wumpus.awake = true;
                                    Wumpus.Move();
                                }
                            }
                            if (Map.Check(Player.location) == GameObjectType.Pit)
                            {
                                Console.WriteLine("...and fall down a pit to your death.");
                                Game.end = true;
                            }
                            break;
                    }
                }
            }
        }
    }

    //All physical objects in the game
    class GameObject { }

    class Player : GameObject
    {
        public static int location;
        public static int arrows;

        public Player()
        {
            location = Map.GetEmpty();
            Map.empty.Add(location);
            arrows = 5;
        }

        public static bool At(int a)
        {
            return a == location;
        }
    }

    class Wumpus : GameObject
    {
        public static int location;
        public static bool alive;
        public static bool awake;

        public Wumpus()
        {
            location = Map.GetEmpty();
            awake = false;
        }

        public static void Move()
        {
            Room[] adjacent = Map.Adjacent(location);
            List<int> choices = new List<int>();
            foreach (Room room in adjacent)
            {
                if (Map.empty.Contains(room.roomNum))
                    choices.Add(room.roomNum);
            }
            if (choices.Count > 0)
            {
                int choice = choices[Game.rand.Next(choices.Count)];
                Map.empty.Remove(choice);
                Map.empty.Add(location);
                location = choice;
            }
        }

        public static bool At(int a)
        {
            return a == location;
        }
    }

    class Bat : GameObject
    {
        public static int location;

        public Bat()
        {
            location = Map.GetEmpty();
        }

        public static bool At(int a)
        {
            return a == location;
        }
    }

    class Pit : GameObject
    {
        public static List<int> locations;
        public Pit(int n)
        {
            locations = new List<int>();

            for (int i = 0; i < n; i++)
                locations.Add(Map.GetEmpty());
        }

        public static bool At(int a)
        {
            foreach (int location in locations)
            {
                if (a == location)
                    return true;
            }
            return false;
        }
    }

    class Map
    {
        List<GameObject> gameObjects = new List<GameObject>();
        const int roomMax = 20;
        int objectCount = 0;
        int wumpusRoom;

        //20 Room map no more no less
        public static List<Room> rooms;
        public static List<int> empty;

        public Map()
        {
            //Remake lists
            rooms = new List<Room>(20);
            empty = new List<int>(20);

            //Create rooms
            for (int i = 0; i < 20; i++)
                rooms.Add(new Room(i));
            for (int i = 0; i < 20; i++)
                empty.Add(i);

            //Attach adjacent rooms
            rooms[0].setAdjacent(rooms[1], rooms[4], rooms[5]);
            rooms[1].setAdjacent(rooms[0], rooms[2], rooms[7]);
            rooms[2].setAdjacent(rooms[1], rooms[3], rooms[9]);
            rooms[3].setAdjacent(rooms[2], rooms[4], rooms[11]);
            rooms[4].setAdjacent(rooms[0], rooms[3], rooms[13]);
            rooms[5].setAdjacent(rooms[0], rooms[6], rooms[14]);
            rooms[6].setAdjacent(rooms[5], rooms[7], rooms[15]);
            rooms[7].setAdjacent(rooms[1], rooms[6], rooms[8]);
            rooms[8].setAdjacent(rooms[7], rooms[9], rooms[19]);
            rooms[9].setAdjacent(rooms[2], rooms[8], rooms[10]);
            rooms[10].setAdjacent(rooms[9], rooms[11], rooms[18]);
            rooms[11].setAdjacent(rooms[3], rooms[10], rooms[12]);
            rooms[12].setAdjacent(rooms[11], rooms[13], rooms[17]);
            rooms[13].setAdjacent(rooms[4], rooms[12], rooms[14]);
            rooms[14].setAdjacent(rooms[5], rooms[13], rooms[16]);
            rooms[15].setAdjacent(rooms[6], rooms[16], rooms[19]);
            rooms[16].setAdjacent(rooms[14], rooms[15], rooms[17]);
            rooms[17].setAdjacent(rooms[12], rooms[16], rooms[18]);
            rooms[18].setAdjacent(rooms[10], rooms[17], rooms[19]);
            rooms[19].setAdjacent(rooms[8], rooms[15], rooms[18]);
        }

        public void Add(GameObject g)
        {
            if (objectCount < roomMax)
            {
                objectCount++;
                gameObjects.Add(g);
            }
            else
                Console.WriteLine("Map Full");
        }

        public bool FireArrow(int start, string path)
        {
            bool hit = false;
            try
            {
                string[] pathArray = path.Split(' ');
                if (pathArray.Length > 5)
                    return false;
                if (pathArray.Length == 0)
                    return false;

                int currentLocation = start;
                int last = -1;
                int lastlast = -1;
                for (int i = 0; i < pathArray.Length; i++)
                {
                    lastlast = last;
                    last = currentLocation;
                    int next = Int32.Parse(pathArray[i]);
                    if (Map.isAdjacent(currentLocation, next))
                        currentLocation = next;
                    else
                        currentLocation = rooms[currentLocation].adjacentRooms[Game.rand.Next(3)].roomNum;

                    //Too sharp
                    if (lastlast == currentLocation)
                        return false;

                    //Shoot self
                    if (currentLocation == start)
                    {
                        Console.WriteLine("WAM! An arrow hits you. You die.");
                        Game.end = true;
                        return true;
                    }
                    else if (Map.Check(currentLocation) == GameObjectType.Wumpus)
                        hit = true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            //Player wins
            if (hit)
            {
                Console.WriteLine("Hooray! You hit the Wumpus!");
                Game.end = true;
                return true;
            }

            //Miss
            Console.WriteLine("You didn't hit anything.");
            if(!Wumpus.awake)
                Console.WriteLine("Your arrow made a lot of noise.");
            Wumpus.awake = true;
            return true;
        }

        //Check current room
        public static GameObjectType Check(int c)
        {
            if (Wumpus.At(c))
                return GameObjectType.Wumpus;
            if (Bat.At(c))
                return GameObjectType.Bat;
            if (Pit.At(c))
                return GameObjectType.Pit;
            return GameObjectType.None;
        }

        //Check adjacent rooms
        public static GameObjectType[] CheckAdjacent(int c)
        {
            GameObjectType[] hazards = new GameObjectType[3];
            int count = 0;
            foreach (Room room in rooms[c].Adjacent())
                hazards[count++] = Check(room.roomNum);
            return hazards;
        }

        //String of adjacent rooms
        public static string AdjacentString(int r)
        {
            Room[] adj = Adjacent(r);
            return adj[0].roomNum.ToString() + ", " + adj[1].roomNum.ToString() + ", " + adj[2].roomNum.ToString();
        }

        //List of adjacent rooms
        public static Room[] Adjacent(int r)
        {
            return rooms[r].Adjacent();
        }

        //Determines if two rooms are adjacent
        public static bool isAdjacent(int first, int second)
        {
            return rooms[first].isAdjacent(second);
        }

        //Returns a random empty room and removes it from the list
        public static int GetEmpty()
        {
            if (empty.Count > 0)
            {
                int room = empty[Game.rand.Next(empty.Count)];
                empty.Remove(room);
                return room;
            }
            else
                return -1;
        }
    }

    class Room
    {
        public Room[] adjacentRooms = new Room[3];
        public int roomNum;

        public Room(int r)
        {
            roomNum = r;
        }

        public void setAdjacent(Room r0, Room r1, Room r2)
        {
            adjacentRooms[0] = r0;
            adjacentRooms[1] = r1;
            adjacentRooms[2] = r2;
        }

        public bool isAdjacent(int a)
        {
            return adjacentRooms[0].roomNum == a || adjacentRooms[1].roomNum == a || adjacentRooms[2].roomNum == a;
        }

        public Room[] Adjacent()
        {
            return adjacentRooms;
        }

        public string ToString()
        {
            return roomNum.ToString();
        }
    }
}
