using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net.Sockets;

namespace CustomProtocolDemo {
    public class RockWebRequest : WebRequest {
        public override Uri RequestUri { get; }
        public RockVerb Verb { get; set; } = RockVerb.Update;
        public IEnumerable<string> Records { get; set; }
        public long Fields { get; set; }


        public RockWebRequest(Uri uri) {
            RequestUri = uri;
        }

        public override WebResponse GetResponse() {
            var messageString = ConcatenateRecords();
            var message = Encoding.ASCII.GetBytes(messageString);
            var checksum = SHA256.Create().ComputeHash(message);

            var byteList = new List<byte>();
            byteList.AddRange(GetHeaderBytes(message.Length));
            byteList.AddRange(checksum);
            byteList.AddRange(message);
            TcpClient client = new TcpClient(RequestUri.Host, RequestUri.Port);
            var stream = client.GetStream();
            stream.Write(byteList.ToArray(), 0, byteList.Count);
            return new RockWebResponse(stream);
        }

        private string ConcatenateRecords() {
            StringBuilder messageBuilder = new StringBuilder();
            foreach (var record in Records) {
                if (messageBuilder.ToString().Length > 0) {
                    messageBuilder.Append(Environment.NewLine);
                }
                messageBuilder.Append(record);
            }
            return messageBuilder.ToString();
        }

        private IEnumerable<byte> GetHeaderBytes(int messageSize) {
            var headerBytes = new List<byte>();
            int verbAndSize = (int)Verb | (messageSize >> 2);
            headerBytes.AddRange(BitConverter.GetBytes(verbAndSize));

            //Add empty byte padding in the FIELDS header
            for (var i = 0; i < 20; i++) {
                headerBytes.Add(0b00000000);
            }

            headerBytes.AddRange(BitConverter.GetBytes(Fields));
            return headerBytes;
        }
    }
}
