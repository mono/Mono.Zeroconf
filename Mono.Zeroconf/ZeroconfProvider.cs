//
// ZeroconfProvider.cs
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

namespace Mono.Zeroconf
{
    public interface IZeroconfProvider
    {
        void Initialize();
        Type ServiceBrowser { get; }
        Type RegisterService { get; }
        Type TxtRecord { get; }
    }

    public static class ZeroconfProvider
    {
        private static IZeroconfProvider [] providers;
        private static IZeroconfProvider selected_provider;
    
        public static IZeroconfProvider DefaultProvider {
            get {
                if(providers == null) {
                    GetProviders();
                }
                
                return providers[0];
            }
        }
        
        public static IZeroconfProvider SelectedProvider {
            get { return selected_provider == null ? DefaultProvider : selected_provider; }
            set { selected_provider = value; }
        }
    
        public static IZeroconfProvider [] GetProviders()
        {
            if(providers != null) {
                return providers;
            }
        
            ArrayList providers_list = new ArrayList();
            providers_list.Add(new Mono.Zeroconf.Bonjour.ZeroconfProvider());
            providers = providers_list.ToArray(typeof(IZeroconfProvider)) as IZeroconfProvider [];
            
            return providers;
        }
    }
}
