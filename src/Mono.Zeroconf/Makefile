ASM_SOURCES = Mono.Zeroconf.Bonjour/*.cs *.cs
ASM = Mono.Zeroconf.dll
MONO = mono --debug 
MCS = mcs -debug

all: $(ASM)

$(ASM): $(ASM_SOURCES)
	$(MCS) -out:$@ -target:library $(ASM_SOURCES)

clean: 
	rm -f $(ASM) *.mdb *.pidb

