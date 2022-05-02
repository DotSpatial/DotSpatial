cd ..
call Compile_BuildTools2019.cmd

cd .nuGet

del *.nupkg

Set NugetVersion=4.0.0
Set PackageVersion=4.0.0

nuget pack ..\Core\DotSpatial.Analysis\DotSpatial.Analysis.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Compatibility\DotSpatial.Compatibility.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Controls\DotSpatial.Controls.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Data\DotSpatial.Data.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Data.Forms\DotSpatial.Data.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Extensions\DotSpatial.Extensions.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Modeling.Forms\DotSpatial.Modeling.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.NTSExtension\DotSpatial.NTSExtension.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Positioning\DotSpatial.Positioning.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Positioning.Design\DotSpatial.Positioning.Design.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Positioning.Forms\DotSpatial.Positioning.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Projections\DotSpatial.Projections.nuspec -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Projections.Forms\DotSpatial.Projections.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Serialization\DotSpatial.Serialization.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Symbology\DotSpatial.Symbology.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release" 
nuget pack ..\Core\DotSpatial.Symbology.Forms\DotSpatial.Symbology.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release"