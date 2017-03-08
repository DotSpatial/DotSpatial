The contained files can be updated based on the files of the nad folder of proj.4 project (https://github.com/OSGeo/proj.4/tree/master/nad).

After updating the .txt files in the AuthorityCodes folder

DeflateStreamUtility ..\AuthorityCodes\epsg.txt 

has to be called from command line to update the corresponding .ds file.

This must be done for all manipulated . txt files.