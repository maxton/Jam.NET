using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jammit.Mobile.Client
{
  public enum AuthorizationStatus
  {
    Unknown   = 0,
    Requested = 1,
    Rejected  = 2,
    Approved  = 3
  }
}
