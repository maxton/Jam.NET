using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Mobile.Client
{
  public enum AuthorizationStatus
  {
    Requested = 0,
    Rejected  = 1,
    Approved  = 2,
    Unknown   = 3
  }
}
