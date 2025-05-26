exA.txt: Makefile exA.exe
	mono exA.exe > exA.txt
	sed -i 's/,/./g' exA.txt
erf.txt: Makefile exA.exe
	mono exA.exe erf > erf.txt
	sed -i 's/,/./g' erf.txt

erf1.txt: Makefile exA.exe
	mono exA.exe erf1 > erf1.txt
	sed -i 's/,/./g' erf1.txt

exA.exe: exA.cs integrator.dll
	mcs -r:integrator.dll exA.cs -out:exA.exe
	
integrator.dll: integrator.cs
	mcs -target:library integrator.cs -out:integrator.dll
	
erf.svg: erf.txt
	echo "set terminal svg; \
	    set output 'erf.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Comparison of erf(x): computed vs tabulated';\
		set xlabel 'x'; set ylabel 'erf(x)';\
		set key left top;\
		plot 'erf.txt' using 1:2 with linespoints lt rgb 'blue' pt 7 ps 1.0 title 'Computed erf',\
  			 'erf.txt' using 1:3 with linespoints lt rgb 'red'  pt 7 ps 1.0 title 'MathNet Erf'\
		" | gnuplot

erf1.svg: erf1.txt
	echo "set terminal svg; \
	    set output 'erf1.svg'; \
		set terminal svg background rgb 'white';\
		set logscale xy;\
	    set title 'absolute value of the difference between erf1 and the exact result as function of acc'; \
	    set xlabel 'acc'; \
	    set ylabel 'err'; \
	    plot 'erf1.txt' using 1:2 with linespoint title 'error in erf(1)" | gnuplot
	
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
