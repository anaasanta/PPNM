Out.txt: ex1.exe
	mono ex1.exe > Out.txt

ex1.exe: ex1.cs sfuns.dll
	mcs -target:exe -reference:sfuns.dll -out:ex1.exe ex1.cs

sfuns.dll: sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs


