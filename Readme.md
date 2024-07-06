# Free Enterprise Spoiler Analyzer

This project is constructed to analyze spoiler logs for Free Enterprise in a way that helps validate the effect of code changes that require generating lots of seeds to see statistical changes. 

This is **not** a project to help in spoiler log races, and as such will generally work with aggregated data. Also, the parser looks for files ending in .spoiler.private, which is what the command line generator for FE will supply when generating seeds. 

## Usage
Run the command line application, supplying any Folders that contain spoiler logs to analyze. When finished, it will report the total number of logs examined, and how many had Darkness Crystal 

## Possible Future Updates
 * Add support for correctly analyzing seeds with Knofree enabled
 * Add support for selecting a different key item at runtime
 * Add support for saving results to a database


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
FreeSpoilerAnalyzer.exe "C:\path\to\spoiler-folder" "C:\path\to\another\spoiler-folder"
```