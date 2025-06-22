Out.txt: main.exe
	mono main.exe > Out.txt
	sed -i 's/,/./g' Out.txt

exA.dll: exA.cs QRGS.dll matrix.dll vector.dll solveODE.dll
	mcs -target:library -out:exA.dll -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll exA.cs

exB.dll: exB.cs QRGS.dll matrix.dll vector.dll solveODE.dll
	mcs -target:library -out:exB.dll -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll exB.cs

exC.dll: exC.cs QRGS.dll matrix.dll vector.dll solveODE.dll
	mcs -target:library -out:exC.dll -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll exC.cs

QRGS.dll: QRGS.cs matrix.dll vector.dll
	mcs -target:library -out:QRGS.dll -reference:matrix.dll -reference:vector.dll QRGS.cs

solveODE.dll: solveODE.cs vector.dll
	mcs -target:library -out:solveODE.dll -reference:vector.dll solveODE.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

main.exe: main.cs QRGS.dll matrix.dll vector.dll solveODE.dll exA.dll exB.dll exC.dll
	mcs -target:exe -out:main.exe -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:solveODE.dll -reference:exA.dll -reference:exB.dll -reference:exC.dll main.cs

exB.svg: wave.dat
	echo "\
		set terminal svg background rgb 'white';\
		set output '$@';\
		set xlabel 'r';\
		set ylabel 'f(r)';\
		set title 'Schrödinger equation solution for H atom';\
		plot '$<' using 1:2 with linespoints lt rgb 'blue'  title 'Numeric Solution',\
     		''     using 1:3 with linespoints lt rgb 'red'   title 'Exact solution';\
		" | gnuplot

rmax.svg : wave_rmax_*.dat
	sed -i 's/,/./g' wave_rmax_*.dat
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different rmax values';\
                plot 'wave_rmax_10.dat' using 1:2 with lines  lt rgb 'red' title 'value 10',\
                'wave_rmax_8.dat' using 1:2 with lines lt rgb 'blue' title 'value 8',\
                'wave_rmax_6.dat' using 1:2 with lines lt rgb 'green' title 'value 6',\
                'wave_rmax_4.dat' using 1:2 with lines lt rgb 'purple' title 'value 4';\
                "| gnuplot

rmin.svg : wave_rmin_*.dat
	sed -i 's/,/./g' wave_rmin_*.dat
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different rmin values';\
                plot 'wave_rmin_0.001.dat' using 1:2 with lines  lt rgb 'red' title 'value 0.001',\
                'wave_rmin_0.0001.dat' using 1:2 with lines lt rgb 'blue' title 'value 0.0001',\
                'wave_rmin_1E-05.dat' using 1:2 with lines lt rgb 'green' title 'value 1E-05',\
                'wave_rmin_1E-06.dat' using 1:2 with lines lt rgb 'purple' title 'value 1E-06';\
                "| gnuplot

acc.svg : wave_acc_*.dat
	sed -i 's/,/./g' wave_acc_*.dat
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different acc values';\
                plot 'wave_acc_0.0001.dat' using 1:2 with lines  lt rgb 'red' title 'value 0.0001',\
                'wave_acc_1E-05.dat' using 1:2 with lines lt rgb 'blue' title 'value 1E-05',\
                'wave_acc_1E-06.dat' using 1:2 with lines lt rgb 'green' title 'value 1E-06',\
                'wave_acc_1E-07.dat' using 1:2 with lines lt rgb 'purple' title 'value 1E-07';\
                "| gnuplot

eps.svg : wave_eps_*.dat
	sed -i 's/,/./g' wave_eps_*.dat
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different eps values';\
                plot 'wave_eps_0.0001.dat' using 1:2 with lines  lt rgb 'red' title 'value 0.0001',\
                'wave_eps_1E-05.dat' using 1:2 with lines lt rgb 'blue' title 'value 1E-05',\
                'wave_eps_1E-06.dat' using 1:2 with lines lt rgb 'green' title 'value 1E-06',\
                'wave_eps_1E-07.dat' using 1:2 with lines lt rgb 'purple' title 'value 1E-07';\
                "| gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
