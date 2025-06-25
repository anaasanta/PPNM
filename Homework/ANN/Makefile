Out.txt out_exA.txt out_exB.txt: main.exe
	mono main.exe > $@
	sed -i 's/^\s*//' $@

main.exe: main.cs vector.dll ann.dll integrator.dll 
	mcs -target:exe main.cs -r:vector.dll -r:ann.dll -r:integrator.dll -out:main.exe 
 
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

integrator.dll: integrator.cs
	mcs -target:library integrator.cs -out:integrator.dll

ann.dll: ann.cs vector.dll matrix.dll minimisation.dll QRGS.dll solveODE.dll integrator.dll
	mcs -target:library -out:ann.dll -reference:vector.dll -reference:matrix.dll -reference:minimisation.dll -reference:QRGS.dll -reference:solveODE.dll -reference:integrator.dll ann.cs


exA.svg: out_exA.txt
	echo "set terminal svg; \
   		set output 'exA.svg'; \
		set terminal svg background rgb 'white';\
  		set title 'Exact value vs ANN value'; \
   		set xlabel 'x'; \
   		set ylabel 'f(x)'; \
   		plot 'out_exA.txt' every ::2 using 1:2 with linespoint title 'Exact value',\
			'out_exA.txt' every ::2 using 1:3 with linespoint title 'ANN value';\
	" | gnuplot

exB.svg: out_exB.txt
	echo "set terminal svg; \
		set output 'exB.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Exact value vs ANN value'; \
		set xlabel 'x'; \
		set ylabel 'f(x)'; \
		plot 'out_exB.txt' every ::1 using 1:2 with linespoint title 'Exact value',\
			'out_exB.txt' every ::1 using 1:3 with linespoint title 'ANN value';\
    " | gnuplot

exB_derivative.svg: out_exB.txt
	echo "set terminal svg; \
		set output 'exB_derivative.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Exact value vs ANN value (derivative)'; \
		set xlabel 'x'; \
		set ylabel 'f(x)'; \
		plot 'out_exB.txt' every ::1 using 1:4 with linespoint title 'Exact value',\
			'out_exB.txt' every ::1 using 1:5 with linespoint title 'ANN value';\
	" | gnuplot

exB_second_derivative.svg: out_exB.txt
	echo "set terminal svg; \
		set output 'exB_second_derivative.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Exact value vs ANN value (second derivative)'; \
		set xlabel 'x'; \
		set ylabel 'f(x)'; \
		plot 'out_exB.txt' every ::1 using 1:6 with linespoint title 'Exact value',\
			'out_exB.txt' every ::1 using 1:7 with linespoint title 'ANN value';\
	" | gnuplot

exB_antiderivative.svg: out_exB.txt
	echo "set terminal svg; \
		set output 'exB_antiderivative.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Exact value vs ANN value (antiderivative)'; \
		set xlabel 'x'; \
		set ylabel 'f(x)'; \
		plot 'out_exB.txt' every ::1 using 1:8 with linespoint title 'Exact value',\
			'out_exB.txt' every ::1 using 1:9 with linespoint title 'ANN value';\
	" | gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
