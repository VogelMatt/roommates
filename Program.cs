using Roommates.Models;
using Roommates.Repositories;
using Roommates.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Roommates
{
    class Program
    {
        //  that is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for chores"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name} ");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a chore"):
                        Console.Write("chore name: ");
                        string choreName = Console.ReadLine();

                   
                        Chore choreToAdd = new()
                        {
                            Name = choreName,                            
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;


                    case ("Search for roommate"):
                        Console.Write("roommate id; ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"{roommate.FirstName}'s rent is {roommate.RentPortion}. they live in {roommate.Room.Name}");
                        Console.Write("press any key to continue");
                        Console.ReadKey ();
                        break;

                    case ("View unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnnasignedChores();
                        Console.WriteLine("The following chores are unassigned:");
                        foreach (Chore uc in unassignedChores)
                        {
                            Console.WriteLine($" - {uc.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign chore to roommate"):
                        List<Chore> allChores = choreRepo.GetAll();
                        List<Roommate> allRoommates = roommateRepo.GetAll();

                        // Show list of all chores
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Id}: {c.Name}");
                        }
                        Console.Write("Please select a chore: ");
                        int selectedChoreId = int.Parse(Console.ReadLine());

                        // Show list of all roommates
                        foreach (Roommate r in allRoommates)
                        {
                            Console.WriteLine($"{r.Id}: {r.FirstName}");
                        }
                        Console.Write("Please enter the number of roommate to assign the chore: ");
                        int selectedRoommateId = int.Parse(Console.ReadLine());

                        // Add assigned chore to RoommateChore data table
                        choreRepo.AssignChore( selectedRoommateId, selectedChoreId);
                        Console.WriteLine("Success! Chore has been assigned.");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;



                    case ("Delete a room"):
                        List<Room> roomsToDelete = roomRepo.GetAll();
                        foreach (Room r in roomsToDelete)
                        {
                            Console.WriteLine("Select a room to delete.");
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }
                            int deleteMe = Int32.Parse(Console.ReadLine());
                        try
                        {
                            roomRepo.Delete(deleteMe);
                        }
                        catch
                        {
                            roomRepo.UpdateRoomId(deleteMe);
                            roomRepo.Delete(deleteMe); 

                            Console.WriteLine("this room has occupants. select a different room.");
                        }
                        Console.Write("press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a chore"):
                        List<Chore> choresToDelete = choreRepo.GetAll();
                        foreach (Chore r in choresToDelete)
                        {
                            Console.WriteLine("Select a chore to delete.");
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }
                        int deleteThis = Int32.Parse(Console.ReadLine());
                        try
                        {
                            choreRepo.Delete(deleteThis);
                        }
                        catch
                        {
                            choreRepo.DeleteChoreAssignment(deleteThis);
                            choreRepo.Delete(deleteThis);
                        }
                        Console.Write("press any key to continue");
                        Console.ReadKey();
                        break;


                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Delete a room",
                "Delete a chore",
                "Show all chores",
                "Search for chores",
                "Add a chore",
                "Search for roommate",
                "View unassigned chores",
                "Assign chore to roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {

                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
