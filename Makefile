SUBDIRS = Mono.Zeroconf Test
LIBRARY_SOURCES_FILE = Mono.Zeroconf.dll.sources
LIBRARY_BUILD_SOURCES = \
	Assembly/AssemblyInfo.cs \
	../../build/common/Consts.cs \
	../../build/common/Locale.cs

all:
	@for i in $(SUBDIRS); do (cd $$i && $(MAKE)) || exit 1; done

clean:
	@rm -f *.pidb && for i in $(SUBDIRS); do (cd $$i && $(MAKE) clean) || exit 1; done

update-sources:
	(for source in $(LIBRARY_BUILD_SOURCES); do (echo "$$source"); done) > $(LIBRARY_SOURCES_FILE); \
	(find Mono.Zeroconf -iregex ".*\.cs$$") >> $(LIBRARY_SOURCES_FILE)
