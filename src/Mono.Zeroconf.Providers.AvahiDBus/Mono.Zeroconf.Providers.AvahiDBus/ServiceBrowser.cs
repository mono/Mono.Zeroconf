//
// ServiceBrowser.cs
//
// Author:
//   Aaron Bockover <abockover@novell.com>
//
// Copyright (C) 2007-2008 Novell, Inc.
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
using System.Collections.Generic;
using Mono.Zeroconf;

using NDesk.DBus;

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    public class ServiceBrowser : IServiceBrowser
    {
        public event ServiceBrowseEventHandler ServiceAdded;
        public event ServiceBrowseEventHandler ServiceRemoved;
    
        private IAvahiServiceBrowser service_browser;
        private Dictionary<string, BrowseService> services = new Dictionary<string, BrowseService> ();
        
        public void Dispose ()
        {
            lock (this) {
                IAvahiServiceBrowser service_browser = this.service_browser;
                if (service_browser != null) {
                    this.service_browser = null;
                    service_browser.ItemNew -= OnItemNew;
                    service_browser.ItemRemove -= OnItemRemove;
                    service_browser.Free ();
                }

                if (services.Count > 0) {
                    List<BrowseService> services_list = new List<BrowseService> (services.Values);
                    services.Clear (); // Clear first so we no-op if we Dispose() again

                    foreach (BrowseService service in services_list) {
                        service.Dispose ();
                    }
                }
            }
        }
    
        public void Browse (uint interfaceIndex, AddressProtocol addressProtocol, string regtype, string domain)
        {
            DBusManager.Bus.TrapSignals ();
            
            lock (this) {
                Dispose ();
                
                ObjectPath object_path = DBusManager.Server.ServiceBrowserNew (
                    AvahiUtils.FromMzcInterface (interfaceIndex), 
                    AvahiUtils.FromMzcProtocol (addressProtocol), 
                    regtype ?? String.Empty, domain ?? String.Empty, 
                    LookupFlags.None);
                    
                service_browser = DBusManager.GetObject<IAvahiServiceBrowser> (object_path);
            }
            
            service_browser.ItemNew += OnItemNew;
            service_browser.ItemRemove += OnItemRemove;
            
            DBusManager.Bus.UntrapSignals ();
        }
        
        protected virtual void OnServiceAdded (BrowseService service)
        {
            ServiceBrowseEventHandler handler = ServiceAdded;
            if (handler != null) {
                handler (this, new ServiceBrowseEventArgs (service));
            }
        }
        
        protected virtual void OnServiceRemoved (BrowseService service)
        {
            ServiceBrowseEventHandler handler = ServiceRemoved;
            if (handler != null) {
                handler (this, new ServiceBrowseEventArgs (service));
            }
        }
        
        public IEnumerator<IResolvableService> GetEnumerator ()
        {
            lock (this) {
                foreach (IResolvableService service in services.Values) {
                    yield return service;
                }
            }
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }
        
        private void OnItemNew (int @interface, Protocol protocol, string name, string type, 
            string domain, LookupResultFlags flags)
        {
            lock (this) {
                BrowseService service = new BrowseService (name, type, domain, @interface, protocol);
                BrowseService to_dispose;

                if (services.TryGetValue (name, out to_dispose)) {
                    services[name] = service;
                    to_dispose.Dispose ();
                } else {
                    services.Add (name, service);
                }
                
                OnServiceAdded (service);
            }
        }
        
        private void OnItemRemove (int @interface, Protocol protocol, string name, string type, 
            string domain, LookupResultFlags flags)
        {
            lock (this) {
                BrowseService service = new BrowseService (name, type, domain, @interface, protocol);
                BrowseService to_dispose;

                // handler may be called recursively, so remove service from services before disposing
                if (services.TryGetValue (name, out to_dispose)) {
                    services.Remove (name);
                    to_dispose.Dispose ();
                }
                
                OnServiceRemoved (service);
                service.Dispose ();
            }
        }
    }
}
