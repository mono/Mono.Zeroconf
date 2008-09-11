SUBDIRS = \
	Mono.Zeroconf \
	Mono.Zeroconf.Providers.Bonjour \
	Mono.Zeroconf.Providers.AvahiDBus \
	Mono.Zeroconf.Providers.Avahi \
	MZClient

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = mono-zeroconf.pc

uninstall-hook:
	if test $$(find $(pkglibdir) | wc -l) -eq 1; then \
		rmdir $(pkglibdir); \
	fi

EXTRA_DIST = \
	AssemblyInfo.cs \
	mono-zeroconf.pc.in \
	mono-zeroconf.snk \
	policy.config.in

CLEANFILES = \
	*.dll

DISTCLEANFILES = \
	mono-zeroconf.pc

MAINTAINERCLEANFILES = \
	Makefile.in

