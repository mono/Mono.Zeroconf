ASSEMBLY_NAME = Mono.Zeroconf.Providers.Avahi
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_PROVIDERS_AVAHI_SRC = \
	IAvahiServiceBrowser.cs \
	DBusManager.cs \
	IAvahiServer.cs \
	ZeroconfProvider.cs \
	ServiceBrowser.cs

#Mono.Zeroconf.Providers.Avahi/ResolvableService.cs \
#Mono.Zeroconf.Providers.Avahi/ServiceBrowser.cs \
#Mono.Zeroconf.Providers.Avahi/Service.cs \
#Mono.Zeroconf.Providers.Avahi/TxtRecord.cs \
#Mono.Zeroconf.Providers.Avahi/ZeroconfProvider.cs

SOURCES_PRE = \
	$(foreach source,$(MONO_ZEROCONF_PROVIDERS_AVAHI_SRC),Mono.Zeroconf.Providers.Avahi/$(source))

SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(foreach source,$(SOURCES_PRE),$(srcdir)/$(source))

if BUILD_AVAHI

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY)

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -debug -target:library -out:$@ -r:$< $(AVAHI_LIBS) $(SOURCES)

endif

EXTRA_DIST = $(SOURCES)
CLEANFILES = *.dll *.exe *.mdb
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = \
	Makefile.in

