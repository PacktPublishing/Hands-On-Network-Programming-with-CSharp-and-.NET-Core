using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FitnessDataStore {
    public interface IDataAccess {
        Task<bool> WriteRecord(FitnessRecord newRecord);
        Task<List<FitnessRecord>> GetAllRecords();
        Task<List<FitnessRecord>> GetRecordsByWorkoutType(string workoutType);
        Task<FitnessRecord> GetRecordByTitle(string title);
        Task<bool> UpdateRecord(string title, string newComment);
        Task<bool> DeleteRecord(string title);
    }

    public class DataAccess : IDataAccess {
        private readonly string FILE_PATH = @"../fitness_data.txt";

        public async Task<bool> WriteRecord(FitnessRecord newRecord) {
            var records = await GetAllRecords();
            if (records.Where(x => x.title.ToUpper().Equals(newRecord.title.ToUpper())).Count() > 0) {
                return false;
            }
            using (StreamWriter sw = File.AppendText(FILE_PATH)) {
                var recordStr = JsonConvert.SerializeObject(newRecord);
                await sw.WriteLineAsync(recordStr);
            }
            return true;
        }

        public async Task<List<FitnessRecord>> GetAllRecords() {
            List<FitnessRecord> currentRecords = new List<FitnessRecord>();
            if (!File.Exists(FILE_PATH)) {
                using (var file = File.Create(FILE_PATH)){
                    //We only use this to create the file in the path if it doesn't exist.
                }
            }
            using (StreamReader sr = File.OpenText(FILE_PATH)) {
                while (!sr.EndOfStream) {
                    var recordStr = await sr.ReadLineAsync();
                    currentRecords.Add(JsonConvert.DeserializeObject<FitnessRecord>(recordStr));
                }
            }
            return currentRecords;
        }

        public async Task<List<FitnessRecord>> GetRecordsByWorkoutType(string workoutType) {
            var records = await GetAllRecords();
            return records.Where(x => x.workoutType.ToUpper().Contains(workoutType.ToUpper())).ToList();
        }

        public async Task<FitnessRecord> GetRecordByTitle(string title) {
            var records = await GetAllRecords();
            return records.Where(x => x.title.ToUpper().Equals(title.ToUpper())).FirstOrDefault();
        }

        public async Task<bool> UpdateRecord(string title, string newComment) {
            bool didUpdate = false;

            var records = await GetAllRecords();
            StringBuilder sb = new StringBuilder();

            foreach (var record in records) {
                if (record.title.ToUpper().Equals(title.ToUpper())) {
                    record.comments = newComment;
                    didUpdate = true;
                }
                var recordJson = JsonConvert.SerializeObject(record);
                sb.Append(recordJson);
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(FILE_PATH, sb.ToString());
            return didUpdate;
        }

        public async Task<bool> DeleteRecord(string title) {
            bool didDelete = false;

            var records = await GetAllRecords();
            StringBuilder sb = new StringBuilder();

            foreach (var record in records) {
                if (record.title.ToUpper().Equals(title.ToUpper())) {
                    didDelete = true;
                    continue;
                }
                var recordJson = JsonConvert.SerializeObject(record);
                sb.Append(recordJson);
            }

            File.WriteAllText(FILE_PATH, sb.ToString());
            return didDelete;
        }

    }
}
