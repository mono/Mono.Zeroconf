//
// Service.cs
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
    public class Service : Mono.Zeroconf.IService
    {
        private string name;
        private string regtype;
        private string reply_domain;
        private int @interface;
        private Protocol aprotocol;
        private ITxtRecord txt_record;
        
        public Service ()
        {
            this.@interface = -1; // Unspecified
        }
        
        public Service (string name, string regtype, string replyDomain, int @interface, Protocol aprotocol)
        {
            this.name = name;
            this.regtype = regtype;
            this.reply_domain = replyDomain;
            this.@interface = @interface;
            this.aprotocol = aprotocol;
        }
        
        protected int AvahiInterface {
            get { return @interface; }
            set { @interface = value; }
        }
        
        protected Protocol AvahiProtocol {
            get { return aprotocol; }
            set { aprotocol = value; }
        }
        
        public string Name {
            get { return name; }
            set { name = value; }
        }
        
        public string RegType {
            get { return regtype; }
            set { regtype = value; }
        }
        
        public string ReplyDomain {
            get { return reply_domain; }
            set { reply_domain = value; }
        }
        
        public uint NetworkInterface {
            get { return AvahiUtils.ToMzcInterface (@interface); }
        }
        
        public AddressProtocol AddressProtocol {
            get { return AvahiUtils.ToMzcProtocol (aprotocol); }
        }
        
        public ITxtRecord TxtRecord {
            get { return txt_record; }
            set { txt_record = value; }
        }
    }
}
