#
# spec file for package mono-zeroconf (Version 0.8.0)
#
# Copyright (c) 2008 SUSE LINUX Products GmbH, Nuernberg, Germany.
# This file and all modifications and additions to the pristine
# package are under the same license as the package itself.
#
# Please submit bugfixes or comments via http://bugs.opensuse.org/
#

# norootforbuild


Name:           mono-zeroconf
AutoReqProv:    on
License:        X11/MIT
Group:          Development/Languages/Mono
Summary:        A cross platform Zero Configuration Networking library for Mono
Url:            http://mono-project.com/Mono.Zeroconf
Version:        0.8.0
Release:        1
Source0:        %{name}-%{version}.tar.bz2
BuildArch:      noarch
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
BuildRequires:  mono-devel
Requires:       mono-zeroconf-provider
%define assembly_version 3.0.0.80
## --- Build Configuration --- ##
%define build_avahi 1
%define build_mdnsr 1
%define build_docs 1
# openSUSE Configuration
%if 0%{?suse_version}
%if %{suse_version} >= 1030
%define build_avahi 1
%define build_mdnsr 0
#BuildRequires:  avahi-mono
%endif
%if %{suse_version} >= 1020 && %{suse_version} < 1030
%define build_avahi 1
%define build_mdnsr 1
#BuildRequires:  avahi-mono
BuildRequires:  mDNSResponder-devel
%endif
%if %{suse_version} < 1020
%define build_avahi 0
%define build_mdnsr 1
BuildRequires:  mDNSResponder-devel
%endif
%endif
# Fedora Configuration
%if 0%{?fedora_version}
%define env_options export MONO_SHARED_DIR=/tmp
%define build_docs 0
%define build_mdnsr 0
%define build_avahi 1
BuildRequires:  avahi-sharp
%endif
# Mandriva Configuration
%if 0%{?mandriva_version}
%define build_docs 0
%define build_avahi 1
%define build_mdnsr 0
BuildRequires:  avahi-sharp
%endif
%if 0%{?build_docs}
BuildRequires:  monodoc-core
%endif
## --- Base Package Information --- ##

%description
Mono.Zeroconf is a cross platform Zero Configuration Networking library
for Mono and .NET. It provides a unified API for performing the most
common zeroconf operations on a variety of platforms and subsystems:
all the operating systems supported by Mono and both the Avahi and
Bonjour/mDNSResponder transports.



Authors:
--------
    Aaron Bockover <abockover@novell.com>

%files
%defattr(-, root, root)
%doc AUTHORS COPYING ChangeLog NEWS README
%dir %_prefix/lib/mono-zeroconf
%dir %_prefix/lib/mono/mono-zeroconf
%dir %_prefix/lib/mono/gac/Mono.Zeroconf
%dir %_prefix/lib/mono/gac/Mono.Zeroconf/%{assembly_version}__e60c4f4a95e1099e
%dir %_prefix/lib/mono/gac/policy.1.0.Mono.Zeroconf
%dir %_prefix/lib/mono/gac/policy.1.0.Mono.Zeroconf/0.0.0.0__e60c4f4a95e1099e
%dir %_prefix/lib/mono/gac/policy.2.0.Mono.Zeroconf
%dir %_prefix/lib/mono/gac/policy.2.0.Mono.Zeroconf/0.0.0.0__e60c4f4a95e1099e
%dir %_prefix/lib/mono/gac/policy.3.0.Mono.Zeroconf
%dir %_prefix/lib/mono/gac/policy.3.0.Mono.Zeroconf/0.0.0.0__e60c4f4a95e1099e
%_bindir/mzclient
%_prefix/share/pkgconfig/mono-zeroconf.pc
%_prefix/lib/mono/gac/Mono.Zeroconf/*/*.dll*
%_prefix/lib/mono/gac/policy.1.0.Mono.Zeroconf/*/*
%_prefix/lib/mono/gac/policy.2.0.Mono.Zeroconf/*/*
%_prefix/lib/mono/gac/policy.3.0.Mono.Zeroconf/*/*
%_prefix/lib/mono/mono-zeroconf/Mono.Zeroconf.dll*
%_prefix/lib/mono-zeroconf/MZClient.exe*
## --- mDNSResponder Provider --- ##
%if %{build_mdnsr} == 1

