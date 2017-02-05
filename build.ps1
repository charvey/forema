nuget restore .\src\forema.sln
&"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" ".\src\forema.sln"
if(-Not(Test-Path docs)) {
	mkdir docs
}
rm docs\*
cd docs
..\src\bin\Debug\forema.exe
cd ..