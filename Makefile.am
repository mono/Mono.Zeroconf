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

collect:
	rm -rf bin; \
	mkdir bin; \
	find src \( -iname *.exe -o -iname *.dll \) -exec cp {} bin \;

PKV=$(PACKAGE)-$(VERSION)

dist-windows: all
	@rm -rf $(PKV) && \
	mkdir -p $(PKV)/bin && \
	cp src/MZClient/MZClient.exe src/Mono.Zeroconf/Mono.Zeroconf.dll src/Mono.Zeroconf.Providers.Bonjour/Mono.Zeroconf.Providers.Bonjour.dll $(PKV)/bin && \
	cp AUTHORS NEWS README $(PKV) && \
	cp COPYING $(PKV)/LICENSE && \
	zip -r9 $(PKV).zip $(PKV) && \
	rm -rf $(PKV)

release:
	@test -z $$MONO_USER && { echo "Please set MONO_USER for the commit"; exit 1; }; \
	test -e $(PKV).tar.bz2 -a -e $(PKV).tar.gz -a -e $(PKV).zip || { echo "You must run distcheck and dist-windows before release"; exit 1; }; \
	cp ChangeLog $(PKV).changes && \
	sha1sum $(PKV).* > $(PKV).sha1sum && \
	touch LATEST-IS-$(VERSION) && \
	ssh banshee@banshee-project.org rm /home/banshee/public_html/files/mono-zeroconf/LATEST-IS*; \
	scp $(PKV).* LATEST* README banshee@banshee-project.org:public_html/files/mono-zeroconf/ && rm LATEST* $(PKV).* && \
	echo "Sleeping 10 seconds, about to tag release (as user $$MONO_USER)..."; \
	sleep 10; \
	echo "Starting svn tag..."; \
	svn cp svn+ssh://$$MONO_USER@mono-cvs.ximian.com/source/trunk/Mono.Zeroconf svn+ssh://$$MONO_USER@mono-cvs.ximian.com/source/tags/Mono.Zeroconf-$(VERSION) -m 'Created tag for Mono.Zeroconf $(VERSION)';

