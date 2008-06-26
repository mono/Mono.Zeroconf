ASSEMBLY_NAME = Mono.Zeroconf
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_SRC = \
	Mono.Zeroconf/AddressProtocol.cs \
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

$(ASSEMBLY): $(SOURCES) $(POLICY_ASSEMBLIES)
	$(MCS) -debug -keyfile:$(top_srcdir)/src/mono-zeroconf.snk -target:library -out:$@ $(SOURCES)

install-data-local: gac-install

uninstall-local: gac-uninstall

include $(top_srcdir)/src/Makefile.include

EXTRA_DIST = $(SOURCES) Mono.Zeroconf.csproj
CLEANFILES = *.dll *.mdb $(POLICY_ASSEMBLIES) $(POLICY_CONFIGS)
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = Makefile.in

