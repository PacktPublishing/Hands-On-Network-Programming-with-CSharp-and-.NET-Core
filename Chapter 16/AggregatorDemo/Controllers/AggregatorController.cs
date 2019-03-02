using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AggregatorDemo {
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase {

        private IDependentDataService _dependentService;

        public AggregatorController(IDependentDataService depSvc) {
            _dependentService = depSvc;
        }
        
        [HttpGet("data")]
        public async Task<ActionResult<string>> Get() {
            return await _dependentService.GetDataResponse();
        }
    }
}
