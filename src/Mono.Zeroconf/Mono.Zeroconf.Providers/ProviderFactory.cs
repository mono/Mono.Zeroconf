//
// ProviderFactory.cs
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
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Mono.Zeroconf.Providers
{
    internal static class ProviderFactory
    {
        private static IZeroconfProvider [] providers;
        private static IZeroconfProvider selected_provider;
    
        private static IZeroconfProvider DefaultProvider {
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
    
        private static IZeroconfProvider [] GetProviders()
        {
            if(providers != null) {
                return providers;
            }
        
            List<IZeroconfProvider> providers_list = new List<IZeroconfProvider>();
            List<string> directories = new List<string>();
            Assembly asm = Assembly.GetExecutingAssembly();
            
            string env_path = Environment.GetEnvironmentVariable("MONO_ZEROCONF_PROVIDERS");
            if(!String.IsNullOrEmpty(env_path)) {
                foreach(string path in env_path.Split(':')) {
                    if(Directory.Exists(path)) {
                        directories.Add(path);
                    }
                }
            }
            
            string this_asm_path = asm.Location;
            directories.Add(Path.GetDirectoryName(this_asm_path));
            
            if(Assembly.GetExecutingAssembly().GlobalAssemblyCache) {
                string [] path_parts = directories[0].Split(Path.DirectorySeparatorChar);
                string new_path = Path.DirectorySeparatorChar.ToString();
                string root = Path.GetPathRoot (this_asm_path);
                if (root.StartsWith (path_parts[0])) {
                    path_parts[0] = root;
                }

                for(int i = 0; i < path_parts.Length - 4; i++) {
                    new_path = Path.Combine(new_path, path_parts[i]);
                }
                
                directories.Add(Path.Combine(new_path, "mono-zeroconf"));
            }
            
            foreach(string directory in directories) {
                foreach(string file in Directory.GetFiles(directory, "Mono.Zeroconf.Providers.*.dll")) {
                    if(Path.GetFileName(file) != Path.GetFileName(this_asm_path)) {
                        Assembly provider_asm = Assembly.LoadFile(file);
                        foreach(Attribute attr in provider_asm.GetCustomAttributes(false)) {
                            if(attr is ZeroconfProviderAttribute) {
                                Type type = (attr as ZeroconfProviderAttribute).ProviderType;
                                IZeroconfProvider provider = (IZeroconfProvider)Activator.CreateInstance(type);
                                try {
                                    provider.Initialize();
                                    providers_list.Add(provider);
                                } catch (Exception e) {
                                    Console.WriteLine (e);
                                }
                            }
                        }
                    }
                }
            }
            
            if(providers_list.Count == 0) {
                throw new Exception("No Zeroconf providers could be found or initialized. Necessary daemon may not be running.");
            }
            
            providers = providers_list.ToArray();
            
            return providers;
        }
    }
}
