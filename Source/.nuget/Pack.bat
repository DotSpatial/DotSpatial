del *.nupkg

Set NugetVersion=2.0.0.0%1
Set PackageVersion=2.0.0.0%1

nuget pack ..\DotSpatial.Controls\DotSpatial.Controls.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Data\DotSpatial.Data.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Data.Forms\DotSpatial.Data.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Extensions\DotSpatial.Extensions.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Modeling.Forms\DotSpatial.Modeling.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.NTSExtension\DotSpatial.NTSExtension.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Projections\DotSpatial.Projections.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Projections.Forms\DotSpatial.Projections.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Serialization\DotSpatial.Serialization.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Symbology\DotSpatial.Symbology.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Symbology.Forms\DotSpatial.Symbology.Forms.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Python\DotSpatial.Python.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Data.Rasters.GdalExtension\DotSpatial.Data.Rasters.GdalExtension.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
nuget pack ..\DotSpatial.Plugins.Measure\DotSpatial.Plugins.Measure.csproj -version "%NugetVersion%" -Properties "PackageVersion=%PackageVersion%";"Configuration=Release";"Platform=AnyCPU"
