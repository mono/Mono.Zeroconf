DOC_UPDATER = @MONODOCER@ -delete -namespace:Mono.Zeroconf
DOC_ASSEMBLER = @MDASSEMBLER@ --out mono-zeroconf-docs --ecma

ASSEMBLIES = \
	$(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll

ASSEMBLIES_BUILD = $(foreach asm,$(ASSEMBLIES),$(addprefix -assembly:,$(asm)))

if BUILD_DOCS

all: MonodocNodeConfig.exe

MonodocNodeConfig.exe: MonodocNodeConfig.cs
	mcs -out:$@ -r:System.Xml $<

monodocdir = $(DESTDIR)$(DOCDIR)
monodoc_DATA = \
	mono-zeroconf-docs.zip \
	mono-zeroconf-docs.tree \
	mono-zeroconf-docs.source

mono-zeroconf-docs.zip mono-zeroconf-docs.tree: $(srcdir)/en/*/*.xml
	$(DOC_ASSEMBLER) $(srcdir)/en

install-data-hook:
	$(MONO) $(top_builddir)/docs/MonodocNodeConfig.exe --insert "Mono.Zeroconf Libraries" classlib-mono-zeroconf $(DOCDIR)/../monodoc.xml

uninstall-hook:
	$(MONO) $(top_builddir)/docs/MonodocNodeConfig.exe --remove classlib-mono-zeroconf $(DOCDIR)/../monodoc.xml

update-docs: $(ASSEMBLIES)
	mkdir -p en/Mono.Zeroconf
	$(DOC_UPDATER) $(ASSEMBLIES_BUILD) -path:en/

endif

EXTRA_DIST = \
	$(srcdir)/en/*/*.xml \
	mono-zeroconf-docs.source \
	MonodocNodeConfig.cs
	
DISTCLEANFILES = \
	mono-zeroconf-docs.zip \
	mono-zeroconf-docs.tree

MAINTAINERCLEANFILES = \
	Makefile.in

CLEANFILES = \
	MonodocNodeConfig.exe

