Out.txt : Makefile main.exe
	mono main.exe -numbers:1,2,3,4,5 > $@
	echo "-numbers:1,2,3,4,5" > inputfile
	mono main.exe $$(cat inputfile) >> $@

main.exe : main.cs
	mcs -target:exe -out:$@ $<
