using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Marking_week_project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //test test 
            ValidateUser();
            MenuLoop();
        }
        static void MenuLoop()
        {

            hotel MyHotel = new hotel("Hotel");
            bool DisplayMenu = true;
            string uChoice = "";

            while (DisplayMenu != false)
            {
                DisplayMenue();
                uChoice = Console.ReadLine();

                switch (uChoice)
                {
                    case "1":
                        MenuCheckGuessIn(MyHotel);
                        break;

                    case "2":
                        MenuCheckGuessOut(MyHotel);
                        break;

                    case "3":
                        MenuGetAvailableRooms(MyHotel);

                        break;
                    case "4":
                        MenuCalculateBill(MyHotel);
                        break;
                    case "5":
                        MenuListGuests(MyHotel);
                        break;
                    case "6":
                        MenuAddNewUser();
                        break;
                    case "0":
                        DisplayMenu = false;
                        break;
                    default:
                        Console.WriteLine("Not a valid menu option. Please try again \n");
                        break;
                }
            }

        }
        static string HashFrom(string str)
        {
            string hashed = "";
            foreach (char c in str)
            {
                hashed = ((int.Parse(str) - 101) / 67).ToString(); 
            }
            return hashed;
        }

        static string HashTo(string str)
        {
            string hashed = "";
            foreach (char c in str)
            {
                hashed = ((int)c * 67 + 101).ToString();
            }
            return hashed;
        }

        static void MenuAddNewUser()
        {
            string FileName = "users.txt";
            user newUser = new user();
            newUser.SetName(StrValidate("Enter your username: "));
            newUser.SetPassword(StrValidate("Enter your password: "));

            using (StreamWriter SW = new StreamWriter(FileName))
            {
                SW.WriteLine();
                SW.Write(HashTo(newUser.GetName()) + "," + HashTo(newUser.GetPassword()));
            }
        }

        static void MenuListGuests(hotel MyHotel)
        {
            MyHotel.ListGuests();
        }
        static void MenuCalculateBill(hotel MyHotel)
        {
            MyHotel.CalculateBill(GuestValidate(MyHotel.GetGuestList()));
        }

        static void MenuGetAvailableRooms(hotel MyHotel)
        {
            List<Room> AvailableRooms = MyHotel.GetAvailableRooms();

            Console.WriteLine("Available rooms" + new string('-', 30));
            foreach (Room room in AvailableRooms)
            {
                Console.WriteLine(room.RoomNumber);
            }
        }

        static void MenuCheckGuessOut(hotel MyHotel)
        {
            MyHotel.CheckOut(StrValidate("Guest ID?:"));
        }

        static void MenuCheckGuessIn(hotel MyHotel)
        {
            Guest NewGuest = new Guest();

            NewGuest.SetName(StrValidate("Name of Guest?: ", "name"));

            NewGuest.SetGuestID(StrValidate("Guest ID?: "));

            NewGuest.SetCheckInDate(DateValidate("Check in date?: "));

            Console.WriteLine("\nSelect Room Type:");
            Console.WriteLine("1. Single - $15 per night");
            Console.WriteLine("2. Double - $20 per night");
            Console.WriteLine("3. Suite - $25 per night");
            string roomChoice = Console.ReadLine();

            string selectedRoomType = "";
            switch (roomChoice)
            {
                case "1":
                    selectedRoomType = "single";
                    break;
                case "2":
                    selectedRoomType = "double";
                    break;
                case "3":
                    selectedRoomType = "suite";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to single room.");
                    selectedRoomType = "single";
                    break;
            }

            MyHotel.CheckIn(NewGuest, selectedRoomType);
        }

        static void ValidateUser()
        {
            List<user> UserList = GetUsers();
            user CurrentUser = new user();
            bool validUser = false;
            string userName, password;

            while (!validUser)
            {
                userName = StrValidate("Enter your username: ");
                CurrentUser.SetName(userName);

                password = StrValidate("Enter your password: ");
                CurrentUser.SetPassword(password);

                for (int i = 0; i < UserList.Count; i++)
                {
                    if (CurrentUser.GetName() == HashFrom(UserList[i].GetName()) && CurrentUser.GetPassword() == HashFrom(UserList[i].GetPassword()) || CurrentUser.GetName() == "0")
                    {
                        validUser = true;
                        Console.WriteLine();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid user, try again");
                    }
                }
            }
        }

        static List<user> GetUsers()
        {
            string FileName = "users.txt";
            List<user> UserList = new List<user>();

            if (File.Exists(FileName))
            {
                using (StreamReader SR = new StreamReader(FileName))
                {
                    foreach (string line in SR.ReadToEnd().Split('\n'))
                    {
                        user MyUser = new user();
                        string[] entries = line.Split(',');
                        MyUser.SetName(entries[0].Trim());
                        MyUser.SetPassword(entries[1].Trim());
                        UserList.Add(MyUser);
                    }
                }
            }

            return UserList;
        }
        static void DisplayMenue()
        {
            Console.WriteLine("Hotel");
            Console.WriteLine("1. Check Guest in");
            Console.WriteLine("2. Check Guest Out");
            Console.WriteLine("3. Get available rooms");
            Console.WriteLine("4. Calculate Guest Bill");
            Console.WriteLine("5. List current Guests");
            Console.WriteLine("6. Add new user");
            Console.WriteLine("0. Exit");
        }

        static string StrValidate(string Input, string type = "")
        {
            bool validated = false;
            string ans = "";

            while (validated == false)
            {
                Console.WriteLine(Input);
                ans = Console.ReadLine();

                if (type.ToLower() == "name")
                {
                    if (Regex.IsMatch(ans, @"^[A-Za-z\s]+$"))
                    {
                        validated = true;
                    }
                    else
                    {
                        Console.WriteLine("Not a valid name, try again");
                    }
                }
                else
                {
                    if (Regex.IsMatch(ans, @"^[A-Za-z0-9\s]+$"))
                    {
                        validated = true;
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input, try again");
                    }
                }
            }
            return ans;
        }

        static DateTime DateValidate(string Input)
        {
            bool validated = false;
            DateTime ans = DateTime.MinValue;

            while (validated == false)
            {
                Console.WriteLine(Input);

                if (DateTime.TryParse(Console.ReadLine(), out ans))
                {
                    validated = true;
                }
                else
                {
                    Console.WriteLine("Not a valid date, try again");
                }
            }
            return ans;
        }

        static Guest GuestValidate(List<Guest> GuestList)
        {
            bool validated = false;
            bool found = false;
            Guest SearchGuest = new Guest();

            while (validated == false)
            {
                found = false;
                SearchGuest.SetGuestID(StrValidate("What is the Guest's ID?: "));

                foreach (Guest guest in GuestList)
                {
                    if (SearchGuest.GuestID == guest.GuestID)
                    {
                        Console.WriteLine("Valid ID");
                        SearchGuest = guest;
                        found = true;
                        validated = true;
                    }

                }

                if (found == false)
                {
                    Console.WriteLine("Invalid Guest ID try again");
                }
            }
            return SearchGuest;
        }

        class hotel
        {
            private string Name;
            private List<Room> Rooms;
            private List<Guest> CurrentGuests;

            public hotel(string name)
            {
                Name = name;
                Rooms = new List<Room>();
                CurrentGuests = new List<Guest>();

                Rooms.Add(new Room(101, "single", "15"));
                Rooms.Add(new Room(102, "single", "15"));
                Rooms.Add(new Room(103, "double", "20"));
                Rooms.Add(new Room(104, "double", "20"));
                Rooms.Add(new Room(105, "suite", "25"));
            }

            public List<Guest> GetGuestList()
            {
                return new List<Guest>(CurrentGuests);
            }

            public void CheckIn(Guest NewGuest, string roomType)
            {
                bool roomFound = false;
                for (int i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms[i].IsOccupied == false && Rooms[i].RoomType == roomType && roomFound == false)
                    {
                        Rooms[i].IsOccupied = true;
                        NewGuest.SetRoomNumber(Rooms[i].RoomNumber);
                        CurrentGuests.Add(NewGuest);
                        roomFound = true;
                        Console.WriteLine($"Guest checked into room {Rooms[i].RoomNumber} ({roomType})");
                    }
                }

                if (roomFound == false)
                {
                    Console.WriteLine($"No available {roomType} rooms. Hotel may be full or that room type is unavailable.");
                }
            }

            public void CheckOut(string GuestID)
            {
                bool CheckedOut = false;

                for (int i = 0; i < CurrentGuests.Count; i++)
                {
                    if (CurrentGuests[i].GuestID == GuestID)
                    {
                        int RoomNum = CurrentGuests[i].AssignedRoomNumber;

                        foreach (Room room in Rooms)
                        {
                            if (room.RoomNumber == RoomNum)
                            {
                                room.IsOccupied = false;
                                break;
                            }
                        }

                        CurrentGuests.RemoveAt(i);
                        CheckedOut = true;
                        Console.WriteLine($"Guest {GuestID} checked out successfully.");
                        break;
                    }
                }

                if (!CheckedOut)
                {
                    Console.WriteLine("Guest ID not found.");
                }
            }

            public List<Room> GetAvailableRooms()
            {
                List<Room> AvailableRooms = new List<Room>();
                foreach (Room room in Rooms)
                {
                    if (room.IsOccupied == false)
                    {
                        AvailableRooms.Add(room);
                    }
                }
                return AvailableRooms;
            }

            public double CalculateBill(Guest Guest)
            {
                DateTime checkOutDate;
                bool validDate = false;

                while (!validDate)
                {
                    checkOutDate = DateValidate("Check out date?: ");

                    if (checkOutDate <= Guest.CheckInDate)
                    {
                        Console.WriteLine($"Check out date must be after check in date ({Guest.CheckInDate.ToShortDateString()}). Please try again.");
                    }
                    else
                    {
                        Guest.SetCheckOutDate(checkOutDate);
                        validDate = true;
                    }
                }

                TimeSpan Duration = Guest.CheckOutDate - Guest.CheckInDate;

                string RoomType = "";
                double PricePerNight = 0;

                foreach (Room room in Rooms)
                {
                    if (room.RoomNumber == Guest.AssignedRoomNumber)
                    {
                        RoomType = room.RoomType;
                        PricePerNight = double.Parse(room.PricePerNight);
                        break;
                    }
                }

                double TotalPrice = Duration.TotalDays * PricePerNight;

                Console.WriteLine($"Guest: {Guest.Name}");
                Console.WriteLine($"Room Type: {RoomType}");
                Console.WriteLine($"Duration: {Duration.TotalDays} days");
                Console.WriteLine($"Total Bill: ${TotalPrice}");

                return TotalPrice;
            }

            public void ListGuests()
            {
                Console.WriteLine("Guest List" + new String('-', 30));

                if (CurrentGuests.Count == 0)
                {
                    Console.WriteLine("No guests currently checked in.");
                    return;
                }

                foreach (Guest guest in CurrentGuests)
                {
                    Console.WriteLine("\t" + guest.Name);
                    Console.WriteLine("\t" + guest.GuestID);
                    Console.WriteLine("\tRoom: " + guest.AssignedRoomNumber);
                    Console.WriteLine("\tCheck In: " + guest.CheckInDate);
                    Console.WriteLine("\t" + new String('-', 15));
                }
            }
        }

        class Room
        {
            public int RoomNumber { get; }
            public string RoomType { get; }
            public string PricePerNight { get; }
            public bool IsOccupied { get; set; }

            public Room(int roomNumber, string roomType, string pricePerNight)
            {
                RoomNumber = roomNumber;
                RoomType = roomType;
                PricePerNight = pricePerNight;
                IsOccupied = false;
            }
        }

        class Guest
        {
            public string Name { get; private set; }
            public string GuestID { get; private set; }
            public int AssignedRoomNumber { get; private set; }
            public DateTime CheckInDate { get; private set; }
            public DateTime CheckOutDate { get; private set; }

            public void SetName(string name)
            {
                this.Name = name;
            }

            public void SetGuestID(string id)
            {
                this.GuestID = id;
            }

            public void SetRoomNumber(int roomNumber)
            {
                this.AssignedRoomNumber = roomNumber;
            }

            public void SetCheckInDate(DateTime checkInDate)
            {
                this.CheckInDate = checkInDate;
            }

            public void SetCheckOutDate(DateTime checkOutDate)
            {
                this.CheckOutDate = checkOutDate;
            }
        }

        class user
        {
            private string name;
            private string password;

            public void SetName(string name)
            {
                this.name = name;
            }

            public string GetName()
            {
                return this.name;
            }

            public void SetPassword(string password)
            {
                this.password = password;
            }

            public string GetPassword()
            {
                return this.password;
            }

        }
    }
}