%package provider-mDNSResponder
Summary:        A cross platform Zero Configuration Networking library for Mono
Group:          Development/Languages/Mono
BuildRequires:  mDNSResponder-devel
Requires:       mDNSResponder mono-zeroconf
Provides:       mono-zeroconf-provider

%description provider-mDNSResponder
Mono.Zeroconf is a cross platform Zero Configuration Networking library
for Mono and .NET. It provides a unified API for performing the most
common zeroconf operations on a variety of platforms and subsystems:
all the operating systems supported by Mono and both the Avahi and
Bonjour/mDNSResponder transports.



Authors:
--------
    Aaron Bockover <abockover@novell.com>

%files provider-mDNSResponder
%defattr(-, root, root)
%dir %_prefix/lib/mono-zeroconf
%_prefix/lib/mono-zeroconf/Mono.Zeroconf.Providers.Bonjour.dll*
%endif
## --- Avahi Provider --- ##
%if %{build_avahi} == 1

%package provider-avahi
Summary:        A cross platform Zero Configuration Networking library for Mono
Group:          Development/Languages/Mono
Requires:       mono-zeroconf avahi
Provides:       mono-zeroconf-provider

%description provider-avahi
Mono.Zeroconf is a cross platform Zero Configuration Networking library
for Mono and .NET. It provides a unified API for performing the most
common zeroconf operations on a variety of platforms and subsystems:
all the operating systems supported by Mono and both the Avahi and
Bonjour/mDNSResponder transports.



Authors:
--------
    Aaron Bockover <abockover@novell.com>

%files provider-avahi
%defattr(-, root, root)
%dir %_prefix/lib/mono-zeroconf
%_prefix/lib/mono-zeroconf/Mono.Zeroconf.Providers.AvahiDBus.dll*
%endif
## --- Monodoc Developer API Documentation --- ##
%if %{build_docs} == 1

%package doc
Summary:        A cross platform Zero Configuration Networking library for Mono
Group:          Development/Languages/Mono

%description doc
Mono.Zeroconf is a cross platform Zero Configuration Networking library
for Mono and .NET. It provides a unified API for performing the most
common zeroconf operations on a variety of platforms and subsystems:
all the operating systems supported by Mono and both the Avahi and
Bonjour/mDNSResponder transports.



Authors:
--------
    Aaron Bockover <abockover@novell.com>

%files doc
%defattr(-, root, root)
%dir %_prefix/lib/monodoc/sources/
%_prefix/lib/monodoc/sources/mono-zeroconf-docs*
%endif
## --- Build/Install --- #

%prep
%setup -q

%build
%{?env_options}
./configure --prefix=/usr \
%if %{build_docs} == 0
	--disable-docs \
%else
	--enable-docs \
%endif
%if %{build_avahi} == 0
	--disable-avahi \
%else
	--enable-avahi \
%endif
%if %{build_mdnsr} == 0
	--disable-mdnsresponder
%else
	--enable-mdnsresponder
%endif
make

%install
%{?env_options}
make install DESTDIR=$RPM_BUILD_ROOT
mkdir -p $RPM_BUILD_ROOT/usr/share/pkgconfig
mv $RPM_BUILD_ROOT/usr/lib/pkgconfig/* $RPM_BUILD_ROOT/usr/share/pkgconfig

%clean
rm -rf $RPM_BUILD_ROOT

%changelog
* Fri Mar 21 2008 abockover@suse.de
- Updated to version 0.7.6
- Adds NetworkInterface API to IService objects
- Supports IPv6 host address resolutions
* Fri Jan 25 2008 abockover@suse.de
- Updated to version 0.7.5
- Adds GAC version policy assemblies so package upgrades don't break apps
* Wed Jan 23 2008 abockover@suse.de
- Updated to version 0.7.4
- Fixes IP address resolution bug in Bonjour provider that
  only manifested under .NET on Windows XP SP2
- Minor bug fix in MZClient
- libdir patch removed from package; fixed in upstream release
* Fri Dec 28 2007 abockover@suse.de
- Patch to fix libdir issue in mono-zeroconf.pc on x86_64
* Sun Dec 23 2007 abockover@suse.de
- Initial import of Mono.Zeroconf for STABLE from the build serivce
- Version 0.7.3 release
- Provides a cross platform (Linux, Mac, Windows) Mono/.NET API
  for Zero Configuration networking supporting either Bonjour
  or Avahi mDNS providers
