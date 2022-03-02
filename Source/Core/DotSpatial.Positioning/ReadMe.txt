Todo:
Add the designers to the global assembly cache - on XP we could just do it with a post build step 
but on Vista/Win7  this needs administrator priveledges:

runas /user:administrator "$(DevEnvDir)..\..\..\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\x64\gacutil.exe" /if "$(TargetPath)"