using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace DemoTLS {
    [Route("demo/[controller]")]
    [ApiController]
    public class TlsController : ControllerBase {
        [HttpGet("initiate-connection")]
        public ActionResult<string> GetCertificate() {
            return "CERTIFICATE_DELIVERED";
        }

        [HttpGet("certificate-verified")]
        public ActionResult<string> GetVerification() {
            return "PUBLIC_KEY_FOR_ENCRYPTING_HANDSHAKE";
        }

        [HttpGet("hash-algorithms-requested")]
        public ActionResult<IEnumerable<string>> GetAlgorithms() {
            return new string[] {
                "SHA-256",
                "AES",
                "RSA"
            };
        }

        [HttpPost("hash-algorithm-selected")]
        public ActionResult<string> Post([FromBody] string sharedAlgorithm) {
            SessionService.CurrentAlgorithm = sharedAlgorithm;
            return SessionService.GenerateSharedKeyWithPrivateKeyAndRandomSessionKey();
        }
    }
}
