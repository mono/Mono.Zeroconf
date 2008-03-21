//
// ResolvableService.cs
//
// Authors:
//    Aaron Bockover  <abockover@novell.com>
//
// Copyright (C) 2006-2007 Novell, Inc (http://www.novell.com)
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
using System.Net;
using AV=Avahi;
using Mono.Zeroconf;

namespace Mono.Zeroconf.Providers.Avahi
{
    public class ResolvableService : Service, IResolvableService
    {
        private AV.Client client;
    
        public event ServiceResolvedEventHandler Resolved;
    
        public ResolvableService(AV.Client client, AV.ServiceInfo service) : base(service)
        {
            this.client = client;
        }
        
        public void Resolve()
        {
            AV.ServiceResolver resolver = new AV.ServiceResolver(client, service);
            resolver.Found += OnServiceResolved;
        }
        
        private void OnServiceResolved(object o, AV.ServiceInfoArgs args)
        {
            service = args.Service;
            ((AV.ServiceResolver)o).Dispose();
        
            ServiceResolvedEventHandler handler = Resolved;
            if(handler != null) {
                handler(this, new ServiceResolvedEventArgs(this));
            }
        }
        
        public string FullName { 
            get {
                return String.Format("{0}.{1}.{2}", service.Name.Replace(" ", "\\032"), 
                    service.ServiceType, service.Domain);
            }
        }
        
        public IPHostEntry HostEntry { 
            get { 
                IPHostEntry host_entry = new IPHostEntry();
                host_entry.AddressList = new IPAddress[1];
                host_entry.AddressList[0] = service.Address;
                if(service.Protocol == AV.Protocol.IPv6) {
                    host_entry.AddressList[0].ScopeId = service.NetworkInterface;
                }
                host_entry.HostName = service.HostName;
                return host_entry;
            }
        }
        
        public string HostTarget { 
            get { return service.HostName; }
        }

        public int NetworkInterface {
            get { return service.NetworkInterface; }
        }
        
        public short Port { 
            get { return (short)service.Port; }
        }
    }
}
