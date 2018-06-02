Add-Type -AssemblyName System.IO.Compression.FileSystem
Add-Type -Path "$PSScriptRoot\..\packages\plist-cil.1.16.0\lib\net40\plist-cil.dll"

$instrumentIds = @{
    0 = 'guitar';
    1 = 'bass';
    2 = 'drums';
    3 = 'keyboard';
    4 = 'vocal'
}

$zipFiles = Get-ChildItem *.zip
$processed = @()
$unprocessed = @()
foreach ($zipFile in $zipFiles) {
    try {
        $archive = [System.IO.Compression.ZipFile]::OpenRead($zipFile)
        $entry = $archive.Entries | Where-Object { $_.Name -eq 'info.plist' }
        $songInfo = [Claunia.PropertyList.PropertyListParser]::Parse($entry.Open())

        $id = Split-Path -Path $zipFile -Leaf
        if ($id -like '*-*-*-*-*.zip' -and $id.Length -eq 40) {
            $id = $id.Substring(0, 36)
        }
        $instrumentId = [int]$songInfo['instrument']
        $object = @{
            'id' = $id
            'artist' = $songInfo['artist'].Content
            'album' = $songInfo['album'].Content
            'title' = $songInfo['title'].Content
            'instrumentId' = $instrumentId
            'instrument' = $instrumentIds[$instrumentId]
            'sku' = $songInfo['sku'].Content
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