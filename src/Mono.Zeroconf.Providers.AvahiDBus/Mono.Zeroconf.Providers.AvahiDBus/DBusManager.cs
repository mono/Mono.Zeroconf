//
// DBusManager.cs
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
using System.IO;
using System.Reflection;
using System.Threading;

using NDesk.DBus;

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    internal static class DBusManager
    {
        private const uint MINIMUM_AVAHI_API_VERSION = 515;
        
        private static object thread_wait = new object ();
        private static bool initialized;
        private static Bus bus;
        public static Bus Bus {
            get { return bus; }
        }

        private static IAvahiServer server;
        public static IAvahiServer Server {
            get { return server; }
        }
                                
        private static void ConnectToSystemBus ()
        {
            string address = Environment.GetEnvironmentVariable ("DBUS_SYSTEM_BUS_ADDRESS");
            if (String.IsNullOrEmpty (address)) {
                address = "unix:path=/var/run/dbus/system_bus_socket";
            }
            
            bus = Bus.Open (address);
        }
        
        private static void IterateThread (object o)
        {
            lock (thread_wait) {
                ConnectToSystemBus ();
                Monitor.Pulse (thread_wait);
            }
            
            while (true) {
                bus.Iterate ();
            }
        }

        public static void Initialize ()
        {
            if (initialized) {
                return;
            }
            
            initialized = true;
            
            lock (thread_wait) {
                ThreadPool.QueueUserWorkItem (IterateThread);
                Monitor.Wait (thread_wait);
            }
            
            if (!bus.NameHasOwner("org.freedesktop.Avahi")) {
                throw new ApplicationException ("Could not find org.freedesktop.Avahi");
            }
            
            server = GetObject<IAvahiServer> ("/");
            uint api_version = server.GetAPIVersion ();
            
            if (api_version < MINIMUM_AVAHI_API_VERSION) {
                throw new ApplicationException (String.Format ("Avahi API version " +
                    "{0} is required, but {1} is what the server returned.", 
                    MINIMUM_AVAHI_API_VERSION, api_version));
            }
        }
        
        public static T GetObject<T> (string objectPath)
        {
            return GetObject<T> (new ObjectPath (objectPath));
        }

        public static T GetObject<T> (ObjectPath objectPath)
        {
            return bus.GetObject<T> ("org.freedesktop.Avahi", objectPath);
        }
    }
}
