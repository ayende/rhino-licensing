# Helper script for those who want to run
# psake without importing the module.

param(
    [Parameter(Position=0,Mandatory=0)]
    [string[]]$taskList = @(),
    [Parameter(Position=1,Mandatory=0)]
    [string]$framework = '4.0',
    [Parameter(Position=2,Mandatory=0)]
    [switch]$docs = $false,
    [Parameter(Position=3,Mandatory=0)]
    [System.Collections.Hashtable]$parameters = @{},
    [Parameter(Position=4, Mandatory=0)]
    [System.Collections.Hashtable]$properties = @{},
    [Parameter(Position=5, Mandatory=0)]
    [switch]$nologo = $false,
    [Parameter(Position=6, Mandatory=0)]
    [switch]$help = $false,
    [Parameter(Position=7, Mandatory=0)]
    [string]$scriptPath = $(Split-Path -parent $MyInvocation.MyCommand.path)
)

$buildFile = '.\default.ps1'

remove-module psake
import-module (join-path $scriptPath psake.psm1)

if ($help) {
  Get-Help Invoke-psake -full
  return
}

if (-not(test-path $buildFile)) {
    $absoluteBuildFile = (join-path $scriptPath $buildFile)
    if (test-path $absoluteBuildFile)	{
        $buildFile = $absoluteBuildFile
    }
} 

invoke-psake $buildFile $taskList $framework
