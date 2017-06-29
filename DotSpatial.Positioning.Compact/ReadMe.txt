These files are in the form exactly as they were originally committed.

It assumes that tidyup ensured that these files will compile properly 
on Compact Framework, Pocket PC and other obsolete smart device platforms.

Since these platforms are not supported as of VS2010 or beyond, and are 
not supported by any express version of visual studio, and as I understand
it, not functionally supported by SharpDevelop, there is no reliable way
to ensure maintenance for these libraries.  They have therefore been
copied to a separate folder named DotSpatial.Positioning.Compact and
will remain compatible with those platforms, but will need to be updated
independently from the core project which will be maintained as part of 
the .Net 3.5/4.0 based framework.