using System;
using System.Linq;
using System.Threading.Tasks;
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

            Task.Run(async () =>
            {
                await collection.Find(_ => true).ForEachAsync(student =>
                {
                    if (student.Scores.Count(x => x.Type.Equals("homework")) == 2)
                    {
                        var minScore = student.Scores.Where(x => x.Type.Equals("homework")).OrderBy(x => x.ScoreValue)
                            .FirstOrDefault();
                        student.Scores.Remove(minScore);
                        collection.ReplaceOne(x => x.Id == student.Id, student);
                    }
                });
            }).GetAwaiter().GetResult();

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
