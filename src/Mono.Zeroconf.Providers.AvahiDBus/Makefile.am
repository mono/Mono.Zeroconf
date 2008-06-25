ASSEMBLY_NAME = Mono.Zeroconf.Providers.AvahiDBus
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_PROVIDERS_AVAHI_SRC =  \
	Mono.Zeroconf.Providers.AvahiDBus/DBusManager.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiServer.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiServiceBrowser.cs \
	Mono.Zeroconf.Providers.AvahiDBus/ServiceBrowser.cs \
	Mono.Zeroconf.Providers.AvahiDBus/ZeroconfProvider.cs

SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(foreach source,$(MONO_ZEROCONF_PROVIDERS_AVAHI_SRC),$(srcdir)/$(source))

if BUILD_AVAHI

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY) $(ASSEMBLY).mdb

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -debug -target:library -out:$@ -r:$< $(AVAHI_LIBS) $(SOURCES) -pkg:ndesk-dbus-1.0 -pkg:ndesk-dbus-glib-1.0 -pkg:glib-sharp-2.0

endif

EXTRA_DIST = $(SOURCES)
CLEANFILES = *.dll *.exe *.mdb
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = \
	Makefile.in

