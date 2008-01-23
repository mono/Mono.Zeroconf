ASSEMBLY_NAME = MZClient
ASSEMBLY = $(ASSEMBLY_NAME).exe

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY) $(ASSEMBLY).mdb

wrapperdir = $(bindir)
wrapper_SCRIPTS = mzclient

SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(srcdir)/ZeroconfClient.cs

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -debug -out:$@ -r:$< $(SOURCES)

EXTRA_DIST = $(SOURCES) MZClient.csproj
CLEANFILES = $(ASSEMBLY){,.mdb}
MAINTAINERCLEANFILES = \
	Makefile.in

