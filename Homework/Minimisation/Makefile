higgs_data.txt:
	@echo "101 -0.25 2.0"           > $@
	@echo "103 -0.30 2.0"          >> $@
	@echo "105 -0.15 1.9"          >> $@
	@echo "107 -1.71 1.9"          >> $@
	@echo "109  0.81 1.9"          >> $@
	@echo "111  0.65 1.9"          >> $@
	@echo "113 -0.91 1.9"          >> $@
	@echo "115  0.91 1.9"          >> $@
	@echo "117  0.96 1.6"          >> $@
	@echo "119 -2.52 1.6"          >> $@
	@echo "121 -1.01 1.6"          >> $@
	@echo "123  2.01 1.6"          >> $@
	@echo "125  4.83 1.6"          >> $@
	@echo "127  4.58 1.6"          >> $@
	@echo "129  1.26 1.3"          >> $@
	@echo "131  1.01 1.3"          >> $@
	@echo "133 -1.26 1.3"          >> $@
	@echo "135  0.45 1.3"          >> $@
	@echo "137  0.15 1.3"          >> $@
	@echo "139 -0.91 1.3"          >> $@
	@echo "141 -0.81 1.1"          >> $@
	@echo "143 -1.41 1.1"          >> $@
	@echo "145  1.36 1.1"          >> $@
	@echo "147  0.50 1.1"          >> $@
	@echo "149 -0.45 1.1"          >> $@
	@echo "151  1.61 1.1"          >> $@
	@echo "153 -2.21 1.1"          >> $@
	@echo "155 -1.86 0.9"          >> $@
	@echo "157  1.76 0.9"          >> $@
	@echo "159 -0.50 0.9"          >> $@


Out.txt :  Makefile main.exe
	mono main.exe < higgs_data.txt 1> Out.txt 2> log.txt

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

higgs_bosson.svg: higgs_data.txt out_breit_wigner_fit.txt
	echo "\
	set terminal svg background rgb 'white' size 600,400;\
	set output '$@';\
	set datafile separator whitespace;\
	set xlabel 'E';\
	set ylabel 'Signal';\
	set title 'Higgs Boson: Breit–Wigner Fit';\
	plot \
	  'higgs_data.txt'       using 1:2:3 with yerrorbars lt 1 lw 1 pt 7 title 'Data', \
	  'out_breit_wigner_fit.txt' using 1:3   with lines     lt 2 lw 2      title 'Fit';\
	" | gnuplot


clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
