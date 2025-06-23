Out.txt: main.exe
	mono main.exe < higgs.data.txt 1> out.txt 2> log.txt

minimisation.dll: minimisation.cs QRGS.dll matrix.dll vector.dll solveODE.dll
	mcs -target:library -out:minimisation.dll -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll minimisation.cs

QRGS.dll: QRGS.cs matrix.dll vector.dll
	mcs -target:library -out:QRGS.dll -reference:matrix.dll -reference:vector.dll QRGS.cs

solveODE.dll: solveODE.cs vector.dll
	mcs -target:library -out:solveODE.dll -reference:vector.dll solveODE.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

main.exe: main.cs minimisation.dll QRGS.dll matrix.dll vector.dll solveODE.dll
	mcs -out:main.exe -reference:minimisation.dll -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll main.cs

higgs_bosson.svg: higgs.data.txt breit_wigner_fit.dat
	echo "\
	set terminal svg background rgb 'white' size 600,400;\
	set output '$@';\
	set datafile separator whitespace;\
	set xlabel 'E [GeV]';\
	set ylabel 'Signal \\sigma';\
	set title 'Higgs Boson: Breit–Wigner Fit';\
	plot \
	  'higgs.data.txt'       using 1:2:3 with yerrorbars lt 1 lw 1 pt 7 title 'Data', \
	  'breit_wigner_fit.dat' using 1:3   with lines     lt 2 lw 2      title 'Fit';\
	" | gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
