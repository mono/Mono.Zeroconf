AC_INIT([Mono.Zeroconf], [0.9.0])
AC_CANONICAL_SYSTEM
AC_PREREQ(2.13)
AM_INIT_AUTOMAKE([1.9 dist-bzip2 tar-ustar dist-zip])
AM_MAINTAINER_MODE

API_VERSION="4.0.0.90"
POLICY_VERSIONS="1.0 2.0 3.0 4.0"
AC_SUBST(API_VERSION)
AC_SUBST(POLICY_VERSIONS)

DISTCHECK_CONFIGURE_FLAGS="--disable-docs"
AC_SUBST(DISTCHECK_CONFIGURE_FLAGS)

AC_PROG_INSTALL

SHAMROCK_CHECK_MONO_MODULE(1.1.10)
SHAMROCK_FIND_MONO_2_0_COMPILER
SHAMROCK_FIND_MONO_RUNTIME
SHAMROCK_CHECK_MONODOC
SHAMROCK_CHECK_MONO_2_0_GAC_ASSEMBLIES([
	System
	System.Web
])

AC_PATH_PROG(GACUTIL, gacutil, no)
if test "x$GACUTIL" = "xno"; then
	AC_MSG_ERROR([You need to install gacutil])
fi

AC_PATH_PROG(AL, al2, no)
if test "x$AL" = "xno" ; then
	AC_MSG_ERROR([You need to install al])
fi

GACUTIL_FLAGS='-package $(PACKAGE) -root $(DESTDIR)$(prefix)/lib'
GACUTIL_POLICY_FLAGS='-root $(DESTDIR)$(prefix)/lib'
AC_SUBST(GACUTIL_FLAGS)
AC_SUBST(GACUTIL_POLICY_FLAGS)


AC_ARG_ENABLE(avahi, AC_HELP_STRING([--disable-avahi],
	[Do not build with Avahi support]), , enable_avahi=yes)

if test "x$enable_avahi" = "xyes"; then
	AVAHI_LIBS="-r:Mono.Posix"
	AC_SUBST(AVAHI_LIBS)
	AM_CONDITIONAL(BUILD_AVAHI, true)
else
	AC_MSG_NOTICE([not building Avahi support])
	AM_CONDITIONAL(BUILD_AVAHI, false)
fi

AC_ARG_ENABLE(mdnsresponder, AC_HELP_STRING([--disable-mdnsresponder],
	[Do not build with mDNSResponder support]), , enable_mdnsresponder=yes)
AM_CONDITIONAL(BUILD_MDNSRESPONDER, test "x$enable_mdnsresponder" = "xyes")

case ${host} in
	*-*-darwin*)
		mdns_target_library="libc"
		;;
	*)
		mdns_target_library="libnss_mdns-0.2.so"
		;;
esac

AC_SUBST(mdns_target_library)

SHAMROCK_EXPAND_LIBDIR

AC_OUTPUT([
Makefile
src/Makefile
src/policy.config
src/mono-zeroconf.pc
src/Mono.Zeroconf/Makefile
src/Mono.Zeroconf.Providers.Bonjour/Makefile
src/Mono.Zeroconf.Providers.Bonjour/Mono.Zeroconf.Providers.Bonjour.dll.config
src/Mono.Zeroconf.Providers.Avahi/Makefile
src/Mono.Zeroconf.Providers.AvahiDBus/Makefile
src/MZClient/Makefile
src/MZClient/mzclient
docs/Makefile
])

echo "
    Installation Prefix:     ${prefix}
    C# Compiler:             ${MCS}
    Host Platform:           ${host}

    Avahi Support:           ${enable_avahi}
    mDNSResponder Support:   ${enable_mdnsresponder}"

if test "x${enable_mdnsresponder}" = "xyes"; then
	echo "      Target Library:        ${mdns_target_library}"
fi

echo "
    Developer Documentation: ${enable_docs}
"

