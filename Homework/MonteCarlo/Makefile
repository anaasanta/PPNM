out_area_uc.txt out_exA_integral.txt out_pseuVquaVstra.txt out_exB.txt: Makefile main.exe
	mono main.exe > out_area_uc.txt out_exA_integral.txt out_pseuVquaVstra.txt out_exB.txt
	sed -i 's/,/./g' out_area_uc.txt out_exA_integral.txt out_pseuVquaVstra.txt out_exB.txt

main.exe: main.cs mc.dll vector.dll
	mcs -r:mc.dll -r:vector.dll main.cs -out:main.exe

mc.dll: mc.cs vector.dll
	mcs -target:library -r:vector.dll mc.cs -out:mc.dll

vector.dll: vector.cs
	mcs -target:library vector.cs -out:vector.dll


unit_circle.svg : out_area_uc.txt
	echo "set terminal svg background rgb 'white';\
	      set output '$@';\
		  set title ' Estimated error and the actual error for the area of the unit circle';\
	      set xlabel '1/√N';\
	      set ylabel 'Error';\
	      plot '$<' using 5:3 with linespoints lt rgb 'blue'  title 'Estimated error',\
	           ''    using 5:4 with linespoints lt rgb 'red'   title 'Actual error',\
	           ''    using 5:5 with lines       lt rgb 'green' title '1/√N scaling';" \
	| gnuplot

exB.svg : out_exB.txt
	echo "set terminal svg background rgb 'white';\
	      set output '$@';\
		  set title 'Scaling of the multidimensional error vs pseudo-random error Monte-Carlo integrator';\
	      set xlabel 'N';\
	      set ylabel 'Error';\
	      plot '$<' using 1:3 with linespoints lt rgb 'blue'  title 'Estimated error pr',\
	           ''    using 1:4 with linespoints lt rgb 'red'   title 'Actual error pr',\
			   '' using 1:6 with linespoints lt rgb 'green' title 'Estimated error md',\
				'' using 1:7 with linespoints lt rgb 'orange' title 'Actual error md';"\
		| gnuplot


clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
