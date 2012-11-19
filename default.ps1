properties { 
  $product_name = "Rhino.Licensing"
  $base_dir = Resolve-Path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build"
  $buildartifacts_dir = "$build_dir\"
  $sln_file = "$base_dir\$product_name.sln"
  $version = "1.2.0.0"
  $humanReadableversion = "1.2"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $uploadCategory = "Rhino-Mocks"
  $uploadScript = "C:\Builds\Upload\PublishBuild.build"
  $xunit = "$tools_dir\xUnit\xunit.console.clr4.exe"
  $nuget = "$tools_dir\NuGet.exe"
  $nuget_project = "$base_dir\$product_name\$product_name.csproj"
} 

include .\psake_ext.ps1

task default -depends Release

task Nuget-Pack -depends Release {
  write-host "Generating nuget artefacts..."
  exec { invoke-expression "$nuget pack $nuget_project -OutputDirectory $release_dir" }
}

task Nuget-Publish  { #-depends Nuget-Pack
  write-host "Publishing nuget package at $release_dir\$product_name.$humanReadableversion.nupkg..."
  exec { invoke-expression "$nuget push $release_dir\$product_name.$humanReadableversion.nupkg" }
}

task Clean { 
  write-host "lib folder is $lib_dir"

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
		-clsCompliant "false" `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004 - 2009"
		
	new-item $release_dir -itemType directory 
	new-item $buildartifacts_dir -itemType directory 
	cp $tools_dir\xunit\*.* $build_dir
} 

task Compile -depends Init { 
  exec { msbuild /p:OutDir=$buildartifacts_dir $sln_file  }
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  
  exec { invoke-expression "$xunit $build_dir\Rhino.Licensing.Tests.dll /noshadow" }  
  exec { invoke-expression "$xunit $build_dir\Rhino.Licensing.AdminTool.Tests.dll /noshadow" }
  
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