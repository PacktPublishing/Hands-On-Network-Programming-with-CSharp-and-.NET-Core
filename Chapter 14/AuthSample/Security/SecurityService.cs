using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSample {
public static class SecurityService {
  public static SymmetricSecurityKey GetSecurityKey() {
      string key = "0125eb1b-0251-4a86-8d43-8ebeeeb39d9a";
      return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
  }

  public static string GetIssuer() {
      return "https://our-issuer.com/oauth";
  }

  public static string GetAudience() {
      return "we_the_audience";
  }
}
}
