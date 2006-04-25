ASM_SOURCES = *.cs
ASM = Mono.Zeroconf.dll
MZCLIENT_SOURCES=../Test/ZeroconfClient.cs
MZCLIENT_ASSEMBLY=mzclient.exe
MONO = mono --debug 
MCS = mcs -debug

all: $(ASM) $(MZCLIENT_ASSEMBLY)

$(ASM): $(ASM_SOURCES)
	$(MCS) -out:$@ -target:library $(ASM_SOURCES)

$(MZCLIENT_ASSEMBLY): $(MZCLIENT_SOURCES)
	$(MCS) -out:$@ -r:$(ASM) $(MZCLIENT_SOURCES)

clean: 
	rm -f $(ASM) $(MZCLIENT_ASSEMBLY) *.mdb *.pidb

run:
	$(MONO) $(MZCLIENT_ASSEMBLY)

install-client:
	mkdir -p /usr/lib/mzclient; \
	cp $(ASM) $(MZCLIENT_ASSEMBLY) $(ASM).config *.mdb /usr/lib/mzclient/;
	cp ../Test/mzclient.in /usr/bin/mzclient;
	chmod 0755 /usr/bin/mzclient;

