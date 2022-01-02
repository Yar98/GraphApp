if not exist .\bin mkdir .\bin
pushd .\bin

csc -target:winexe -resource:..\imageresource.resources ..\*.cs

popd