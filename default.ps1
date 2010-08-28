properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build" 
  $buildartifacts_dir = "$build_dir\" 
  $sln_file = "$base_dir\Rhino.Licensing.sln" 
  $version = "1.2.0.0"
  $humanReadableversion = "1.2"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $uploadCategory = "Rhino-Mocks"
  $uploadScript = "C:\Builds\Upload\PublishBuild.build"
} 

include .\psake_ext.ps1
	
task default -depends Release

task Clean { 
  remove-item -force -recurse $buildartifacts_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
	
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Licensing\Properties\AssemblyInfo.cs" `
		-title "Rhino Licensing $version" `
		-description "Licensing Framework for .NET" `
		-company "Hibernating Rhinos" `
		-product "Rhino Licensing $version" `
		-version $version `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
		
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Licensing.Tests\Properties\AssemblyInfo.cs" `
		-title "Rhino Licensing $version" `
		-description "Licensing Framework for .NET" `
		-company "Hibernating Rhinos" `
		-product "Rhino Licensing $version" `
		-version $version `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
		
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Licensing.AdminTool\Properties\AssemblyInfo.cs" `
		-title "Rhino Licensing $version" `
		-description "Licensing Framework for .NET" `
		-company "Hibernating Rhinos" `
		-product "Rhino Licensing $version" `
		-version $version `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
		
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Licensing.AdminTool.Tests\Properties\AssemblyInfo.cs" `
		-title "Rhino Licensing $version" `
		-description "Licensing Framework for .NET" `
		-company "Hibernating Rhinos" `
		-product "Rhino Licensing $version" `
		-version $version `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
		
	new-item $release_dir -itemType directory 
	new-item $buildartifacts_dir -itemType directory 
	cp $tools_dir\xunit\*.* $build_dir
} 

task Compile -depends Init { 
  exec msbuild "/p:OutDir=""$buildartifacts_dir "" $sln_file"
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  exec "$tools_dir\xUnit\xunit.console.exe" "$build_dir\Rhino.Licensing.Tests.dll"
  exec "$tools_dir\xUnit\xunit.console.exe" "$build_dir\Rhino.Licensing.AdminTool.Tests.dll"
  cd $old		
}


task Release -depends Test {
	& $tools_dir\zip.exe -9 -A -j `
		$release_dir\Rhino.Licensing-$humanReadableversion-Build-$env:ccnetnumericlabel.zip `
		$build_dir\Rhino.Licensing.dll `
		$build_dir\Rhino.Licensing.xml `
		license.txt `
		acknowledgements.txt
		
	& $tools_dir\zip.exe -9 -A -j `
		$release_dir\Rhino.Licensing-AdminTool-$humanReadableversion-Build-$env:ccnetnumericlabel.zip `
		$build_dir\Caliburn.Core.dll `
		$build_dir\Caliburn.PresentationFramework.dll `
		$build_dir\Caliburn.ShellFramework.dll `
		$build_dir\Caliburn.Windsor.dll `
		$build_dir\Castle.Core.dll `
		$build_dir\Castle.DynamicProxy2.dll `
		$build_dir\Castle.MicroKernel.dll `
		$build_dir\Castle.Windsor.dll `
		$build_dir\log4net.dll `
		$build_dir\Microsoft.Practices.ServiceLocation.dll `
		$build_dir\Rhino.Licensing.AdminTool.exe `
		license.txt `
		acknowledgements.txt
		
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }
}

task Upload -depend Release {
	if (Test-Path $uploadScript ) {
		$log = git log -n 1 --oneline		
		msbuild $uploadScript /p:Category=$uploadCategory "/p:Comment=$log" "/p:File=$release_dir\Rhino.Mocks-$humanReadableversion-Build-$env:ccnetnumericlabel.zip"
		
		if ($lastExitCode -ne 0) {
			throw "Error: Failed to publish build"
		}
	}
	else {
		Write-Host "could not find upload script $uploadScript, skipping upload"
	}
}