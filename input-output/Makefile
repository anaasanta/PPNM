Out.txt : Makefile main.exe
	mono main.exe -numbers:1,2,3,4,5 > $@
	mono main.exe $$(cat inputfile) >> $@

main.exe : main.cs
	mcs -target:exe -out:$@ $<

clean:
	$(RM) main.exe [Oo]out*
