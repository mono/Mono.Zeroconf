ASSEMBLY_NAME = Mono.Zeroconf.Avahi
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_AVAHI_SRC = \
	Mono.Zeroconf.Avahi/ResolvableService.cs \
	Mono.Zeroconf.Avahi/ServiceBrowser.cs \
	Mono.Zeroconf.Avahi/Service.cs \
	Mono.Zeroconf.Avahi/TxtRecord.cs \
	Mono.Zeroconf.Avahi/ZeroconfProvider.cs

SOURCES_PRE = \
	$(MONO_ZEROCONF_AVAHI_SRC)

SOURCES = $(foreach source,$(SOURCES_PRE),$(srcdir)/$(source))

if BUILD_AVAHI

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY)

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -target:library -out:$@ -r:$< $(AVAHI_LIBS) $(SOURCES)

endif

EXTRA_DIST = $(SOURCES)
CLEANFILES = $(ASSEMBLY)
MAINTAINERCLEANFILES = \
	Makefile.in

