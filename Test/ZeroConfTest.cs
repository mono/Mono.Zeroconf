//
// ZeroConfTest.cs
//
// Authors:
//	Aaron Bockover  <abockover@novell.com>
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
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
using Gtk;

public class DnsTest 
{
    public static void Main()
    {
        Application.Init();

        // Register a sample service
        RegisterService service = new RegisterService("Fruity Music", null, "_daap._tcp");
        TxtRecord record = new TxtRecord();
        record.Add("A", "Apples");
        record.Add("B", "Bananas");
        record.Add("C", "Carrots");
        service.Port = 8080; 
        service.TxtRecord = record;
        service.RegisterAsync();

        // Listen for events of some service type
        ServiceBrowser browser = new ServiceBrowser("_daap._tcp");
        browser.ServiceAdded += OnServiceAdded;
        browser.ServiceRemoved += OnServiceRemoved;
        browser.StartAsync();
        
        // Unregister our service in 10 seconds
        GLib.Timeout.Add(10000, delegate {
            service.Dispose();
            return false;
        });
        
        // Stop browsing and quit in 15 seconds
        GLib.Timeout.Add(15000, delegate {
            browser.Dispose();
            Application.Quit();
            return false;
        });
        
        Application.Run();
    }
    
    private static void OnServiceAdded(object o, ServiceBrowseEventArgs args)
    {
        Console.WriteLine("ADDED {0}", args.Service.Name);
        args.Service.Resolved += OnServiceResolved;
        args.Service.Resolve();
    }
    
    private static void OnServiceRemoved(object o, ServiceBrowseEventArgs args)
    {
        Console.WriteLine("REMOVED {0}", args.Service.Name);
    }
    
    private static void OnServiceResolved(object o, EventArgs args)
    {
        Service service = o as Service;
        Console.WriteLine("Resolved: {0} {1} {2} {3}", service.FullName, service.Port, 
            service.HostTarget, service.HostEntry.AddressList[0]);
        Console.WriteLine(service.TxtRecord);    
    }
}

