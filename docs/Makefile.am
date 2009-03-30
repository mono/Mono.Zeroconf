DOC_UPDATER = @MONODOCER@ -delete
DOC_ASSEMBLER = @MDASSEMBLER@ --out mono-zeroconf-docs --ecma

ASSEMBLIES = \
	$(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll

ASSEMBLIES_BUILD = $(foreach asm,$(ASSEMBLIES),$(addprefix -assembly:,$(asm)))

if BUILD_DOCS

monodocdir = $(DOCDIR)
monodoc_DATA = \
	mono-zeroconf-docs.zip \
	mono-zeroconf-docs.tree \
	mono-zeroconf-docs.source

mono-zeroconf-docs.zip mono-zeroconf-docs.tree: $(srcdir)/en/*/*.xml $(srcdir)/en/*.xml
	$(DOC_ASSEMBLER) $(srcdir)/en

update-docs: $(ASSEMBLIES)
	$(DOC_UPDATER) $(ASSEMBLIES_BUILD) -path:en/

update-html:
	rm -rf html
	monodocs2html -source:$(srcdir)/en -dest:$(srcdir)/html

update-svn:
	@for remove in $$(find en -iregex .*\.remove$$); do \
		real_remove=$${remove%.remove}; \
		mv $$remove $$real_remove; \
		svn delete $$real_remove; \
	done; \
	for add in $$(svn status | grep ^? | awk '{print $$2}'); do \
		svn add $$add; \
	done;

endif

merge:
	monodoc --merge-changes $$HOME/.config/monodoc/changeset.xml .

EXTRA_DIST = \
	$(srcdir)/en/*/*.xml \
	$(srcdir)/en/*.xml \
	mono-zeroconf-docs.source
	
DISTCLEANFILES = \
	mono-zeroconf-docs.zip \
	mono-zeroconf-docs.tree

MAINTAINERCLEANFILES = \
	Makefile.in

