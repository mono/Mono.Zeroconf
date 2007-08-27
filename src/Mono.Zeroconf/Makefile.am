ASSEMBLY_NAME = Mono.Zeroconf
ASSEMBLY = $(ASSEMBLY_NAME).dll

assemblydir = $(libdir)/mono/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY).mdb

MONO_ZEROCONF_SRC = \
	Mono.Zeroconf/IRegisterService.cs \
	Mono.Zeroconf/IResolvableService.cs \
	Mono.Zeroconf/IServiceBrowser.cs \
	Mono.Zeroconf/IService.cs \
	Mono.Zeroconf/ITxtRecord.cs \
	Mono.Zeroconf/RegisterService.cs \
	Mono.Zeroconf/RegisterServiceEventArgs.cs \
	Mono.Zeroconf/RegisterServiceEventHandler.cs \
	Mono.Zeroconf/ServiceBrowseEventArgs.cs \
	Mono.Zeroconf/ServiceBrowseEventHandler.cs \
	Mono.Zeroconf/ServiceBrowser.cs \
	Mono.Zeroconf/ServiceResolvedEventArgs.cs \
	Mono.Zeroconf/ServiceResolvedEventHandler.cs \
	Mono.Zeroconf/ServiceErrorCode.cs \
	Mono.Zeroconf/TxtRecord.cs \
	Mono.Zeroconf/TxtRecordItem.cs

MONO_ZEROCONF_PROVIDERS_SRC = \
	Mono.Zeroconf.Providers/IZeroconfProvider.cs \
	Mono.Zeroconf.Providers/ZeroconfProviderAttribute.cs \
	Mono.Zeroconf.Providers/ProviderFactory.cs

SOURCES_PRE = \
	$(MONO_ZEROCONF_SRC) \
	$(MONO_ZEROCONF_PROVIDERS_SRC)

SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(foreach source,$(SOURCES_PRE),$(srcdir)/$(source))

all: $(ASSEMBLY)

$(ASSEMBLY): $(SOURCES)
	$(MCS) -debug -keyfile:$(top_srcdir)/src/mono-zeroconf.snk -target:library -out:$@ $(SOURCES)

GACUTIL_ARGS = -package $(PACKAGE) -root $(DESTDIR)$(libdir) -gacdir $(libdir)

install-data-hook:
	$(GACUTIL) -i $(ASSEMBLY) $(GACUTIL_ARGS)

uninstall-hook:
	$(GACUTIL) -u $(ASSEMBLY_NAME) $(GACUTIL_ARGS)

EXTRA_DIST = $(SOURCES)
CLEANFILES = *.dll *.mdb
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = Makefile.in

