using System;
using System.Net;


namespace CustomProtocolDemo {
    public class RockWebRequestCreator : IWebRequestCreate {
        public WebRequest Create(Uri uri) {
            return new RockWebRequest(uri);
        }
    }
}