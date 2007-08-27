//
// RegisterService.cs
//
// Authors:
//    Aaron Bockover  <abockover@novell.com>
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
using AV=Avahi;

namespace Mono.Zeroconf.Providers.Avahi
{
    public class RegisterService : IRegisterService
    {
        private string name;
        private string regtype;
        private string reply_domain;
        private short port;
        private ITxtRecord txt_record;
        
        private AV.Client client;
        private AV.EntryGroup entry_group;
        private object eg_mutex = new object();
     
        public event RegisterServiceEventHandler Response;
     
        public RegisterService()
        {
            client = new AV.Client();
        }
        
        public void Register()
        {
            lock(eg_mutex) {
                if(entry_group != null) {
                    entry_group.Reset();
                } else {
                    entry_group = new AV.EntryGroup(client);
                    entry_group.StateChanged += OnEntryGroupStateChanged;
                }
                
                try {
                    string [] rendered_txt_record = null;
                    
                    if(txt_record != null && txt_record.Count > 0) {
                        rendered_txt_record = new string[txt_record.Count];
                        for(int i = 0; i < txt_record.Count; i++) {
                            TxtRecordItem item = txt_record.GetItemAt(i);
                            rendered_txt_record[i] = String.Format("{0}={1}", 
                                item.Key, item.ValueString);
                        }
                    }
                    
                    entry_group.AddService(name, regtype, reply_domain, (ushort)port, rendered_txt_record);
                    entry_group.Commit();
                } catch(AV.ClientException e) {
                    if(e.ErrorCode == AV.ErrorCode.Collision && OnResponse(e.ErrorCode)) {
                        return;
                    } else {
                        throw e;
                    }
                }
            }
        }
        
        public void Dispose()
        {
            lock(eg_mutex) {
                if(entry_group == null) {
                    return;
                }
                
                entry_group.Reset();
                entry_group.Dispose();
                entry_group = null;
            }
        }
        
        private void OnEntryGroupStateChanged(object o, AV.EntryGroupStateArgs args)
        {
            switch(args.State) {
                case AV.EntryGroupState.Collision:
                    if(!OnResponse(AV.ErrorCode.Collision)) {
                        throw new ApplicationException();
                    }
                    break;
                case AV.EntryGroupState.Failure:
                    if(!OnResponse(AV.ErrorCode.Failure)) {
                        throw new ApplicationException();
                    }
                    break;
                case AV.EntryGroupState.Established:
                    OnResponse(AV.ErrorCode.Ok);
                    break;
            }
        }
        
        protected virtual bool OnResponse(AV.ErrorCode errorCode)
        {
            RegisterServiceEventArgs args = new RegisterServiceEventArgs();
            
            args.Service = this;
            args.IsRegistered = false;
            args.ServiceError = AvahiUtils.ErrorCodeToServiceError(errorCode);
            
            if(errorCode == AV.ErrorCode.Ok) {
                args.IsRegistered = true;
            }
            
            RegisterServiceEventHandler handler = Response;
            if(handler != null) {
                handler(this, args);
                return true;
            } else {
                return false;
            }
        }
        
        public string Name { 
            get { return name; }
            set { name = value; }
        }
        
        public string RegType {
            get { return regtype; }
            set { regtype = value; }
        }
        
        public string ReplyDomain { 
            get { return reply_domain; }
            set { reply_domain = value; }
        }
        
        public short Port {
            get { return port; }
            set { port = value; }
        }
        
        public ITxtRecord TxtRecord {
            get { return txt_record; }
            set { txt_record = value; }
        }
    }
}
