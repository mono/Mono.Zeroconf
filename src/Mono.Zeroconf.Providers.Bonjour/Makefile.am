ASSEMBLY_NAME = Mono.Zeroconf.Providers.Bonjour
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_PROVIDERS_BONJOUR_SRC = \
	BrowseService.cs \
	Native.cs \
	RegisterService.cs \
	ServiceBrowser.cs \
	ServiceClass.cs \
	Service.cs \
	ServiceError.cs \
	ServiceErrorException.cs \
	ServiceFlags.cs \
	ServiceRef.cs \
	ServiceType.cs \
	TxtRecord.cs \
	TxtRecordEnumerator.cs \
	ZeroconfProvider.cs

SOURCES_PRE = \
	$(foreach source,$(MONO_ZEROCONF_PROVIDERS_BONJOUR_SRC),Mono.Zeroconf.Providers.Bonjour/$(source))
SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(foreach source,$(SOURCES_PRE),$(srcdir)/$(source))

if BUILD_MDNSRESPONDER

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY) $(ASSEMBLY).mdb $(ASSEMBLY).config

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -debug -target:library -out:$@ -r:$< $(AVAHI_LIBS) $(SOURCES)

endif

EXTRA_DIST = $(SOURCES) $(ASSEMBLY).config \
	Mono.Zeroconf.Providers.Bonjour.csproj
CLEANFILES = *.dll *.mdb
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = \
	Makefile.in

