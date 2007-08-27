//
// TxtRecord.cs
//
// Authors:
//    Aaron Bockover  <abockover@novell.com>
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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using AV=Avahi;
using Mono.Zeroconf;

namespace Mono.Zeroconf.Providers.Avahi
{
    public class TxtRecord : ITxtRecord
    {
        private ArrayList records = new ArrayList();
        
        private static readonly Encoding encoding = new UTF8Encoding();
    
        public TxtRecord()
        {
        }
    
        public TxtRecord(byte [][] data)
        {
            foreach(byte [] raw_item in data) {
                Regex item_regex = new Regex(@"""[^""]*""|[^,]+", RegexOptions.IgnorePatternWhitespace);
                foreach(Match item_match in item_regex.Matches(encoding.GetString(raw_item))) {
                    string item = item_match.Groups[0].Value;
                    string [] split_item = item.Split(new char [] { '=' }, 2);
                    Add(split_item[0], split_item[1]);
                }
            }
        }
    
        public void Dispose()
        {
        }
    
        public void Add(string key, string value)
        {
            records.Add(new TxtRecordItem(key, value));
        }
        
        public void Add(string key, byte [] value)
        {
            records.Add(new TxtRecordItem(key, value));
        }
        
        public void Add(TxtRecordItem item)
        {
            records.Add(item);
        }
        
        public void Remove(string key)
        {
            TxtRecordItem item = Find(key);
            if(item != null) {
                records.Remove(item);
            }
        }
        
        public TxtRecordItem Find(string key)
        {
            foreach(TxtRecordItem item in records) {
                if(item.Key == key) {
                    return item;
                }
            }
            
            return null;
        }
        
        public TxtRecordItem GetItemAt(int index)
        {
            return records[index] as TxtRecordItem;
        }

        internal byte [][] Render()
        {
            byte [][] items = new byte[records.Count][];
            int index = 0;
            
            foreach(TxtRecordItem item in records) {
                string txt = String.Format("{0}={1}", item.Key, item.ValueString);
                items[index++] = encoding.GetBytes(txt);
            }
            
            return items;
        }
        
        public IEnumerator GetEnumerator()
        {
            return records.GetEnumerator();
        }
        
        public TxtRecordItem this[string key] { 
            get { return Find(key); }
        }
        
        public int Count { 
            get { return records.Count; }
        }
        
        public ITxtRecord BaseRecord { 
            get { return this; }
        }
    }
}
