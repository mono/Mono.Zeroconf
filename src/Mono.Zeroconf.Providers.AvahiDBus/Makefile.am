ASSEMBLY_NAME = Mono.Zeroconf.Providers.AvahiDBus
ASSEMBLY = $(ASSEMBLY_NAME).dll

MONO_ZEROCONF_PROVIDERS_AVAHI_SRC =  \
	Mono.Zeroconf.Providers.AvahiDBus/AvahiServerState.cs \
	Mono.Zeroconf.Providers.AvahiDBus/AvahiUtils.cs \
	Mono.Zeroconf.Providers.AvahiDBus/BrowseService.cs \
	Mono.Zeroconf.Providers.AvahiDBus/DBusManager.cs \
	Mono.Zeroconf.Providers.AvahiDBus/EntryGroupState.cs \
	Mono.Zeroconf.Providers.AvahiDBus/ErrorCode.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiEntryGroup.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiServer.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiServiceBrowser.cs \
	Mono.Zeroconf.Providers.AvahiDBus/IAvahiServiceResolver.cs \
	Mono.Zeroconf.Providers.AvahiDBus/LookupFlags.cs \
	Mono.Zeroconf.Providers.AvahiDBus/LookupResultFlags.cs \
	Mono.Zeroconf.Providers.AvahiDBus/Protocol.cs \
	Mono.Zeroconf.Providers.AvahiDBus/PublishFlags.cs \
	Mono.Zeroconf.Providers.AvahiDBus/RegisterService.cs \
	Mono.Zeroconf.Providers.AvahiDBus/Service.cs \
	Mono.Zeroconf.Providers.AvahiDBus/ServiceBrowser.cs \
	Mono.Zeroconf.Providers.AvahiDBus/TxtRecord.cs \
	Mono.Zeroconf.Providers.AvahiDBus/ZeroconfProvider.cs \
	NDesk.DBus/Address.cs \
	NDesk.DBus/Authentication.cs \
	NDesk.DBus/Bus.cs \
	NDesk.DBus/BusObject.cs \
	NDesk.DBus/Connection.cs \
	NDesk.DBus/DBus.cs \
	NDesk.DBus/DProxy.cs \
	NDesk.DBus/ExportObject.cs \
	NDesk.DBus/Introspection.cs \
	NDesk.DBus/IntrospectionSchema.cs \
	NDesk.DBus/Mapper.cs \
	NDesk.DBus/MatchRule.cs \
	NDesk.DBus/Message.cs \
	NDesk.DBus/MessageFilter.cs \
	NDesk.DBus/MessageReader.cs \
	NDesk.DBus/MessageWriter.cs \
	NDesk.DBus/PendingCall.cs \
	NDesk.DBus/Protocol.cs \
	NDesk.DBus/Server.cs \
	NDesk.DBus/Signature.cs \
	NDesk.DBus/SocketTransport.cs \
	NDesk.DBus/Transport.cs \
	NDesk.DBus/TypeDefiner.cs \
	NDesk.DBus/TypeImplementer.cs \
	NDesk.DBus/UnixMonoTransport.cs \
	NDesk.DBus/UnixNativeTransport.cs \
	NDesk.DBus/UnixTransport.cs \
	NDesk.DBus/Wrapper.cs

SOURCES = \
	$(top_srcdir)/src/AssemblyInfo.cs \
	$(foreach source,$(MONO_ZEROCONF_PROVIDERS_AVAHI_SRC),$(srcdir)/$(source))

if BUILD_AVAHI

assemblydir = $(libdir)/mono-zeroconf
assembly_SCRIPTS = $(ASSEMBLY) $(ASSEMBLY).mdb

all: $(ASSEMBLY)

$(ASSEMBLY): $(top_builddir)/src/Mono.Zeroconf/Mono.Zeroconf.dll $(SOURCES)
	$(MCS) -unsafe -debug -target:library -out:$@ -r:$< $(AVAHI_LIBS) $(SOURCES)

endif

EXTRA_DIST = $(SOURCES)
CLEANFILES = *.dll *.exe *.mdb
DISTCLEANFILES = *.pidb
MAINTAINERCLEANFILES = \
	Makefile.in

