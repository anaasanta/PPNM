area_uc.txt: Makefile main.exe
	mono main.exe > area_uc.txt
	sed -i 's/,/./g' area_uc.txt

integral.txt: Makefile main.exe
	mono main.exe > integral.txt
	sed -i 's/,/./g' integral.txt

pseuVquaVstra.txt: Makefile main.exe
	mono main.exe > pseuVquaVstra.txt
	sed -i 's/,/./g' pseuVquaVstra.txt

main.exe: main.cs mc.dll vector.dll
	mcs -r:mc.dll -r:vector.dll main.cs -out:main.exe

mc.dll: mc.cs vector.dll
	mcs -target:library -r:vector.dll mc.cs -out:mc.dll

vector.dll: vector.cs
	mcs -target:library vector.cs -out:vector.dll


unit_circle.svg : area_uc.txt
	echo "set terminal svg background rgb 'white';\
	      set output '$@';\
	      set xlabel '1/√N';\
	      set ylabel 'Error';\
	      plot '$<' using 5:3 with linespoints lt rgb 'blue'  title 'Estimated error',\
	           ''    using 5:4 with linespoints lt rgb 'red'   title 'Actual error',\
	           ''    using 5:5 with lines       lt rgb 'green' title '1/√N scaling';" \
	| gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
