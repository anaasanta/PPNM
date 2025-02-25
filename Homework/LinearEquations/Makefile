Out.txt: main.exe
	mono main.exe > Out.txt

main.exe: main.cs vector.dll matrix.dll QRGS.dll
	mcs -target:exe -out:main.exe -reference:vector.dll -reference:matrix.dll -reference:QRGS.dll main.cs

QRGS.dll: QRGS.cs matrix.dll vector.dll
	mcs -target:library -out:QRGS.dll -reference:matrix.dll -reference:vector.dll QRGS.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

clean:
	rm -f *.dll *.exe Out.txt