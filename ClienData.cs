using System;
namespace CSharpWS2 {
    public class ClienData {
        public Guid To { get; set; }
        public Guid From { get; set; }
        public string Message { get; set; }
        public string AuthenticationToken { get; set; }
    }
}
