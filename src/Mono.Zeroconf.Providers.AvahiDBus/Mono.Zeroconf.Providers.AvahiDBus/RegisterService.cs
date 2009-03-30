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
using System.Threading;
using NDesk.DBus;

namespace Mono.Zeroconf.Providers.AvahiDBus
{
    public class RegisterService : Service, IRegisterService
    {
        private ushort port;
        private IAvahiEntryGroup entry_group;
        
        public event RegisterServiceEventHandler Response;
     
        public RegisterService ()
        {
        }
     
        public RegisterService (string name, string regtype, string replyDomain, int @interface, Protocol aprotocol)
            : base (name, regtype, replyDomain, @interface, aprotocol)
        {
        }
        
        public void Register ()
        {
            RegisterDBus ();

            byte [][] txt_record = TxtRecord == null 
                ? new byte[0][] 
                : Mono.Zeroconf.Providers.AvahiDBus.TxtRecord.Render (TxtRecord);
            
            entry_group.AddService (AvahiInterface, AvahiProtocol, PublishFlags.None, 
                Name ?? String.Empty, RegType ?? String.Empty, ReplyDomain ?? String.Empty, 
                String.Empty, port, txt_record);

            entry_group.Commit ();
        }
        
        private void RegisterDBus ()
        {
            try {
                Monitor.Enter (this);
                DBusManager.Bus.TrapSignals ();
                
                if (entry_group != null) {
                    entry_group.Reset ();
                    return;
                }
                
                if (DBusManager.Server.GetState () != AvahiServerState.Running) {
                    throw new ApplicationException ("Avahi Server is not in the Running state");
                }

                ObjectPath path = DBusManager.Server.EntryGroupNew ();
                entry_group = DBusManager.GetObject<IAvahiEntryGroup> (path);
                
                Monitor.Exit (this);
                
                entry_group.StateChanged += OnEntryGroupStateChanged;
            } finally {
                Monitor.Exit (this);
                DBusManager.Bus.UntrapSignals ();
            }
        }
        
        private void OnEntryGroupStateChanged (EntryGroupState state, string error)
        {
            switch (state) {
                case EntryGroupState.Collision:
                    if (!OnResponse (ErrorCode.Collision)) {
                        throw new ApplicationException ();
                    }
                    break;
                case EntryGroupState.Failure:
                    if (!OnResponse (ErrorCode.Failure)) {
                        throw new ApplicationException ();
                    }
                    break;
                case EntryGroupState.Established:
                    OnResponse (ErrorCode.Ok);
                    break;
            }
        }
        
        protected virtual bool OnResponse (ErrorCode errorCode)
        {
            RegisterServiceEventArgs args = new RegisterServiceEventArgs ();
            
            args.Service = this;
            args.IsRegistered = false;
            args.ServiceError = AvahiUtils.ErrorCodeToServiceError (errorCode);
            
            if (errorCode == ErrorCode.Ok) {
                args.IsRegistered = true;
            }
            
            RegisterServiceEventHandler handler = Response;
            if (handler != null) {
                handler (this, args);
                return true;
            }
            
            return false;
        }
        
        public void Dispose ()
        {
            lock (this) {
                if (entry_group != null) {
                    entry_group.Reset ();
                    entry_group.Free ();
                    entry_group = null;
                }
            }
        }
        
        public short Port {
            get { return (short)UPort; }
            set { UPort = (ushort)value; }
        }

        public ushort UPort {
            get { return port; }
            set { port = value; }
        }
    }
}
