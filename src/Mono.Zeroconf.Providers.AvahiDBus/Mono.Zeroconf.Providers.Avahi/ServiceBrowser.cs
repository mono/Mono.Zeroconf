//
// ServiceBrowser.cs
//
// Authors:
//	Aaron Bockover  <abockover@novell.com>
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
using Mono.Zeroconf;

namespace Mono.Zeroconf.Providers.Avahi
{
    public class ServiceBrowser : IServiceBrowser
    {
        public event ServiceBrowseEventHandler ServiceAdded;
        public event ServiceBrowseEventHandler ServiceRemoved;
    
    	private IAvahiServiceBrowser service_browser;
    
        public ServiceBrowser()
        {
        }
        
        public void Dispose()
        {
        	if(service_browser != null) {
        		service_browser.ItemNew -= OnItemNew;
        		service_browser.ItemRemove -= OnItemRemove;
        		service_browser.Free();
        	}
        }
    
        public void Browse(string regtype, string domain)
        {
            Dispose();
            
            DBusManager.BusLock();
            
            service_browser = DBusManager.GetObject<IAvahiServiceBrowser>(
            	DBusManager.Server.ServiceBrowserNew(-1, -1, regtype, domain, 0));
            
            service_browser.ItemNew += OnItemNew;
            
            DBusManager.BusUnlock();
            
            DBusManager.BusLock();
            service_browser.ItemRemove += OnItemRemove;
            DBusManager.BusUnlock();
            
        }
        
        private void OnItemNew(int @interface, int protocol, string name, 
        	string type, string domain, uint flags)
        {
        	Console.WriteLine("NEW ITEM: {0}", name);
        }
        
        private void OnItemRemove(int @interface, int protocol, string name, 
        	string type, string domain, uint flags)
        {
        	Console.WriteLine("ITEM REMOVED: {0}", name);
        }
    }
}
