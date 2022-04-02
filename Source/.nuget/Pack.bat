cd ..
call Compile_BuildTools2019.cmd

cd .nuGet

del *.nupkg

Set NugetVersion=2.0.1.2
Set PackageVersion=2.0.1.2

nuget pack ..\DotSpatial.Analysis\DotSpatial.Analysis.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Compatibility\DotSpatial.Compatibility.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Controls\DotSpatial.Controls.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Data\DotSpatial.Data.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Data.Forms\DotSpatial.Data.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Extensions\DotSpatial.Extensions.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Modeling.Forms\DotSpatial.Modeling.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.NTSExtension\DotSpatial.NTSExtension.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Positioning\DotSpatial.Positioning.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Positioning.Design\DotSpatial.Positioning.Design.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Positioning.Forms\DotSpatial.Positioning.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Projections\DotSpatial.Projections.nuspec -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Projections.Forms\DotSpatial.Projections.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Serialization\DotSpatial.Serialization.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Symbology\DotSpatial.Symbology.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\DotSpatial.Symbology.Forms\DotSpatial.Symbology.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release"