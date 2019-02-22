using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

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
