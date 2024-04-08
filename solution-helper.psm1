<#
 .SYNOPSIS
  Solution helpers.

 .DESCRIPTION
  Commands for managing nugets. 
  These methods may be executed from the `Developer PowerShell` terminal window.
  Import-Module ./solution-helper
#>

function Publish-Packages() {
    Param(
        [Parameter(Position = 0)]
        [Alias("c")]
        [string] $Configuration = 'Debug',

        [Alias("t")]
        [string] $Tag = 'local'
    )

	try {
        $Tag = ($Tag -replace '\s+', '').ToLower()
		Write-Host "Building and publishing packages for '$Configuration' with tag '$Tag'."

        $timestamp = [System.DateTime]::UtcNow.ToString( 'yyMMddHHmmss' )

        if ( !$Tag ) {
            Write-Error "Non-semver publication is not supported."
            throw
        }

        dotnet pack --no-build --configuration $Configuration --output ./output --version-suffix "$Tag.$timestamp" -p:PushAfterPack=true
	}
	catch {
		Write-Error "Publish-Packages failed. Make sure you are executing from a `Developer PowerShell` session."
	}
}

function Resize-Feed() {
    Param(
        [string] $Name = '*',
        [int] $Keep = 5,
        [string] $Source = 'local'
    )

	Write-Host "Collecting package versions from $source ..."

    # get unique packages
    $packages = Find-Package $Name -source $Source

    foreach( $package in $packages ) {
        $packageName = $package.Name

        # get all versions for this package

        $sortExpr = { param($version) # convert '1.2.3-..' to '00001-00002-00003-..' 
            $parts = $version.Split('-')
            $key = ( $parts[0].Split('.') | ForEach-Object { $_.PadLeft(5,'0') } ) -join '-'
            $key,$parts[1] -join '-'
        }

        $versions = Find-Package $packageName -source $source -allversions | Sort-Object { &$sortExpr -version $_.Version } -Descending

        Write-Host "Found '$packageName'. $($versions.Count) Packages."
        
        if ( $versions.Count -gt $Keep ) {
            $removeCount = $versions.Count - $Keep
            Write-Host "$removeCount Packages will be removed."

            foreach( $p in ($versions | Select-Object -Skip $Keep ) ) {
                dotnet nuget delete $p.Name $p.Version --source $Source --non-interactive
            }
        }
    }
}

function Update-Version() {
   Param(
        [Parameter(Position = 0, Mandatory=$true)]
        [ValidateSet('Major','Minor','Patch', IgnoreCase = $true)]
        [string] $Type,
        [string] $Path = 'Directory.Build.props',
        [switch] $Commit
    )
    
    try {
        if (!(Test-Path $Path)) {
            Write-Error "The version file '$Path' was not found in the current directory."
            throw
        }

        $Type = (Get-Culture).TextInfo.ToTitleCase($Type) # e.g. convert 'major' to 'Major'
        $propName = $Type + "Version"

        $xml = [xml](Get-Content $Path)
        $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
        $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)

        $node = $xml.SelectSingleNode("//ns:Project/ns:PropertyGroup[ns:$propName]", $ns)
        $version = $node.$propName -as [Int]

        $previousVersionString = "v$($node.MajorVersion).$($node.MinorVersion).$($node.PatchVersion)"

        $node.$propName = ($version + 1) -as [String]

        if ( $Type -eq 'major' ) {
            $node.MinorVersion = '0'
            $node.PatchVersion = '0'
        }

        if ( $Type -eq 'minor' ) {
            $node.PatchVersion = '0'
        }

        $newVersionString = "v$($node.MajorVersion).$($node.MinorVersion).$($node.PatchVersion)"
        Write-Host "Previous version was '$previousVersionString'. Version now '$newVersionString'."

        $xml.Save($Path)

        if ( $Commit ) {
            git add $Path
            git commit -m "bump $($Type.ToLower())" -q -o $Path
        }

        return $previousVersionString, $newVersionString
    }
    catch {
		Write-Error "Update-Version failed. Make sure you are executing from a `Developer PowerShell`."
	}
}

function Set-Version() {
    Param(
        [Parameter(Position = 0,Mandatory=$true)]
        [string] $Version,
        [string] $Path = 'Directory.Build.props',
        [switch] $Commit
    )
        
    try {
        if (!(Test-Path $Path)) {
            Write-Error "The version file '$Path' was not found in the current directory."
            throw
        }

        # Remove any non-numeric characters from the version string
        $Version = $Version -replace '[^0-9.]', ''

        # Split the version string into major, minor, and patch versions
        $MajorVersion, $MinorVersion, $PatchVersion = $Version.Split('.')

        $xml = [xml](Get-Content $Path)
        $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
        $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)

        $node = $xml.SelectSingleNode("//ns:Project/ns:PropertyGroup", $ns)

        $previousVersionString = "v$($node.MajorVersion).$($node.MinorVersion).$($node.PatchVersion)"

        # Update the version numbers
        $node.MajorVersion = $MajorVersion
        $node.MinorVersion = $MinorVersion
        $node.PatchVersion = $PatchVersion

        $newVersionString = "v$($node.MajorVersion).$($node.MinorVersion).$($node.PatchVersion)"
        Write-Host "Previous version was '$previousVersionString'. Version now '$newVersionString'."

        $xml.Save($Path)
        
        if ( $Commit ) {
            git add $Path
            git commit -m "bump $($Type.ToLower())" -q -o $Path
        }

        return $previousVersionString, $newVersionString
    }
    catch {
        Write-Error "Set-Version failed. Make sure you are executing from a `Developer PowerShell`."
    }
}

function Get-Version() {
    Param(
        [string] $Path = 'Directory.Build.props'
    )
        
    try {
        if (!(Test-Path $Path)) {
            Write-Error "The version file '$Path' was not found in the current directory."
            throw
        }

        $xml = [xml](Get-Content $Path)
        $ns = New-Object System.Xml.XmlNamespaceManager($xml.NameTable)
        $ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)

        $node = $xml.SelectSingleNode("//ns:Project/ns:PropertyGroup", $ns)

        $versionString = "v$($node.MajorVersion).$($node.MinorVersion).$($node.PatchVersion)"
        Write-Host "Current version is '$versionString'."

        return $versionString
    }
    catch {
        Write-Error "Get-Version failed. Make sure you are executing from a `Developer PowerShell`."
    }
}

Export-ModuleMember -Function 'Publish-Packages'
Export-ModuleMember -Function 'Resize-Feed'
Export-ModuleMember -Function 'Update-Version'
Export-ModuleMember -Function 'Set-Version'
Export-ModuleMember -Function 'Get-Version'