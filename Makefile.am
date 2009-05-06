SUBDIRS = \
	src \
	docs

MAINTAINERCLEANFILES = \
	Makefile.in \
	aclocal.m4 \
	configure \
	config.* \
	INSTALL \
	install-sh \
	missing

EXTRA_DIST = \
	mono-zeroconf.spec \
	Mono.Zeroconf.sln \
	README.Windows

collect:
	rm -rf bin; \
	mkdir bin; \
	find src \( -iname *.exe -o -iname *.dll \) -exec cp {} bin \;

PKV=$(PACKAGE)-$(VERSION)
PKB=$(PKV)-binary

dist-windows: all
	@rm -rf $(PKB) && \
	mkdir -p $(PKB)/bin && \
	cp src/MZClient/MZClient.exe src/Mono.Zeroconf/Mono.Zeroconf.dll src/Mono.Zeroconf.Providers.Bonjour/Mono.Zeroconf.Providers.Bonjour.dll $(PKB)/bin && \
	cp AUTHORS NEWS README README.Windows $(PKB) && \
	cp COPYING $(PKB)/LICENSE && \
	zip -r9 $(PKB).zip $(PKB) && \
	rm -rf $(PKB)

release:
	(cd $(top_srcdir)/docs; make update-html) && \
	test -e $(PKV).tar.bz2 -a -e $(PKV).tar.gz -a -e $(PKV).zip -a -e $(PKB).zip || { echo "You must run distcheck and dist-windows before release"; exit 1; }; \
	test -z $$MONO_USER && { echo "Please set MONO_USER for the commit"; exit 1; }; \
	cp ChangeLog $(PKV).changes && \
	sha1sum $(PKV).* $(PKB).* > $(PKV).sha1sum && \
	touch LATEST-IS-$(VERSION) && \
	ssh bansheeweb@banshee-project.org rm /home/bansheeweb/download.banshee-project.org/mono-zeroconf/LATEST-IS*; \
	ssh bansheeweb@banshee-project.org rm -rf /home/bansheeweb/download.banshee-project.org/mono-zeroconf/docs; \
	scp $(PKV).* $(PKB).* mono-zeroconf.spec LATEST* README README.Windows bansheeweb@banshee-project.org:/home/bansheeweb/download.banshee-project.org/mono-zeroconf/ && rm LATEST* $(PKV).* $(PKB).* && \
	scp -r $(top_builddir)/docs/html bansheeweb@banshee-project.org:/home/bansheeweb/download.banshee-project.org/mono-zeroconf/docs && \
	echo "Sleeping 10 seconds, about to tag release (as user $$MONO_USER)..."; \
	sleep 10; \
	echo "Starting svn tag..."; \
	svn cp svn+ssh://$$MONO_USER@mono-cvs.ximian.com/source/trunk/Mono.Zeroconf svn+ssh://$$MONO_USER@mono-cvs.ximian.com/source/tags/Mono.Zeroconf-$(VERSION) -m 'Created tag for Mono.Zeroconf $(VERSION)';

