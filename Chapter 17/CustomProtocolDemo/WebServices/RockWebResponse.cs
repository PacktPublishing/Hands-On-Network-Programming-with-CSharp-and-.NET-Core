using System;
using System.Net;
using System.IO;

namespace CustomProtocolDemo {
public class RockWebResponse : WebResponse {
    private Stream _responseStream { get; set; }
    public DateTime TimeStamp { get; set; }
    public RockStatus Status { get; set; }
    public int Size { get; set; }
        
    public RockWebResponse(Stream responseStream) {
        _responseStream = responseStream;

        byte[] header = new byte[4];
        _responseStream.Read(header, 0, 4);
        var isValid = ValidateHeaders(header);
    }

    public Stream GetResponseStream() {
        return _responseStream;
    }

    private bool ValidateHeaders(byte[] header) {
        return true;
    }
}
}
