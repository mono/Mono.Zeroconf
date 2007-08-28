AC_INIT([Mono.Zeroconf], [0.7.2])
AC_CANONICAL_SYSTEM
AC_PREREQ(2.13)
AM_INIT_AUTOMAKE([1.9 dist-bzip2 tar-ustar])
AM_MAINTAINER_MODE

DISTCHECK_CONFIGURE_FLAGS="--disable-docs"
AC_SUBST(DISTCHECK_CONFIGURE_FLAGS)

ASM_VERSION="1.0.0.0"
AC_SUBST(ASM_VERSION)

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

AC_ARG_ENABLE(avahi, AC_HELP_STRING([--disable-avahi],
	[Do not build with Avahi support]), , enable_avahi=yes)

if test "x$enable_avahi" = "xyes"; then
	PKG_CHECK_MODULES(AVAHI, avahi-sharp >= 0.6.0)
	AC_SUBST(AVAHI_LIBS)
	AM_CONDITIONAL(BUILD_AVAHI, true)
else
	AC_MSG_NOTICE([not building Avahi support])
	AM_CONDITIONAL(BUILD_AVAHI, false)
fi

AC_ARG_ENABLE(mdnsresponder, AC_HELP_STRING([--disable-mdnsresponder],
	[Do not build with mDNSResponder support]), , enable_mdnsresponder=yes)
AM_CONDITIONAL(BUILD_MDNSRESPONDER, test "x$enable_mdnsresponder" = "xyes")

SHAMROCK_EXPAND_LIBDIR

AC_OUTPUT([
Makefile
src/Makefile
src/mono-zeroconf.pc
src/AssemblyInfo.cs
src/Mono.Zeroconf/Makefile
src/Mono.Zeroconf.Providers.Bonjour/Makefile
src/Mono.Zeroconf.Providers.Avahi/Makefile
src/MZClient/Makefile
src/MZClient/mzclient
docs/Makefile
])

echo "
    Installation Prefix:     ${prefix}
    C# Compiler:             ${MCS}

    Avahi Support:           ${enable_avahi}
    mDNSResponder Support:   ${enable_mdnsresponder}

    Developer Documentation: ${enable_docs}
"

