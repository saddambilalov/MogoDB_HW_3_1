using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Models;

namespace MongoDB
{
    internal class Program
    {
        private static void Main()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var db = client.GetDatabase("school");
            var collection = db.GetCollection<Student>("students");
            var students = collection.Find(_ => true).ToList();

            foreach (var student in students)
            {
                if (student.Scores.Count(x => x.Type.Equals("homework")) != 2)
                {
                    continue;
                }
                var minScore = student.Scores.Where(x => x.Type.Equals("homework")).OrderBy(x => x.ScoreValue)
                    .FirstOrDefault();
                student.Scores.Remove(minScore);
                collection.ReplaceOne(x => x.Id == student.Id, student);
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
