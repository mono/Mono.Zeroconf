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

release:
	@test -z $$MONO_USER && { echo "Please set MONO_USER for the commit"; exit 1; }; \
	cp ChangeLog $(PACKAGE)-$(VERSION).changes && \
	sha1sum $(PACKAGE)-$(VERSION).* > $(PACKAGE)-$(VERSION).sha1sum && \
	touch LATEST-IS-$(VERSION) && \
	scp $(PACKAGE)-$(VERSION).* LATEST* README abock.org:/home/aaron/web/abock.org/projects/mono-zeroconf/ && rm LATEST* $(PACKAGE)-$(VERSION).* && \
	echo "Sleeping 10 seconds, about to tag release..."; \
	sleep 10; \
	svn cp $$MONO_USER@mono-cvs.ximian.com/source/trunk/Mono.Zeroconf $$MONO_USER@mono-cvs.ximian.com/source/tags/Mono.Zeroconf-$(VERSION) -m 'Created tag for Mono.Zeroconf $(VERSION)';

