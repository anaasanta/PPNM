Out.txt : Makefile main.exe input.txt
	mono main.exe < input.txt > $@

genlist.dll: genlist.cs
	mcs -target:library -out:genlist.dll genlist.cs

main.exe: main.cs genlist.dll
	mcs -target:exe -out:$@ -reference:genlist.dll main.cs

clean:
	$(RM) main.exe [Oo]out*
