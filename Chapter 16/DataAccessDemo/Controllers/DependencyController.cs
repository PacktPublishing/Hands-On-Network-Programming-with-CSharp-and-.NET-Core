using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace DataAccessDemo {
[Route("api/[controller]")]
[ApiController]
public class DependencyController : ControllerBase {
    [HttpGet("new-data")]
    public ActionResult<string> GetDependentValue() {
        Thread.Sleep(Latency.GetLatency());
        return $"requested data: {new Random().Next() }";
    }
        
    [HttpGet("reset")]
    public ActionResult<string> Reset() {
        Latency.ResetLatency();
        return "success";
    }
}
}
