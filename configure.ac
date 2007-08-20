AC_INIT([Mono.Zeroconf], [0.8.1])
AC_CANONICAL_SYSTEM
AC_PREREQ(2.13)
AM_INIT_AUTOMAKE([1.9 dist-bzip2 tar-ustar])
AM_MAINTAINER_MODE

DISTCHECK_CONFIGURE_FLAGS="--disable-docs"
AC_SUBST(DISTCHECK_CONFIGURE_FLAGS)

ASM_VERSION="$VERSION.*"
AC_SUBST(ASM_VERSION)

AC_PROG_INSTALL

SHAMROCK_CHECK_MONO_MODULE(1.1.10)
SHAMROCK_FIND_MONO_2_0_COMPILER
SHAMROCK_FIND_MONO_RUNTIME
SHAMROCK_CHECK_MONODOC
SHAMROCK_CHECK_MONO_2_0_GAC_ASSEMBLIES([
	System
	System.Web
	Mono.Posix
])

AC_ARG_ENABLE(avahi, AC_HELP_STRING([--disable-avahi],
	[Do not build with Avahi support]), , enable_avahi=yes)

if test "x$enable_avahi" = "xyes"; then
	PKG_CHECK_MODULES(AVAHI, avahi-sharp >= 0.6)
	AC_SUBST(AVAHI_LIBS)
	AM_CONDITIONAL(BUILD_AVAHI, true)
else
	AC_MSG_NOTICE([not building Avahi support])
	AM_CONDITIONAL(BUILD_AVAHI, false)
fi

SHAMROCK_EXPAND_LIBDIR

AC_OUTPUT([
Makefile
src/Makefile
src/mono-zeroconf.pc
src/Mono.Zeroconf/Makefile
src/Mono.Zeroconf.Avahi/Makefile
src/MZClient/Makefile
src/MZClient/mzclient
docs/Makefile
])

echo "
    Installation Prefix:  ${prefix}
    C# Compiler:          ${MCS}

    Avahi Support:        ${enable_avahi}
"

