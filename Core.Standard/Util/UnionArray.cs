using System;
using System.Runtime.InteropServices;

namespace Jammit
{
  [StructLayout(LayoutKind.Explicit)]
  public struct UnionArray
  {
    [FieldOffset(0)] public byte[] Bytes;

    [FieldOffset(0)] public sbyte[] Sbytes;

    [FieldOffset(0)] public short[] Shorts;

    [FieldOffset(0)] public int[] Ints;
  }
}
