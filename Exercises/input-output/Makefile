Out.txt : Makefile main.exe
	@echo "Running main.exe with input 1 2 3 4 5"
	mono main.exe -numbers:1,2,3,4,5 > $@
	@echo "Running main.exe with inputfile"
	mono main.exe $$(cat inputfile) >> $@

	@echo "Running main.exe with piped input 1 2 3 4 5"
	echo 1 2 3 4 5 | mono main.exe 2>> $@
	@echo "Running main.exe with piped inputfile"
	echo 1 2 3 4 5 > input.txt
	mono main.exe < input.txt 2>> $@

	@echo "Running main.exe with inputfile and outputfile"
	mono main.exe -input:input.txt -output:output.txt


input.txt : Makefile
	echo 1 2 3 4 5 > $@

stdin.exe : stdin.cs
	mcs -target:exe -out:$@ $<

main.exe : main.cs
	mcs -target:exe -out:$@ $<

clean:
	$(RM) main.exe [Oo]out*
