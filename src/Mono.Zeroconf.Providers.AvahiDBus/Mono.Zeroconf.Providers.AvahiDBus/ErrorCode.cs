//
// ErrorCode.cs
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

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    public enum ErrorCode 
    {
        Ok = 0,
        Failure = -1,
        BadState = -2,
        InvalidHostName = - 3,
        InvalidDomainName = -4,
        NoNetwork = -5,
        InvalidTTL = -6,
        IsPattern = -7,
        Collision = -8,
        InvalidRecord = -9,
        InvalidServiceName = -10,
        InvalidServiceType = -11,
        InvalidPort = -12,
        InvalidKey = -13,
        InvalidAddress = -14,
        Timeout = -15,
        TooManyClients = -16,
        TooManyObjects = -17,
        TooManyEntries = -18,
        OS = -19,
        AccessDenied = -20,
        InvalidOperation = -21,
        DBusError = -22,
        Disconnected = -23,
        NoMemory = -24,
        InvalidObject = -25,
        NoDaemon = -26,
        InvalidInterface = -27,
        InvalidProtocol = -28,
        InvalidFlags = -29,
        NotFound = -30,
        InvalidConfig = -31,
        VersionMismatch = -32,
        InvalidServiceSubtype = -33,
        InvalidPacket = -34,
        InvalidDnsError = -35,
        DnsFormErr = -36,
        DnsServFail = -37,
        DnsNxDomain = -38,
        DnsNoTimp = -39,
        DnsRefused = -40,
        DnsYxDomain = -41,
        DnsYxRrSet = -42,
        DnsNxRrSet = -43,
        DnsNotAuth = -44,
        DnsNotZone = -45,
        InvalidRData = -46,
        InvalidDnsClass = -47,
        InvalidDnsType = -48,
        NotSupported = -49,
        NotPermitted = -50
    }
}
