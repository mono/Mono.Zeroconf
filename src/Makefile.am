SUBDIRS = \
	Mono.Zeroconf \
	Mono.Zeroconf.Providers.Bonjour \
	Mono.Zeroconf.Providers.Avahi \
	MZClient

POLICIES = policy.1.0.Mono.Zeroconf.dll
GACUTIL_POLICY_FLAGS = "/gacdir $(libdir) /root $(DESTDIR)$(libdir)"

pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = mono-zeroconf.pc

all: $(POLICIES)

policy.1.0.Mono.Zeroconf.dll: policy.1.0.Mono.Zeroconf.config
	al -link:$< -out:$@ -keyfile:$(srcdir)/mono-zeroconf.snk

install-data-local:
	@for POLICY in $(POLICIES); do \
		echo "$(GACUTIL) /i $$POLICY /f $(GACUTIL_POLICY_FLAGS)"; \
		$(GACUTIL) /i $$POLICY /f $(GACUTIL_POLICY_FLAGS) || exit 1; \
	done

uninstall-hook:
	if test $$(find $(pkglibdir) | wc -l) -eq 1; then \
		rmdir $(pkglibdir); \
	fi

EXTRA_DIST = \
	AssemblyInfo.cs \
	mono-zeroconf.pc.in \
	mono-zeroconf.snk

CLEANFILES = \
	*.dll

DISTCLEANFILES = \
	mono-zeroconf.pc

MAINTAINERCLEANFILES = \
	Makefile.in

