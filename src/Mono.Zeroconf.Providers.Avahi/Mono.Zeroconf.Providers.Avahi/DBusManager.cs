//
// DBusManager.cs
//
// Authors:
//	Aaron Bockover  <abockover@novell.com>
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
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
using System.IO;
using System.Reflection;
using System.Threading;

using NDesk.DBus;

namespace Mono.Zeroconf.Providers.Avahi
{
    internal static class DBusManager
    {
    	private const uint MINIMUM_AVAHI_API_VERSION = 515;
        
        private static object iterate_thread_mutex = new object();
        private static int iterate_freeze_count = 0;
        private static Thread iterate_thread;

		private static Bus bus;

        private static IAvahiServer server;
        public static IAvahiServer Server {
        	get { return server; }
        }
        
        private static void InitializeSystemBusConnection()
        {
            string address = Environment.GetEnvironmentVariable("DBUS_SYSTEM_BUS_ADDRESS");
            if(String.IsNullOrEmpty(address)) {
            	address = "unix:path=/var/run/dbus/system_bus_socket";
            }
			
			bus = Bus.Open(address);
        }

        public static void Initialize()
        {
            InitializeSystemBusConnection();
            
			if(!bus.NameHasOwner("org.freedesktop.Avahi")) {
				throw new ApplicationException("Could not find org.freedesktop.Avahi");
			}
			
			server = bus.GetObject<IAvahiServer>("org.freedesktop.Avahi", new ObjectPath("/"));
			uint api_version = server.GetAPIVersion();
			
			if(api_version < MINIMUM_AVAHI_API_VERSION) {
				throw new ApplicationException(String.Format("Avahi API version " +
					"{0} is required, but {1} is what the server returned.", 
					MINIMUM_AVAHI_API_VERSION, api_version));
			}
			
			// If you remove this you will see that the events 
			// come through -- it'll work for the first object,
			// but once the thread is created you are hosed.
			// Leaving this call here for testing/debugging
			// while the iterate issue is looked into. For
			// production, this call will be deferred until
			// it's necessary.
			EnsureIterate();
        }
        
        public static T GetObject<T>(string object_path)
        {
        	return GetObject<T>(new ObjectPath(object_path));
        }

		public static T GetObject<T>(ObjectPath object_path)
		{
			return bus.GetObject<T>("org.freedesktop.Avahi", object_path);
		}
        
        public static void EnsureIterate()
        {
        	lock(iterate_thread_mutex) {
        		if(iterate_thread != null) {
        			return;
        		}
        		
        		iterate_thread = new Thread(Iterate);
        		iterate_thread.IsBackground = true;
        		iterate_thread.Start();
        	}
        }
        
        private static void Iterate()
        {
			while(true) {
				if(iterate_freeze_count == 0) {
					bus.Iterate();
				}
			}
        }
        
        public static void BusLock()
        {
        	Interlocked.Increment(ref iterate_freeze_count);
        	EnsureIterate();
        }
        
        public static void BusUnlock()
        {
        	Interlocked.Decrement(ref iterate_freeze_count);
        	EnsureIterate();
        }
    }
}
