//
// AvahiUtils.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright (C) 2008 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using Mono.Zeroconf;

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    public static class AvahiUtils
    {
        public static Protocol FromMzcProtocol (AddressProtocol addressProtocol)
        {
            switch (addressProtocol) {
                case AddressProtocol.IPv4: 
                    return Protocol.IPv4;
                case AddressProtocol.IPv6: 
                    return Protocol.IPv6;
                case AddressProtocol.Any:
                default:
                    return Protocol.Unspecified;
            }
        }
        
        public static AddressProtocol ToMzcProtocol (Protocol addressProtocol)
        {
            switch (addressProtocol) {
                case Protocol.IPv4: 
                    return AddressProtocol.IPv4;
                case Protocol.IPv6: 
                    return AddressProtocol.IPv6;
                case Protocol.Unspecified:
                default:
                    return AddressProtocol.Any;
            }
        }
        
        public static int FromMzcInterface (uint @interface)
        {
            // Avahi appears to use a value of -1 for "unspecified" ("any"), and
            // zero-based interface indexes... we hope
            
            switch (@interface) {
                case 0: return -1;
                default: return (int)@interface - 1;
            }
        }
        
        public static uint ToMzcInterface (int @interface)
        {
            switch (@interface) {
                case -1: return 0;
                default: return (uint)@interface + 1;
            }
        }
        
        public static ServiceErrorCode ErrorCodeToServiceError (ErrorCode error)
        {
            switch (error) {
                case ErrorCode.Ok: return ServiceErrorCode.None;
                case ErrorCode.Collision: return ServiceErrorCode.NameConflict;
                default: return ServiceErrorCode.Unknown;
            }
        }
    }
}
