//
// BrowseService.cs
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
using System.Net;
using NDesk.DBus;
using Mono.Zeroconf;

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    public class BrowseService : Service, IResolvableService, IDisposable
    {
        private string full_name;
        private IPHostEntry host_entry;
        private string host_target;
        private short port;
        private bool disposed;
        private IAvahiServiceResolver resolver;
    
        public event ServiceResolvedEventHandler Resolved;
        
        public BrowseService (string name, string regtype, string replyDomain, int @interface, Protocol aprotocol)
            : base (name, regtype, replyDomain, @interface, aprotocol)
        {
        }
        
        public void Dispose ()
        {
            lock (this) {
                disposed = true;
                DisposeResolver ();
            }
        }
        
        private void DisposeResolver ()
        {
            lock (this) {
                if (resolver != null) {
                    resolver.Failure -= OnResolveFailure;
                    resolver.Found -= OnResolveFound;
                    resolver.Free ();
                    resolver = null;
                }
            }
        }
        
        public void Resolve ()
        {
            if (disposed) {
                throw new InvalidOperationException ("The service has been disposed and cannot be resolved. " + 
                    " Perhaps this service was removed?");
            }
            
            DBusManager.Bus.TrapSignals ();
            
            lock (this) {
                if (resolver != null) {
                    throw new InvalidOperationException ("The service is already running a resolve operation");
                }
                
                ObjectPath path = DBusManager.Server.ServiceResolverNew (AvahiInterface, AvahiProtocol, 
                    Name ?? String.Empty, RegType ?? String.Empty, ReplyDomain ?? String.Empty, 
                    AvahiProtocol, LookupFlags.None);
                    
                resolver = DBusManager.GetObject<IAvahiServiceResolver> (path);
            }
            
            resolver.Failure += OnResolveFailure;
            resolver.Found += OnResolveFound;
            
            DBusManager.Bus.UntrapSignals ();
        }
        
        protected virtual void OnResolved ()
        {
            ServiceResolvedEventHandler handler = Resolved;
            if (handler != null) {
                handler (this, new ServiceResolvedEventArgs (this));
            }
        }
        
        private void OnResolveFailure (string error)
        {
            DisposeResolver ();
        }
        
        private void OnResolveFound (int @interface, Protocol protocol, string name, 
            string type, string domain, string host, Protocol aprotocol, string address, 
            ushort port, byte [][] txt, LookupResultFlags flags)
        {
            Name = name;
            RegType = type;
            AvahiInterface = @interface;
            AvahiProtocol = protocol;
            ReplyDomain = domain;
            TxtRecord = new TxtRecord (txt);
            
            this.full_name = String.Format ("{0}.{1}.{2}", name.Replace (" ", "\\032"), type, domain);
            this.port = (short)port;
            this.host_target = host;
            
            host_entry = new IPHostEntry ();
            host_entry.AddressList = new IPAddress[1];
            if (IPAddress.TryParse (address, out host_entry.AddressList[0]) && protocol == Protocol.IPv6) {
                host_entry.AddressList[0].ScopeId = @interface;
            }
            host_entry.HostName = host;
            
            OnResolved ();
        }
        
        public string FullName { 
            get { return full_name; }
        }
        
        public IPHostEntry HostEntry { 
            get { return host_entry; } 
        }
        
        public string HostTarget { 
            get { return host_target; } 
        }
        
        public short Port { 
            get { return port; }
        }
    }
}
