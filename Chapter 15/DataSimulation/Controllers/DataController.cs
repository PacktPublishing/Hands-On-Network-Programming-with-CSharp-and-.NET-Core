using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace DataSimulation {
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase {

        [HttpGet("value/{id}")]
        public ActionResult<string> GetString(int id) {
            Thread.Sleep(5000);
            return $"{id}: some data";
        }

        [HttpGet("values/{id}")]
        public ActionResult<IEnumerable<string>> GetStrings(int id) {
            Thread.Sleep(5000);
            return new string[] { $"{id}: value1", $"{id + 1}: value2" };
        }

        [HttpGet("record/{id}")]
        public ActionResult<OutputRecord> GetRecord(int id) {
            Thread.Sleep(5000);
            return new OutputRecord() {
                Id = id,
                SimpleString = $"{id}: value 1",
                StringList = new List<string> {
                    $"{id}:value 2",
                    $"{id}:value 3"
                }
            };
        }

        [HttpGet("records/{id}")]
        public ActionResult<IEnumerable<OutputRecord>> GetRecords(int id) {
            Thread.Sleep(5000);
            return new List<OutputRecord>(){
                new OutputRecord() {
                    Id = id,
                    SimpleString = $"{id}: value 1",
                    StringList = new List<string> {
                        $"{id}:value 2",
                        $"{id}:value 3"
                    }
                }, new OutputRecord() {
                    Id = id + 1,
                    SimpleString = $"{id + 1}: value 4",
                    StringList = new List<string> {
                        $"{id + 1}:value 5",
                        $"{id + 1}:value 6"
                    }
                }
            };
        }
    }
}