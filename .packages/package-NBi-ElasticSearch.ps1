param(
    [parameter(Mandatory=$true)]
    [string]$version
)

$root = (split-path -parent $MyInvocation.MyCommand.Definition)

Write-Host "Calculating dependencies ..."

$dependencies = @{}
$solutionRoot = Join-Path ($root) ".."
Write-Host "Looking for projects in $solutionRoot ..."
$projects = Get-ChildItem $solutionRoot | ?{ $_.PSIsContainer -and $_.Name -like "NBi.Core.Elasticsearch*"-and $_.Name -notLike "*Testing*"} | select Name, FullName
foreach($proj in $projects)
{
    $projName = $proj.name
    Write-Host "Looking for dependencies in project $projName ..."
    $path = Join-Path ($proj.FullName) "packages.config"
        
    if(Test-Path $path)
    {
        [xml]$packages = Get-Content $path
        foreach($package in $packages.FirstChild.NextSibling.ChildNodes)
        {
            if (!$dependencies.ContainsKey($package.id)) {$dependencies.add($package.id, "<dependency id=""$($package.id)"" version=""$(($package.allowedVersions, $package.version -ne $null)[0])"" />")}
        }
    }
}

Write-Host "Found $($dependencies.Count) dependencies ..."
$depList = $dependencies.Values -join [Environment]::NewLine + "`t`t"

#For NBi.Elasticsearch (dll)
$lib = "$root\NBi.Elasticsearch\lib"
If (Test-Path $lib)
{
	Remove-Item $lib -recurse
}

new-item -Path $root\..\.nupkg -ItemType directory -force
new-item -Path $lib\net46\ -ItemType directory
Copy-Item $root\..\NBi.Core.Elasticsearch\bin\Debug\NBi.*Elasticsearch*.dll $lib\net46\

Write-Host "Setting .nuspec version tag to $version"

$content = (Get-Content $root\NBi.Elasticsearch\NBi.Elasticsearch.nuspec -Encoding UTF8) 
$content = $content -replace '\$version\$',$version
$content = $content -replace '\$depList\$',$depList

$content | Out-File $root\NBi.Elasticsearch\NBi.Elasticsearch.compiled.nuspec -Encoding UTF8

& NuGet.exe pack $root\..\.packages\NBi.Elasticsearch\NBi.Elasticsearch.compiled.nuspec -Version $version -OutputDirectory $root\..\.nupkg
