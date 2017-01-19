if(-Not(Test-Path docs)) {
	mkdir docs
}
rm docs\*
cd docs
..\src\bin\Debug\forema.exe
cd ..