//
// ZeroconfClient.cs
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
using System.Collections;
using System.Text.RegularExpressions;

using Mono.Zeroconf;

public class MZClient 
{
    private static bool resolve_shares = false; 
    private static string app_name = "mzclient";

    public static int Main(string [] args)
    {
        string type = "_afpovertcp._tcp";
        bool show_help = false;
        ArrayList services = new ArrayList();
        
        for(int i = 0; i < args.Length; i++) {
            if(args[i][0] != '-') {
                continue;
            }
            
            switch(args[i]) {
                case "-t":
                case "--type":
                    type = args[++i];
                    break;
                case "-r":
                case "--resolve":
                    resolve_shares = true;
                    break;
                case "-p":
                case "--publish":
                    services.Add(args[++i]);
                    break;
                case "-h":
                case "--help":
                    show_help = true;
                    break;
            }
        }
        
        if(show_help) {
            Console.WriteLine("Usage: {0} [-t type] [--resolve] [--publish \"description\"]", app_name);
            Console.WriteLine("    -h|--help     shows this help");
            Console.WriteLine("    -t|--type     uses 'type' as the service type");
            Console.WriteLine("                  (default is '_afpovertcp._tcp')");
            Console.WriteLine("    -r|--resolve  resolve found services to hosts");
            Console.WriteLine("    -p|--publish  publish a service of 'description'");
            Console.WriteLine("");
            Console.WriteLine("The service description for publishing has the following syntax.");
            Console.WriteLine("The TXT record is optional.\n");
            Console.WriteLine("    <type> <port> <name> TXT [ <key>='<value>', ... ]\n");
            Console.WriteLine("For example:\n");
            Console.WriteLine("    -p \"_http._tcp 80 Simple Web Server\"");
            Console.WriteLine("    -p \"_daap._tcp 3689 Aaron's Music TXT [ Password='false', Machine Name='Aaron's Box', txtvers='1' ]\"");
            Console.WriteLine("");
            return 1;
        }
        
        if(services.Count > 0) {
            foreach(string service_description in services) {
                RegisterService(service_description);
            }
        } else {
            Console.WriteLine("Hit ^C when you're bored waiting for responses.");
            Console.WriteLine();
            
			// Listen for events of some service type
            ServiceBrowser browser = new ServiceBrowser();
            browser.ServiceAdded += OnServiceAdded;
            browser.ServiceRemoved += OnServiceRemoved;
            browser.Browse(type, "local");
        }
       
        while(true) {
            System.Threading.Thread.Sleep(1000);
        }
    }
    
    private static void RegisterService(string serviceDescription)
    {
        Match match = Regex.Match(serviceDescription, @"(_[a-z]+._tcp|udp)\s*(\d+)\s*(.*)");
        if(match.Groups.Count < 4) {
            throw new ApplicationException("Invalid service description syntax");
        }
        Console.WriteLine(serviceDescription);
        string type = match.Groups[1].Value.Trim();
        short port = Convert.ToInt16(match.Groups[2].Value);
        string name = match.Groups[3].Value.Trim();
        
        int txt_pos = name.IndexOf("TXT");
        string txt_data = null;
        
        if(txt_pos > 0) {
            txt_data = name.Substring(txt_pos).Trim();
            name = name.Substring(0, txt_pos).Trim();
            
            if(txt_data == String.Empty) {
                txt_data = null;
            }
        }
                
        RegisterService service = new RegisterService();
        service.Name = name;
        service.RegType = type;
        service.ReplyDomain = "local.";
        service.Port = port;

        TxtRecord record = null;
        
        if(txt_data != null) {
            Match tmatch = Regex.Match(txt_data, @"TXT\s*\[(.*)\]");
            Console.WriteLine(tmatch.Groups.Count);
            if(tmatch.Groups.Count != 2) {
                throw new ApplicationException("Invalid TXT record definition syntax");
            }
            
            txt_data = tmatch.Groups[1].Value;
        
            foreach(string part in Regex.Split(txt_data, @"'\s*,")) {
                string expr = part.Trim();
                if(!expr.EndsWith("'")) {
                    expr += "'";
                }
                
                Match pmatch = Regex.Match(expr, @"(\w+\s*\w*)\s*=\s*['](.*)[']\s*");
                string key = pmatch.Groups[1].Value.Trim();
                string val = pmatch.Groups[2].Value.Trim();
                
                if(key == null || key == String.Empty || val == null || val == String.Empty) {
                    throw new ApplicationException("Invalid key = 'value' syntax for TXT record item");
                }
                
                if(record == null) {
                    record = new TxtRecord();
                }
                
                record.Add(key, val);
            }
        }
        
        if(record != null) {
            service.TxtRecord = record;
        }
        
        service.Register();
        Console.WriteLine("*** Registered name = '{0}', type = '{1}', domain = '{2}'", 
            service.Name,
            service.RegType,
            service.ReplyDomain);
    }
    
    private static void OnServiceAdded(object o, ServiceBrowseEventArgs args)
    {
        Console.WriteLine("*** Found name = '{0}', type = '{1}', domain = '{2}'", 
            args.Service.Name,
            args.Service.RegType,
            args.Service.ReplyDomain);
        
        if(resolve_shares) {
            args.Service.Resolved += OnServiceResolved;
            args.Service.Resolve();
        }
    }
    
    private static void OnServiceRemoved(object o, ServiceBrowseEventArgs args)
    {        
        Console.WriteLine("*** Lost  name = '{0}', type = '{1}', domain = '{2}'", 
            args.Service.Name,
            args.Service.RegType,
            args.Service.ReplyDomain);    
    }
    
    private static void OnServiceResolved(object o, ServiceResolvedEventArgs args)
    {
        IResolvableService service = o as IResolvableService;
        Console.Write("*** Resolved name = '{0}', host = '{1}', port = '{2}'", 
            service.FullName, service.HostEntry.AddressList[0], service.Port);
        
        ITxtRecord record = service.TxtRecord;
        int record_count = record.Count;
        if(record != null && record_count > 0) {
            Console.Write(", TXT Record = [");
            for(int i = 0, n = record.Count; i < n; i++) {
                TxtRecordItem item = record.GetItemAt(i);
                Console.Write("{0} = '{1}'", item.Key, item.ValueString);
                if(i < n - 1) {
                    Console.Write(", ");
                }
            }
            Console.WriteLine("]");
        } else {
            Console.WriteLine("");
        }
    }
}

