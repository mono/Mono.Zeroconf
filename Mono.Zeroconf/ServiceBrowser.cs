//
// ServiceBrowser.cs
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
using System.Threading;

namespace Mono.Zeroconf
{
    public class ServiceBrowseEventArgs : EventArgs
    {
        public BrowseService Service;
        public bool MoreComing;
    }

    public delegate void ServiceBrowseEventHandler(object o, ServiceBrowseEventArgs args);

    public class ServiceBrowser : IEnumerable, IDisposable
    {
        private uint interface_index;
        private string regtype;
        private string domain;
        
        private ServiceRef sd_ref = ServiceRef.Zero;
        private Hashtable service_table = new Hashtable();
        
        private Thread thread;
        
        public event ServiceBrowseEventHandler ServiceAdded;
        public event ServiceBrowseEventHandler ServiceRemoved;
        
        public ServiceBrowser(string regtype) : this(0, regtype, null)
        {
        }
        
        public ServiceBrowser(uint interfaceIndex, string regtype, string domain)
        {
            this.interface_index = interfaceIndex;
            this.regtype = regtype;
            this.domain = domain;
            
            if(regtype == null) {
                throw new ArgumentNullException("regtype");
            }
        }
        
        public void Start(bool async)
        {
            if(thread != null) {
                throw new InvalidOperationException("ServiceBrowser is already started");
            }
            
            if(async) {
                thread = new Thread(new ThreadStart(ThreadedStart));
                thread.IsBackground = true;
                thread.Start();
            } else {
                ProcessStart();
            }
        }
        
        public void Start()
        {
            Start(false);
        }
        
        public void StartAsync()
        {
            Start(true);
        }
        
        private void ThreadedStart()
        {
            try {
                ProcessStart();
            } catch(ThreadAbortException) {
            }
            
            thread = null;
        }

        private void ProcessStart()
        {
            ServiceError error = Native.DNSServiceBrowse(out sd_ref, ServiceFlags.Default,
                interface_index, regtype,  domain, OnBrowseReply, IntPtr.Zero);

            if(error != ServiceError.NoError) {
                throw new ServiceErrorException(error);
            }

            sd_ref.Process();
        }
        
        public void Stop()
        {
            if(sd_ref != ServiceRef.Zero) {
                sd_ref.Deallocate();
                sd_ref = ServiceRef.Zero;
            }
            
            if(thread != null) {
                thread.Abort();
                thread = null;
            }
        }
        
        public void Dispose()
        {
            Stop();
        }
        
        public IEnumerator GetEnumerator()
        {
            return service_table.Values.GetEnumerator();
        }
        
        private void OnBrowseReply(ServiceRef sdRef, ServiceFlags flags, uint interfaceIndex, ServiceError errorCode, 
            string serviceName, string regtype, string replyDomain, IntPtr context)
        {
            BrowseService service = new BrowseService();
            service.Flags = flags;
            service.Name = serviceName;
            service.RegType = regtype;
            service.ReplyDomain = replyDomain;
            service.InterfaceIndex = interfaceIndex;
            
            ServiceBrowseEventArgs args = new ServiceBrowseEventArgs();
            args.Service = service;
            args.MoreComing = (flags & ServiceFlags.MoreComing) != 0;
            
            if((flags & ServiceFlags.Add) != 0) {
                lock(service_table.SyncRoot) {
                    service_table[serviceName] = service;
                }
                ServiceBrowseEventHandler handler = ServiceAdded;
                if(handler != null) {
                    handler(this, args);
                }
            } else {
                lock(service_table.SyncRoot) {
                    service_table.Remove(serviceName);
                }
                ServiceBrowseEventHandler handler = ServiceRemoved;
                if(handler != null) {
                    handler(this, args);
                }
            }
        }
    }
}
