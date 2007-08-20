ASM = ../Mono.Zeroconf/Mono.Zeroconf.dll
AVAHI_SOURCES=*.cs
AVAHI_ASSEMBLY=Mono.Zeroconf.Avahi.dll
MCS = mcs -debug

all: $(AVAHI_ASSEMBLY)

$(AVAHI_ASSEMBLY): $(AVAHI_SOURCES)
	$(MCS) -out:$@ -target:library `pkg-config --libs avahi-sharp` -r:$(ASM) $(AVAHI_SOURCES)

clean: 
	rm -f $(AVAHI_ASSEMBLY) *.{dll,config,mdb,pidb}

