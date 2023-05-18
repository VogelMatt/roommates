SELECT FirstName, LastName, Roommate.Id, RentPortion, [Name] 
FROM Roommate JOIN Room ON room.Id = roommate.RoomId WHERE roommate.Id = 1;
