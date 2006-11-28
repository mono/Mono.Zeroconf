ASM = ../Mono.Zeroconf/Mono.Zeroconf.dll
MZCLIENT_SOURCES=../Test/ZeroconfClient.cs
MZCLIENT_ASSEMBLY=mzclient.exe
MONO = mono --debug 
MCS = mcs -debug

all: $(MZCLIENT_ASSEMBLY)

$(MZCLIENT_ASSEMBLY): $(MZCLIENT_SOURCES)
	$(MCS) -out:$@ -r:$(ASM) $(MZCLIENT_SOURCES)
	ln -sf $(ASM){,.config} .

clean: 
	rm -f $(MZCLIENT_ASSEMBLY) *.{dll,config,mdb,pidb}

install-client:
	mkdir -p /usr/lib/mzclient;
	cp $(ASM){,.config,.mdb} $(MZCLIENT_ASSEMBLY){,.config,.mdb} /usr/lib/mzclient/;
	cp mzclient.in /usr/bin/mzclient;
	chmod 0755 /usr/bin/mzclient;

