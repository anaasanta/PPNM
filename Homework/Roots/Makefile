Out.txt out_wave.txt out_wave_acc_*.txt out_wave_rmax_*.txt out_wave_rmin_*.txt out_wave_eps_*.txt: main.exe
	mono main.exe > Out.txt out_wave.txt out_wave_acc_0.0001.txt out_wave_acc_1E-05.txt out_wave_acc_1E-06.txt out_wave_acc_1E-07.txt out_wave_rmax_10.txt out_wave_rmax_8.txt out_wave_rmax_6.txt out_wave_rmax_4.txt out_wave_rmin_0.001.txt out_wave_rmin_0.0001.txt out_wave_rmin_1E-05.txt out_wave_rmin_1E-06.txt out_wave_eps_0.0001.txt out_wave_eps_1E-05.txt out_wave_eps_1E-06.txt out_wave_eps_1E-07.txt 
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

exB.svg: out_wave.txt
	echo "\
		set terminal svg background rgb 'white';\
		set output '$@';\
		set xlabel 'r';\
		set ylabel 'f(r)';\
		set title 'Schrödinger equation solution for H atom';\
		plot '$<' using 1:2 with linespoints lt rgb 'blue'  title 'Numeric Solution',\
     		''     using 1:3 with linespoints lt rgb 'red'   title 'Exact solution';\
		" | gnuplot

rmax.svg : out_wave_rmax_*.txt
	sed -i 's/,/./g' out_wave_rmax_*.txt
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different rmax values';\
                plot 'out_wave_rmax_10.txt' using 1:2 with lines  lt rgb 'red' title 'value 10',\
                'out_wave_rmax_8.txt' using 1:2 with lines lt rgb 'blue' title 'value 8',\
                'out_wave_rmax_6.txt' using 1:2 with lines lt rgb 'green' title 'value 6',\
                'out_wave_rmax_4.txt' using 1:2 with lines lt rgb 'purple' title 'value 4';\
                "| gnuplot

rmin.svg : out_wave_rmin_*.txt
	sed -i 's/,/./g' out_wave_rmin_*.txt
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different rmin values';\
                plot 'out_wave_rmin_0.001.txt' using 1:2 with lines  lt rgb 'red' title 'value 0.001',\
                'out_wave_rmin_0.0001.txt' using 1:2 with lines lt rgb 'blue' title 'value 0.0001',\
                'out_wave_rmin_1E-05.txt' using 1:2 with lines lt rgb 'green' title 'value 1E-05',\
                'out_wave_rmin_1E-06.txt' using 1:2 with lines lt rgb 'purple' title 'value 1E-06';\
                "| gnuplot

acc.svg : out_wave_acc_*.txt
	sed -i 's/,/./g' out_wave_acc_*.txt
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different acc values';\
                plot 'out_wave_acc_0.0001.txt' using 1:2 with lines  lt rgb 'red' title 'value 0.0001',\
                'out_wave_acc_1E-05.txt' using 1:2 with lines lt rgb 'blue' title 'value 1E-05',\
                'out_wave_acc_1E-06.txt' using 1:2 with lines lt rgb 'green' title 'value 1E-06',\
                'out_wave_acc_1E-07.txt' using 1:2 with lines lt rgb 'purple' title 'value 1E-07';\
                "| gnuplot

eps.svg : out_wave_eps_*.txt
	sed -i 's/,/./g' out_wave_eps_*.txt
	echo "\
                set terminal svg background rgb 'white';\
                set output '$@';\
                set xlabel 'r';\
                set ylabel 'f(r)';\
                set title 'Different eps values';\
                plot 'out_wave_eps_0.0001.txt' using 1:2 with lines  lt rgb 'red' title 'value 0.0001',\
                'out_wave_eps_1E-05.txt' using 1:2 with lines lt rgb 'blue' title 'value 1E-05',\
                'out_wave_eps_1E-06.txt' using 1:2 with lines lt rgb 'green' title 'value 1E-06',\
                'out_wave_eps_1E-07.txt' using 1:2 with lines lt rgb 'purple' title 'value 1E-07';\
                "| gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg

