out_exA_data.txt out_exA_fit.txt: LeastSquares.exe
	mono LeastSquares.exe

out_exC_fits.txt: exC.exe
	mono exC.exe

exA.svg: out_exA_data.txt out_exA_fit.txt Makefile
	echo "\
	set terminal svg; \
	set output 'exA.svg'; \
	set key left; \
	set title 'Least Squares Fit of Radioactive Decay'; \
	set xlabel 'Time (days)'; \
	set ylabel 'Activity'; \
	set terminal svg background rgb 'white';\
	stats 'out_exA_data.txt' using 4 skip 1 nooutput; \
	set arrow from STATS_min, graph 0 to STATS_min, graph 1 nohead lc rgb 'red' lw 2; \
	plot 'out_exA_data.txt' using 1:2:3 with yerrorbars title 'Experimental Data', \
	     'out_exA_fit.txt' using 1:2 with lines title 'Best Fit'" | gnuplot


exC.svg: out_exC_fits.txt Makefile
	echo "\
	set terminal svg; \
	set output 'exC.svg'; \
	set key left; \
	set title 'Evaluation of Uncertainties in the Fit'; \
	set xlabel 'Time (days)'; \
	set ylabel 'Activity'; \
	set terminal svg background rgb 'white';\
	plot 'out_exC_fits.txt' using 1:2 with lines title 'Best Fit', \
	     'out_exC_fits.txt' using 1:3 with lines title 'F_{+}', \
	     'out_exC_fits.txt' using 1:4 with lines title 'F_{-}'" | gnuplot

LeastSquares.exe: LeastSquares.cs QRGS.dll matrix.dll vector.dll
	mcs -target:exe -out:LeastSquares.exe -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll LeastSquares.cs

exC.exe: exC.cs QRGS.dll matrix.dll vector.dll LeastSquares.exe
	mcs -target:exe -out:exC.exe -reference:QRGS.dll -reference:matrix.dll -reference:vector.dll -reference:LeastSquares.exe exC.cs

QRGS.dll: QRGS.cs matrix.dll vector.dll
	mcs -target:library -out:QRGS.dll -reference:matrix.dll -reference:vector.dll QRGS.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

clean:
	rm -f *.dll *.exe *.txt *.svg
