using Microsoft.Data.SqlClient;
using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roommates.Repository
{
    public class ChoreRepository : BaseRepository
    {



        public ChoreRepository(string connectionString) : base(connectionString) { }



        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using SqlDataReader reader = cmd.ExecuteReader();
                Chore chore = null;


                if (reader.Read())
                {
                    chore = new Chore
                    {
                        Id = id,
                        Name = reader.GetString(reader.GetOrdinal("Name"))
                    };
                }
                return chore;
            }
        }

        public List<Chore> GetUnnasignedChores()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT ch.Id, ch.Name FROM Chore ch
                                        FULL OUTER JOIN RoommateChore rc ON rc.ChoreId = ch.Id
                                        WHERE RoommateId IS NULL";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();
                        while (reader.Read())
                        {
                            // add the resultant objs to the list
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }
        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO roommateChore (roommateId, choreId) OUTPUT INSERTED.ID VALUES (@roommateId, @choreId)";
                    cmd.Parameters.AddWithValue("@roommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    int id = 0;
                    id = (int)cmd.ExecuteScalar();
                    if (id != 0)
                    {
                        Console.WriteLine("This operation was successful");
                    }

                }
            }
        }
        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                                    SET Name = @name,
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.Parameters.AddWithValue("@id", chore.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Insert(Chore chore)
        {
            using SqlConnection conn = Connection;
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Chore (Name) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name)";
            cmd.Parameters.AddWithValue("@name", chore.Name);
            int id = (int)cmd.ExecuteScalar();

            chore.Id = id;


        }
        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            chores.Add(chore);

                        }
                        return chores;
                    }
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteChoreAssignment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM RoommateChore WHERE RoommateChore.ChoreId = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}
