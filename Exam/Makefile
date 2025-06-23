Out.txt: main.exe
	mono main.exe > Out.txt
	sed -i 's/\\n/\n/g' Out.txt

integrator.dll: integrator.cs
	mcs -target:library integrator.cs -out:integrator.dll

integrator_3.dll: integrator_3.cs
	mcs -target:library integrator_3.cs -out:integrator_3.dll

main.exe: main.cs integrator.dll integrator_3.dll
	mcs -r:integrator.dll -r:integrator_3.dll main.cs -out:main.exe

erf.svg: erf.txt
	echo "set terminal svg; \
	    set output 'erf.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Comparison of erf(x): computed vs tabulated';\
		set xlabel 'x'; set ylabel 'erf(x)';\
		set key left top;\
		plot 'erf.txt' using 1:2 with linespoints lt rgb 'blue' pt 7 ps 1.0 title 'Computed erf with homework',\
			'erf.txt' using 1:3 with linespoints lt rgb 'red' pt 7 ps 1.0 title 'Computed erf with integrator_3',\
			'erf.txt' using 1:4 with linespoints lt rgb 'green' pt 7 ps 1.0 title 'Exact Erf';\
		" | gnuplot

erf1.svg: erf1.txt
	echo "set terminal svg; \
	    set output 'erf1.svg'; \
		set terminal svg background rgb 'white';\
		set logscale xy;\
	    set title 'absolute value of the difference between erf1 and the exact result as function of acc'; \
	    set xlabel 'acc'; \
	    set ylabel 'err'; \
	    plot 'erf1.txt' using 1:2 with linespoint title 'error in erf(1)',\
			'erf1.txt' using 1:4 with linespoint title 'error in erf(1) with integrator_3';\
		" | gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
