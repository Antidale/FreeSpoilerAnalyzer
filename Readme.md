# Free Enterprise Spoiler Analyzer

This project is constructed to analyze spoiler logs for Free Enterprise in a way that helps validate the effect of code changes that require generating lots of seeds to see statistical changes. 

This is **not** a project to help in spoiler log races, and as such will generally work with aggregated data. Also, the parser looks for files ending in .spoiler.private, which is what the command line generator for FE will supply when generating seeds. 

## Usage
Run the command line application, supplying any Folders that contain spoiler logs to analyze. When finished it will report on the location of each key item.

## Possible Future Updates
 * Add support for correctly analyzing seeds with Knofree enabled - currently s
 * Add support for selecting specific key items at run time
 * Add support for saving results to a database
 * Add support for reading the flags and changing behavior based on them (mostly for knofree:package and knofree w/wo Bunsafe)
 * Add support for requesting what reporting you want
 * Add support for writing results out to a .csv
 * Add better (read: any) command line flags to accomplish a lot of the above
 * Add a settings file to handle behavior change/reduce the need to pass arguments to the cli


## Examples

#### Mass seed generation (powershell example)
```powershell
for ($i = 0; $i -lt 40000; $i++) {
> python -m FreeEnt '.\ff4.rom.smc' make -f "Onone Kmain/summon/moon Pnone Crelaxed Twild Swild Bstandard Etoggle Glife" --spoileronly 
}
```

#### Analyzer
Single folder:
```
FreeSpoilerAnalyzer.exe "C:\path\to\spoiler-folder"
```

Multiple folders:
```
FreeSpoilerAnalyzer.exe "~/path/to/spoiler/files" "~/path/to/more/spoiler/files"
```