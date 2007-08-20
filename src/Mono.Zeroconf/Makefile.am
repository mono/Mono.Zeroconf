ASSEMBLY_NAME = Mono.Zeroconf
ASSEMBLY = $(ASSEMBLY_NAME).dll

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY) $(ASSEMBLY).config

MONO_ZEROCONF_SRC = \
	Mono.Zeroconf/IRegisterService.cs \
	Mono.Zeroconf/IResolvableService.cs \
	Mono.Zeroconf/IServiceBrowser.cs \
	Mono.Zeroconf/IService.cs \
	Mono.Zeroconf/ITxtRecord.cs \
	Mono.Zeroconf/RegisterService.cs \
	Mono.Zeroconf/ServiceBrowser.cs \
	Mono.Zeroconf/TxtRecord.cs \
	Mono.Zeroconf/TxtRecordItem.cs \
	Mono.Zeroconf/ZeroconfProvider.cs

MONO_ZEROCONF_BONJOUR_SRC = \
	Mono.Zeroconf.Bonjour/BrowseService.cs \
	Mono.Zeroconf.Bonjour/Native.cs \
	Mono.Zeroconf.Bonjour/RegisterService.cs \
	Mono.Zeroconf.Bonjour/ServiceBrowser.cs \
	Mono.Zeroconf.Bonjour/ServiceClass.cs \
	Mono.Zeroconf.Bonjour/Service.cs \
	Mono.Zeroconf.Bonjour/ServiceError.cs \
	Mono.Zeroconf.Bonjour/ServiceErrorException.cs \
	Mono.Zeroconf.Bonjour/ServiceFlags.cs \
	Mono.Zeroconf.Bonjour/ServiceRef.cs \
	Mono.Zeroconf.Bonjour/ServiceType.cs \
	Mono.Zeroconf.Bonjour/TxtRecord.cs \
	Mono.Zeroconf.Bonjour/TxtRecordEnumerator.cs \
	Mono.Zeroconf.Bonjour/ZeroconfProvider.cs

SOURCES_PRE = \
	$(MONO_ZEROCONF_SRC) \
	$(MONO_ZEROCONF_BONJOUR_SRC)

SOURCES = $(foreach source,$(SOURCES_PRE),$(srcdir)/$(source))

all: $(ASSEMBLY)

$(ASSEMBLY): $(SOURCES)
	$(MCS) -target:library -out:$@ $(SOURCES)

EXTRA_DIST = $(SOURCES) $(ASSEMBLY).config
CLEANFILES = $(ASSEMBLY)
MAINTAINERCLEANFILES = \
	Makefile.in

