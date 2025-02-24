CSLIBS = -reference:System.Numerics.dll
CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))

MKEXE = mcs -target:exe -out:$@ $(CSLIBS) $(DLLS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(CSLIBS) $(DLLS) $(CODE)

Out.txt: main.exe
	mono $< > $@
main.exe : main.cs functions.dll
	$(MKEXE)

functions.dll: ../epsilon/functions.cs
	$(MKLIB)
	
clean: 
	$(RM) main.exe [Oo]out* *.dll