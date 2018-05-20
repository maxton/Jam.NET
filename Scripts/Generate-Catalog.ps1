Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -Path "$PSScriptRoot\..\packages\plist-cil.1.16.0\lib\net40\plist-cil.dll"

$zipFiles = Get-ChildItem *.zip
$processed = @()
$unprocessed = @()
foreach ($zipFile in $zipFiles) {
    try {
        $archive = [System.IO.Compression.ZipFile]::OpenRead($zipFile)
        $entry = $archive.Entries | Where-Object { $_.Name -eq 'info.plist' }
        $songInfo = [Claunia.PropertyList.PropertyListParser]::Parse($entry.Open())
    
        $object = @{
            'id' = Split-Path -Path $zipFile -Leaf
            'artist' = $songInfo['artist'].Content;
            'album' = $songInfo['album'].Content;
            'title' = $songInfo['title'].Content;
            'instrument' = [int]$songInfo['instrument']
        }
    
        $processed += $object
    }
    catch {
        $unprocessed += $zipFile
    }

}

Out-File -Encoding utf8 -FilePath 'catalog.json' -InputObject (ConvertTo-Json $processed)
Write-Output "Processed $($processed.Length) files."

if ($unprocessed.Length -gt 0) {
    Write-Output "$($unprocessed.Length) errors encountered:"
    Write-Output $unprocessed
}